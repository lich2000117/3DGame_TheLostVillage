Shader "Unlit/shield"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,1)
        _MainTex ("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture" , 2D) = "white" {}
        _Fresnel("Fresnel Intensity", Range(0,100)) = 3.0
        _FresnelWidth("Fresnel Width", Range(0,5)) = 1
        _Distort("Distort" , Range(0,100)) = 1
        _IntersectionThreshold("Intersection Threshold Highlight", Range(0,1)) = .1
        _Threshold("Threshold", Range(0,1)) = 1
        _ScrollSpeedU("U Speed", float) = 2
        _ScrollSpeedV("V Speed", float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }
        //LOD 100

        GrabPass{ "_GrabTexture"}

        Pass
        {
            Lighting Off
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            Cull off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
                float4 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
                fixed3 rimColor : TEXCOORD2;
                fixed4 screenPos : TEXCOORD3;
            };

            sampler2D _MainTex, _CameraDepthTexture, _GrabTexture, _MaskTex;
            float4 _MainTex_ST, _MainColor, _GrabTexture_ST, _GrabTexture_TexelSize, _MaskTex_ST;
            fixed _Fresnel, _FresnelWidth, _Distort, _IntersectionThreshold, _ScrollSpeedU, _ScrollSpeedV, _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _MaskTex);

                //scroll UV
                o.uv.x += _Time*_ScrollSpeedU;
                o.uv.y += _Time*_ScrollSpeedV;
                o.uv2.x += _Time*_ScrollSpeedU;
                o.uv2.y += _Time*_ScrollSpeedV;

                //Fresnel
                fixed3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                fixed dotProduct = 1- saturate(dot(v.normal, viewDir));
                o.rimColor = smoothstep(1-_FresnelWidth, 1.0, dotProduct) * .5f;
                o.screenPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);          //eye space depth of the vertex

                return o;
            }

            fixed4 frag (v2f i, fixed face:VFACE) : SV_Target
            {
                //intersection
                fixed intersect = saturate((abs(LinearEyeDepth(
                                                                tex2Dproj(_CameraDepthTexture,i.screenPos).r
                                                            ) - i.screenPos.z
                                                )
                                            ) / _IntersectionThreshold);
                
                fixed3 main = tex2D(_MainTex, i.uv);

                //distortion
                 i.screenPos.xy += (main.rg*2-1)* _Distort *_GrabTexture_TexelSize.xy;
                 fixed3 distortColor = tex2Dproj(_GrabTexture, i.screenPos);
                 distortColor *= _MainColor * _MainColor.a + 1;

                //intersect hightlight
                i.rimColor *= intersect* clamp(0,1,face);
                main *= _MainColor * pow(_Fresnel, i.rimColor);

                //lerp distort color & fresnel color
                main = lerp(distortColor, main, i.rimColor.r);
                main += (1-intersect) * (face>0?0.03 : 0.3) * _MainColor * _Fresnel;

                fixed4 singleCol = tex2D(_MaskTex, i.uv2).x;
                
                fixed4 finalCol = singleCol>=_Threshold?fixed4(main,0.9):fixed4(main,0);
                
                return finalCol;
            }
            ENDCG
        }
    }
}
