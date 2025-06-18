#ifndef RAIN_DROP_INCLUDED
#define RAIN_DROP_INCLUDED

//The noise function used for controlling the randomly appearance of rain dots
//position:2d posiiton used for generating the noise, must be floored to int
//return:2d noise between 0 and 1
float2 UVNoise(float2 position)
{
    position = fmod(position, 2048.0f);
    float scale = 0.5;
    float magic = 3571.0;
    float2 random = (1.0 / 4320.0) * position + float2(0.25, 0.0);
    random = frac(dot(random * random, magic));
    random = frac(dot(random * random, magic));
    return /*-scale + 2.0 * scale **/ random;
}

//Raindrop texture
TEXTURE2D(_DropTex);
SAMPLER(sampler_DropTex);

//Curl noise texture,used for distorting the raindrops
TEXTURE2D(_NoiseTex);
SAMPLER(sampler_NoiseTex);

CBUFFER_START(RainDrop)
float _TimeScale;
float _DropScale;
float _TurbulenceScale;
float _IndirectSpecularScale;
float _Translucency;
float _SmoothnessAdd;
float _DropLightingScale;
float _DotSize;
float _DotRepeat;
float4x4 _PrevMatrix;
float4x4 _TargetMatrix;
float _LerpWeight;
CBUFFER_END

//The function used for generating tiling rain dots
//RepeatRate: the tiling scale of rain dots
//DotSize: size scale,controls the dot size relative to tiling grid
//UV: the coordinate used for locating the dots,usually it should be a 2d position
//oNormal: output the normal of rain dots
//return: mask of dot 
half HalfTone(half RepeatRate, half DotSize, half2 UV, out half3 oNormal)
{
    half size = 1.0 / RepeatRate;
    half2 cellSize = half2(size, size);
    half2 cellCenter = cellSize * 0.5;

    half2 uvlocal = fmod(abs(UV), cellSize);
    half dist = length(uvlocal - cellCenter);
    half radius = cellCenter.x * DotSize;
    half threshold = length(ddx(dist) - ddy(dist))/*0.002*/;
    half relDist = dist / radius;
    half2 planeNormalXZ = (uvlocal - cellCenter) / dist;
    half3 planeNormal = normalize(half3(planeNormalXZ.x, 0.25, planeNormalXZ.y));
    oNormal = normalize(lerp(half3(0, 1, 0), planeNormal, pow(relDist, 1)));
    return smoothstep(dist - threshold, dist + threshold, radius);
}

//make rain dot in particular plane
//uv: the coordinate used for locating the dots
//normalAxisValue: the value of axis z
//normalWS: world space normal of surface
//normalTS: tangent space normal of surface
//dotNormal: output the normal of rain dot
//dotWeight: output the weight of rain dot
void ProcessRainDots(float2 uv, half normalAxisValue, half3 normalWS, half3 normalTS, out half3 dotNormal, out half dotWeight)
{
    float2 uv2 = uv;
    float dotRadius;
    half2 cellSize = (1.0 / _DotRepeat).xx;
    half2 cellCenter = cellSize * 0.5;
    half cellRadius = cellCenter.x * _DotSize;
    half2 bias2 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uv2 * 10.0).xy;
    bias2.xy = bias2.xy * 2 - 1;
    uv2 += bias2.xy * 0.4 * cellRadius;
    uv2.xy += normalTS.xy * 0.5 * cellRadius;
    half dots = HalfTone(_DotRepeat, _DotSize, uv2, dotNormal);

    uv2 = floor(uv2 / cellSize) * cellSize + cellCenter;

    float noise = UVNoise(floor(uv2 * 10000)).x;
    //Here you can alter this '0.06' if you want the rain dots to be changed in different speed
    noise = 1 - frac(noise + _Time.y * 0.06);
    //magic operation to make the distribution more natural
    dots = saturate((noise - 0.8) * dots * 5.0);
    dots *= saturate((abs(normalAxisValue) - 0.3) * 1.429);

    dotWeight = dots;
}

