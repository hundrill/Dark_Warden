Shader "AppsTools/Short-Range Skills VFX Effect Bundle 3/Dissolve"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess("Shininess", Range(0.03, 1)) = 0.078125
        _Amount("Amount", Range(0, 1)) = 0.5
        _StartAmount("StartAmount", float) = 0.1
        _Illuminate("Illuminate", Range(0, 1)) = 0.5
        _Tile("Tile", float) = 1
        _DissColor("DissColor", Color) = (1,1,1,1)
        _ColorAnimate("ColorAnimate", vector) = (1,1,1,1)
        _MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
        _BumpMap("Normalmap", 2D) = "bump" {}
        _DissolveSrc("DissolveSrc", 2D) = "white" {}
        _DissolveSrcBump("DissolveSrcBump", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 400
        cull off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BumpMap;
            sampler2D _DissolveSrc;
            sampler2D _DissolveSrcBump;

            fixed4 _Color;
            half4 _DissColor;
            half _Shininess;
            half _Amount;
            static half3 Color = float3(1,1,1);
            half4 _ColorAnimate;
            half _Illuminate;
            half _Tile;
            half _StartAmount;


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv_MainTex : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = v.uv;
                return o;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float4 tex = tex2D(_MainTex, IN.uv_MainTex);
                float3 Albedo = tex.rgb * _Color.rgb;

                float ClipTex = tex2D(_DissolveSrc, IN.uv_MainTex / _Tile).r;
                float ClipAmount = ClipTex - _Amount;
                float Clip = 0;
                float4 DematBump = tex2D(_DissolveSrcBump,IN.uv_MainTex / _Tile);
                //o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

                //fixed3 t_normal = UnpackNormal(tex2D(_DissolveSrcBump, IN.uvDissolveSrc));

                if (tex.a * _Color.a < 0.1) {
                    Clip = 1;
                }

                if (_Amount > 0)
                {
                    if (ClipAmount < 0)
                    {
                        Clip = 1; //clip(-0.1);
                    }
                     else
                     {
                        if (ClipAmount < _StartAmount)
                        {
                            if (_ColorAnimate.x == 0)
                                Color.x = _DissColor.x;
                            else
                                Color.x = ClipAmount / _StartAmount;

                            if (_ColorAnimate.y == 0)
                                Color.y = _DissColor.y;
                            else
                                Color.y = ClipAmount / _StartAmount;

                            if (_ColorAnimate.z == 0)
                                Color.z = _DissColor.z;
                            else
                                Color.z = ClipAmount / _StartAmount;

                            Albedo = (Albedo * ((Color.x + Color.y + Color.z)) * Color * ((Color.x + Color.y + Color.z))) / (1 - _Illuminate);

                            //o.Normal = t_normal;
                        }
                     }
                }
                if (Clip == 1)
                {
                    clip(-0.1);
                }
                /*o.Gloss = tex.a;
                o.Alpha = tex.a * _Color.a;
                o.Specular = _Shininess;*/
                return float4(Albedo, tex.a * _Color.a);
            }
            ENDCG
        }
    }
}
