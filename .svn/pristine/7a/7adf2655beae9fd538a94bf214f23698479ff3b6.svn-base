Shader "UI/SparkleShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _GlowIntensity("Glow Intensity", Float) = 1.0
        _Speed("Speed", Float) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }
            Blend SrcAlpha OneMinusSrcAlpha
            Pass
            {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _Color;
                float _GlowIntensity;
                float _Speed;

                Varyings vert(Attributes v)
                {
                    Varyings o;
                    o.positionHCS = TransformObjectToHClip(v.positionOS);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(Varyings i) : SV_Target
                {
                    float2 uv = i.uv;
                    float time = _Time.y * _Speed;
                    float glow = abs(sin(time)) * _GlowIntensity;

                    float4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                    float4 finalColor = baseColor * _Color;
                    finalColor.rgb += glow;

                    return finalColor;
                }
                ENDHLSL
            }
        }

            FallBack "UI/Default"
}