//The function used for generating the mask and normal of raindrops
//posWS: world space position, here you can use the object space position as well,or use object space position added by an offset
//geoNormal: geometry normal of surface,ignoring the normal map
//normalTS:detail normal in tangent space,if you didn's use any normalmap,you can leave this to half3(0,0,1)
//normalWS:world space normal,considering the normalmap
//dropnormal: output the world space normal of raindrops
//return: raindrop mask,can be used to determine the raindrop area 
half RainDropMask(float3 posWS, half3 geoNormal, half3 normalTS, half3 normalWS, out half3 dropNormal)
{
    float3 originPosWS = posWS;
    //Sample curl noise in two project directions
    //You can alter those '0.5' to let the turbulence effect getting different frequency
    float2 uv0 = posWS.xy * 0.5;
    float2 uv1 = float2(posWS.z + 2.414, posWS.y) * 0.5;

    half2 bias0 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uv0).xy;
    half2 bias1 = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uv1).xy;
    bias0.xy = bias0.xy * 2 - 1;
    bias1.xy = bias1.xy * 2 - 1;
    //a bit influenced by tangent space normal
    //you can edit this '0.2' if necessary
    bias0.xy += normalTS.xy*0.2;
    bias1.xy += normalTS.xy*0.2;

    //offseting y position by time,so raindrops can sliding down
    posWS.y += _Time.y * _TimeScale;
    //calculate the texture coordinates of raindrops by position
    uv0 = posWS.xy * _DropScale;
    uv1 = float2(posWS.z + 2.414, posWS.y) * _DropScale;

#ifdef _DRIP_ENABLE
    //Sample Drop texture to get drop normals and mask
    half3 mask0 = SAMPLE_TEXTURE2D(_DropTex, sampler_DropTex, uv0 + bias0 * _TurbulenceScale).xyz;
    half3 dropNormal0;
    dropNormal0.xy = mask0.xy * 2.0 - 1.0;
    dropNormal0.z = pow(sqrt(saturate(1 - dot(dropNormal0.xy, dropNormal0.xy))),0.2) * sign(geoNormal.z);

    half3 mask1 = SAMPLE_TEXTURE2D(_DropTex, sampler_DropTex, uv1 + bias1 * _TurbulenceScale).xyz;
    half3 dropNormal1;
    dropNormal1.zy = mask1.xy * 2 - 1;
    dropNormal1.x = pow(sqrt(saturate(1 - dot(dropNormal1.zy, dropNormal1.zy))),0.2) * sign(geoNormal.x);

    //lerp normal between two directions
    half orientation = step(abs(geoNormal.z), abs(geoNormal.x));
    half weight0 = saturate((abs(geoNormal.z) - 0.3)*1.429);
    half weight1 = saturate((abs(geoNormal.x) - 0.3)*1.429);
    half weightSum = weight0 + weight1;
    weight0 /= weightSum + 0.0001;
    weight1 /= weightSum + 0.0001;
    half mask = weight0 * mask0.z + weight1 * mask1.z;//lerp(mask0.z, mask1.z, orientation);
    mask = saturate(mask * 2);
    dropNormal = dropNormal0 * mask0.z + dropNormal1 * mask1.z;//lerp(dropNormal0, dropNormal1, orientation);

    dropNormal = lerp(normalWS, dropNormal, mask);
    dropNormal = normalize(dropNormal);
#else
    half mask = 0;
    dropNormal = normalWS;
#endif

    //create drop dots
#ifdef _DOT_ENABLE
    half dotWeightX;
    half3 dotNormalX;
    ProcessRainDots(originPosWS.yz, geoNormal.x, normalWS, normalTS, dotNormalX, dotWeightX);
    dotNormalX = dotNormalX.yxz;
    dotNormalX.x *= sign(geoNormal.x);

    half dotWeightY;
    half3 dotNormalY;
    ProcessRainDots(originPosWS.xz, geoNormal.y, normalWS, normalTS, dotNormalY, dotWeightY);
    dotNormalY = dotNormalY.xyz;
    dotNormalY.xz *= -1;
    dotNormalY.y *= sign(geoNormal.y);

    half dotWeightZ;
    half3 dotNormalZ;
    ProcessRainDots(originPosWS.xy, geoNormal.z, normalWS, normalTS, dotNormalZ, dotWeightZ);
    dotNormalZ = dotNormalZ.xzy;
    dotNormalZ.x *= -1;
    dotNormalZ.z *= sign(geoNormal.z);

    half dots = dotWeightX + dotWeightY + dotWeightZ;
    half3 dotNormal = (dotNormalX * dotWeightX + dotNormalY * dotWeightY + dotNormalZ * dotWeightZ) / max(dots, 0.001);

    dotNormal = normalize(lerp(normalWS, dotNormal, dots));

    //lerp from raindrops to raindots based on normalY
    half lerpY = saturate((abs(geoNormal.y) - 0.5) * 2.0);
    mask = lerp(mask, 0, lerpY);
    //mask = saturate(mask * 2);
    dropNormal = normalize(lerp(dotNormal, dropNormal, mask));
    mask = max(dots, mask);
#endif

    return mask;
}

#endif