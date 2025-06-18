// Made by Moonbae Chae   01.2016
// 세 장의 알파를 포함하지 않는 텍스쳐를 사용
// MainTex = RGB, Mask = Alpha, _DistortTex = Replacement
// Mask Texture는 inspector의 alpha channel에서 R,G,B를 선택하여 사용
// 각 Textures는 UV offset적용됨
// maya4d@gamevil.com 

Shader "Lean/VFX/FX_Distort_Mask_Blend" 
{
	Properties
	{
		_Multiplier ("Color Multiplier", Float) = 2.0
		_TintColor ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("MainTex", 2D) = "white" {}
		_USpeed ("U Speed", Float) = 0.0
		_VSpeed ("V Speed", Float) = 0.0
		
		_MaskTex ("Mask ", 2D) = "white" {}	
		_USpeed2 ("U Speed2", Float) = 0.0
		_VSpeed2 ("V Speed2", Float) = 0.0
		
		_DistortTex ("Distort (RGB)", 2D) = "white" {}
		_USpeed3 ("Distort U Speed", Float) = 0.0
		_VSpeed3 ("Distort V Speed", Float) = 0.0

		_DispPowX  ("DistortPow X", Float) = 0
		_DispPowY  ("DistortPow Y", Float) = 0.2
		
		[KeywordEnum(Red, Green, Blue)] _SelectChannel ("Alpha Channel", Float) = 0

	}
		
	SubShader
	{
		 Tags { "Queue"="AlphaTest+98" "RenderType"="Transperent" }
		
		Pass
		{
			Fog { Mode Off }
			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off
			//ColorMask RGB
			
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag	
			#pragma fragmentoption ARB_precision_hint_fastest	
			#include "UnityCG.cginc"

			
			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			sampler2D _DistortTex;
			fixed4 _DistortTex_ST;
			sampler2D _MaskTex;
			fixed4 _MaskTex_ST;
			fixed _Multiplier;
			fixed4 _TintColor;
			fixed _SelectChannel;
			
			fixed _DispPowX;
			fixed _DispPowY;
			
			fixed _USpeed;
			fixed _VSpeed;

			fixed _USpeed2;
			fixed _VSpeed2;

			fixed _USpeed3;
			fixed _VSpeed3;
			
			struct vertexInput
			{
				half4 vertex : POSITION;
				fixed4 color : COLOR0;
				half2 texcoord0 : TEXCOORD0;
			};

			struct fragmentInput
			{
				fixed4 position : SV_POSITION;
				half4 color : COLOR0;

				// Time에 의한 UV는 float를 쓴다.
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
			};
			

			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				o.position = UnityObjectToClipPos (i.vertex);		
				
				//
				float2 uv = TRANSFORM_TEX(i.texcoord0.xy, _MainTex);
				o.texcoord0.x = uv.x + (_USpeed * _Time.y);
				o.texcoord0.y = uv.y + (_VSpeed * _Time.y);

				//				
				float2 uv2 = TRANSFORM_TEX(i.texcoord0.xy, _MaskTex);
				o.texcoord1.x = uv2.x + (_USpeed2 * _Time.y);
				o.texcoord1.y = uv2.y + (_VSpeed2 * _Time.y);

				//
				float2 uv3 = TRANSFORM_TEX(i.texcoord0.xy, _DistortTex);
				o.texcoord2.x = uv3.x + (_USpeed3 * _Time.y);
				o.texcoord2.y = uv3.y + (_VSpeed3 * _Time.y);
				//
				o.color = i.color;

				//
				return o;
			}


			float4 frag(fragmentInput i) : COLOR
			{

				fixed4 dispColor = tex2D(_DistortTex, i.texcoord2);
				
				fixed2 uvoft =  i.texcoord2;				
				uvoft.x = (dispColor.r * _DispPowX) ;
				uvoft.y = (dispColor.g * _DispPowY) ;
								
				fixed3 mainTexColor = i.color.rgb * tex2D(_MainTex, i.texcoord0 + uvoft ).rgb;								
												
				fixed4 alphaTex = tex2D(_MaskTex, i.texcoord1 + uvoft);
				
				fixed4 ChannelR = fixed4(mainTexColor,(alphaTex.r * i.color.a) );
				fixed4 ChannelG = fixed4(mainTexColor,(alphaTex.g * i.color.a) );
				fixed4 ChannelB = fixed4(mainTexColor,(alphaTex.b * i.color.a) );
				
				
				if(_SelectChannel == 0)
						return ChannelR * _Multiplier * _TintColor;	//  R 채널 사용.
					else if(_SelectChannel == 1)
						return ChannelG * _Multiplier * _TintColor; // G 채널 사용.
					else 
						return ChannelB * _Multiplier * _TintColor;	// B 채널 사용.				

			}
			ENDCG
		}
	}
}