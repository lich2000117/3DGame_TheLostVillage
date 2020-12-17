#ifndef WATERMETHODS
#define WATERMETHODS



float3 CalGersterWaves(float3 vert, float4 _QA, float _A, float _S, float _L, float4 _Dx, float4 _Dz)
{
    float3 wave = float3(0,0,0);
    half w = 2* UNITY_PI/_L;                //frequency
    half phaseConstant = _S*2/_L;
    float4 phase = w*_Dx*vert.x + w*_Dz*vert.z + phaseConstant*_Time.y*float4(0.1,0.1,0.1,0.1);
    float4 sinp=float4(0,0,0,0);
    float4 cosp=float4(0,0,0,0);
    sincos(phase, sinp, cosp);
    wave.x = dot(_QA*_A*_Dx, cosp);
    wave.z = dot(_QA*_A*_Dz, cosp);
    wave.y = dot(_A, sinp);
    return wave;
}

fixed4 texCross(sampler2D tex, float2 uv, float _S)
{
    fixed3 col1 = tex2D(tex , uv - float2(_Time.x*_S , - _Time.x*_S)).rgb;

    fixed3 col2 = tex2D(tex , uv + float2(_Time.x*_S*0.8, -_Time.x*0.5*_S)).rgb;

    // float4 col3 = tex2D(tex , uv + float2(_Time.x*_S*0.84 , - _Time.x*0.11*_S));

    // float4 col4 = tex2D(tex , uv + float2(_Time.x*_S*0.105, -_Time.x*0.3515*_S));

    float4 col =  float4(((col1+col2)/2), 0.1);

    return col;
}

half3 bumpNormal(sampler2D bumpMap, half4 bumpCoords, half bumpStrength)
{
    //get the bump tex normal in tangent space
    half2 bump = (UnpackNormal(tex2D(bumpMap, bumpCoords.xy)) + UnpackNormal(tex2D(bumpMap, bumpCoords.zw)))*0.5; 
    bump = normalize(bump);
	half3 texNormal = half3(0, 1, 0);
	texNormal.xz = bump.xy * bumpStrength;
	return normalize(texNormal);
}
            


#endif