// This Shader is written by Chenghao Li, Refering to tutorial online
// https://roystan.net/articles/toon-shader.html
// Used for COMP30019 Project2. 
// Nov. 2020


Shader "SelfWritten/ToonShader"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}

		//AMbientLight Part	
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716

		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Tags
		{
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;  //Access Normal
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Glossiness;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _RimThreshold;

			float4 frag (v2f i) : SV_Target
			{
				//Convert to world normal
				float3 normal = normalize(i.worldNormal);
				// Dot Product to calculate light strength
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				//Check if is underlight condition:
				float isLit = NdotL > 0 ? 1 : 0;    // The Dark or Light one

				
				// @ Shadow
				float shadow = SHADOW_ATTENUATION(i);
				
				// @ Light Strenght:
				// Alter 1: the color changes depends on light effect in 4 stages.
				float lightStrength; 
					if ( NdotL>0 && NdotL < 0.4){
						lightStrength = 0.5;
					}
					else if (NdotL >= 0.4){
						lightStrength = 1.0;
					}
					else if (NdotL > -0.4){
						lightStrength = 0.1;
					}
					else{
						lightStrength = 0;
					}

				// Alter 2:
				lightStrength = smoothstep(0, 0.01, lightStrength);
				float4 light = lightStrength * _LightColor0 * shadow;

				

				// @ Texture
				float4 sample = tex2D(_MainTex, i.uv);
				
				// @ Blinn-Phong Reflection Effect:
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float BlinnStrength = dot(normal, halfVector);
				float specularIntensity = pow(BlinnStrength * isLit * shadow, _Glossiness * _Glossiness);
				float4 specular = smoothstep(0.005, 0.01, specularIntensity) * _SpecularColor; // made the reflection with edges

				// @ RimEffect
				float4 rimLight = 1-dot(viewDir, normal);
				float rimIntensity = rimLight * pow(BlinnStrength, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;
				


				return _Color * sample * (_AmbientColor + light + specular + rim);

			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}