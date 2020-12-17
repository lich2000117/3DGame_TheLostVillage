Shader "Unlit/waterFinal"
{
    Properties
    {
        _DiffuseColor("Diffuse Color", Color) = (0,0,0.8,0.5)
        _MainTex("Main Texture", 2D) = "white" {}
        _MainTiling("Main Tex Tiling", Vector) = (0.01,0.01,0.013,0.013)
        _MainTexSpeed("Main Tex Speed", Range(1,10)) = 3

        _FoamEdgeTex("Foam Edge Texture",2D) = "white" {}

        _BumpTex("Bump Texture", 2D) = "bump" {}
        _BumpStrength("Bump Strength", Range(0.0,5.0)) = 1.0
        _BumpDirection("Bump Direction", Vector) = (0.01,0.01,0.02,-0.02)
        _BumpTiling("Bump Tiling", Vector) = (0.01,0.01,0.013,0.013)

        _QA("Steepness [0,1]", Vector) = (0.2,0.2,0.2,0.2)                           //the steepness of wave
		_A("Amplitude", Range(0,1)) = 0.5                                           //the height from the water plane to the wave crest.
		_Dx("Direction-x components ", Vector) = (0.5,0.3,0.1,0.4)                  //directions of four waves, the direction of first wave is (_Dx.x,_Dz.x)
		_Dz("Direction-z components ", Vector) = (0.5,0.3,0.2,0.2)
		_S("Wave Speed", Range(1,100)) = 5                                             //the distance the crest moves forward per second. It is convenient to express speed as phase-constant , where phase-constant = S x 2/L.
		_L("Wave Length", Range(1,10)) = 1                                            //the crest-to-crest distance between waves in world space. 
    
        _DistortionFactor("Distortion Factor", Range(1,100)) = 1

        _FresnelFactor("FresnelFactor", float) = 0.5
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _Gloss("Gloss", Range(1.0,256)) = 10

        _Skybox("Skybox", CUBE) = "white"{}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" "LightMode" = "ForwardBase" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        //ZWrite Off
        //Cull off

        LOD 100

        GrabPass{ }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "waterMethodsFinal.cginc"

            float4 _QA;
            float _A;
            float _S;
            float _L;
            float4 _Dx;
            float4 _Dz;

            fixed4 _DiffuseColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTiling;
            float _MainTexSpeed;
            sampler2D _BumpTex;
            float4 _BumpTex_ST;
            half4 _BumpDirection;
		    half4 _BumpTiling;
            half _BumpStrength;

            //深度贴图
            uniform sampler2D_float _CameraDepthTexture;
            sampler2D _FoamEdgeTex;

            //GrabPass
		    sampler2D _ReflectionTex;
            sampler2D _RefractionTex;               
            float4 _RefractionTex_TexelSize;  
            half _DistortionFactor;

            half _FresnelFactor;

            fixed4 _SpecularColor;
            half _Gloss;

            samplerCUBE _Skybox;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL; 
                float4 tangent: TANGENT;
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float3 worldnormal: NORMAL;
                float4 scrPos: TEXCOORD0;
                float2 uv: TEXCOORD1; 
                float4 bumpCoords: TEXCOORD2;
                float4 T2W0: TEXCOORD3;
                float4 T2W1: TEXCOORD4;
                float4 T2W2: TEXCOORD5;
                float depth:TEXCOORD6;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
			    float3 disPos = CalGersterWaves(worldPos, _QA, _A, _S, _Dx, _Dz, _L);
                v.vertex = mul(unity_WorldToObject, float4(worldPos+disPos, 1));

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeGrabScreenPos(o.vertex);
                o.worldnormal = normalize(mul((float3x3)unity_ObjectToWorld,v.normal));

                //UV 
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //Bump 
                o.bumpCoords.xyzw = (worldPos.xzxz*_BumpTiling.xyzw +_Time.yyyy* _BumpDirection.xyzw);

                //depth value
                COMPUTE_EYEDEPTH(o.depth);
                
                //construct a rotation matrix from tangent space to world space
                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
                fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                fixed3 worldBinormal = cross(worldNormal,worldTangent)*v.tangent.w;
                o.T2W0 = float4(worldTangent.x,worldBinormal.x,worldNormal.x, worldPos.x);
                o.T2W1 = float4(worldTangent.y,worldBinormal.y,worldNormal.y, worldPos.y);
                o.T2W2 = float4(worldTangent.z,worldBinormal.z,worldNormal.z, worldPos.z);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // the color 
                fixed4 col = fixed4(0,0,0,0);

                //get the bump tex normal in tangent space
                half3 bump = bumpNormal(_BumpTex, i.bumpCoords, _BumpStrength);

                //compute the offset in tangent space
                float2 offset = bump.xy * _DistortionFactor* _RefractionTex_TexelSize.xy;
                i.scrPos.xy = offset*i.scrPos.z + i.scrPos.xy;
                
                //refraction color
                fixed4 refractCol = tex2D(_RefractionTex, i.scrPos.xy/i.scrPos.w + offset);
                col += refractCol;

                
                //convert the bump tex normal to world space
                float3x3 Mat = { i.T2W0.xyz, i.T2W1.xyz, i.T2W2.xyz };
                float3 worldnormal = normalize(mul(Mat, normalize(bump)));

                float3 worldPos = float3(i.T2W0.w, i.T2W1.w, i.T2W2.w);
                float3 viewDir = normalize(worldPos - _WorldSpaceCameraPos.xyz);

                //fresnel (Schlick's approximation)
                float _FresnelBase = 1;
                float fresnel = _FresnelBase + (1-_FresnelBase)*pow(1.0f-saturate(dot(viewDir, worldnormal)), _FresnelFactor);

                //reflect
                float3 reflDir = reflect( viewDir, worldnormal);
                fixed3 reflectCol = texCUBE(_Skybox, reflDir);
                col.xyz = lerp(col.xyz, reflectCol.xyz, fresnel);

                //specular effect (Blinn-Phong)
                fixed4 ambient = UNITY_LIGHTMODEL_AMBIENT;                                                      //Ambient lighting color (sky color in gradient ambient case). Legacy variable.
                float3 worldLight = normalize(_WorldSpaceLightPos0.xyz);                                        //light direction in world space
                fixed3 diffuse = _LightColor0.rgb*_DiffuseColor.rgb*saturate(dot(i.worldnormal, worldLight));    //calculate diffuse col
                fixed3 halfVector = normalize(worldLight + (-viewDir));                                         //calculate half vector
                fixed3 specular = _SpecularColor * pow(max(0, dot(i.worldnormal, halfVector)), _Gloss);
                col.xyz *= diffuse + ambient + specular;

                //screen depth Z
                float ScreenZ = LinearEyeDepth( SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos))  );
                float _EdgeWidth = 0.1;
                float halfWidth = _EdgeWidth *0.5f;
                float diff = saturate(abs(i.depth - ScreenZ) / halfWidth);
                fixed mask = dot(i.worldnormal,float3(0,-1,0))+0.5;
                mask = mask>0.2?0:1;
                //Foam Edge Col
                fixed4 foamColEdge = texCross(_FoamEdgeTex,i.uv.xy, _MainTexSpeed);
                //Edge Col
                fixed4 edgeCol = mask>0?lerp(_EdgeWidth, _DiffuseColor, diff):_DiffuseColor;
                
                //main texture color
                fixed4 texcol = texCross(_MainTex, i.uv * _MainTiling , _MainTexSpeed);
                texcol.a = 0.8;
                texcol += foamColEdge.r*edgeCol;

                col *= texcol;
   
                return col;
            }
            ENDCG
        }
    }
}
