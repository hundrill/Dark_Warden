Shader "UI/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 col = 0;
                float2 offset = _BlurSize / _ScreenParams.xy;

                // Sample the texture multiple times to create a blur effect
                col += tex2D(_MainTex, uv + float2(-offset.x, -offset.y)) * 0.0625;
                col += tex2D(_MainTex, uv + float2(-offset.x, offset.y)) * 0.0625;
                col += tex2D(_MainTex, uv + float2(offset.x, -offset.y)) * 0.0625;
                col += tex2D(_MainTex, uv + float2(offset.x, offset.y)) * 0.0625;
                col += tex2D(_MainTex, uv) * 0.75;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
