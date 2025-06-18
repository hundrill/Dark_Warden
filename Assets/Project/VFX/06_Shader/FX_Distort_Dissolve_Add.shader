Shader "Lean/VFX/FX_Distort_Dissolve_Add" 
{
	Properties
	{
		_Multiplier ("Color Multiplier", Float) = 2.0
		_TintColor ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("MainTex", 2D) = "white" {}
		_USpeed ("U Speed", Float) = 0.0
		_VSpeed ("V Speed", Float) = 0.0
		[Toggle] _TexAlpha ("Use Mask MainTexture Alpha", Float) = 0
		
		[KeywordEnum(Red, Green, Blue, Alpha)] _SelectChannel ("Alpha Channel", Float) = 0
		[KeywordEnum(None,Red, Green, Blue, Alpha)] _DoubleChannel ("Double Alpha Channel(No Distort)", Float) = 0
		_MaskTex ("Mask ", 2D) = "white" {}	
		_USpeed2 ("U Speed2", Float) = 0.0
		_VSpeed2 ("V Speed2", Float) = 0.0
		
		_DistortTex ("Distort (RGB)", 2D) = "white" {}
		_USpeed3 ("Distort U Speed", Float) = 0.0
		_VSpeed3 ("Distort V Speed", Float) = 0.0

		_DispPowX  ("DistortPow X", Float) = 0
		_DispPowY  ("DistortPow Y", Float) = 0.2

		[Toggle] _usedistort ("Use Distort DissolveTexture", Float) = 0
		_SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
        _SliceAmount ("Slice Amount", Range(0.0, 2.0)) = 0.0
		
	}
		
	SubShader
	{
		Tags {"Queue" = "Transparent" }
		
		Pass
		{
			Fog { Mode Off }
			Lighting Off
			Blend SrcAlpha One
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
			fixed _DoubleChannel;
			
			fixed _DispPowX;
			fixed _DispPowY;
			
			fixed _USpeed;
			fixed _VSpeed;

			fixed _USpeed2;
			fixed _VSpeed2;

			fixed _USpeed3;
			fixed _VSpeed3;

			sampler2D _SliceGuide;
			fixed4 _SliceGuide_ST;
			fixed _SliceAmount;
			int _usedistort;

			int _TexAlpha;
			fixed mtcalpha;

			fixed4 ChannelRGB;

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
				fixed2 texcoord0 : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
				fixed2 texcoord2 : TEXCOORD2;
				fixed2 texcoord3 : TEXCOORD3;
			};
			

			fragmentInput vert(vertexInput i)
			{
				fragmentInput o;
				o.position = UnityObjectToClipPos (i.vertex);		
				
				//
				half2 uv = TRANSFORM_TEX(i.texcoord0.xy, _MainTex);	
				uv.x = uv.x + (_USpeed * _Time.y);
				uv.y = uv.y + (_VSpeed * _Time.y);																		
				o.texcoord0 = uv;

				//				
				half2 uv2 = TRANSFORM_TEX(i.texcoord0.xy, _MaskTex);

				uv2.x = uv2.x + (_USpeed2 * _Time.y);
				uv2.y = uv2.y + (_VSpeed2 * _Time.y);								

				o.texcoord1 = uv2;		

				//
				half2 uv3 = TRANSFORM_TEX(i.texcoord0.xy, _DistortTex);
				uv3.x = uv3.x + (_USpeed3 * _Time.y);
				uv3.y = uv3.y + (_VSpeed3 * _Time.y);	
				o.texcoord2 = uv3;
				//
				o.color = i.color;

				//
				// used by Dissolve Texture;
				half2 uv4 = TRANSFORM_TEX(i.texcoord0.xy, _SliceGuide);
				o.texcoord3 = uv4;

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
				fixed4 dalphaTex = tex2D(_MaskTex, i.texcoord1);

				if(_TexAlpha == 0) mtcalpha = 1.0;
				   else mtcalpha = tex2D(_MainTex, i.texcoord0 + uvoft ).a;

				if(_SelectChannel == 0)
				       ChannelRGB = fixed4(mainTexColor,(alphaTex.r * i.color.a * mtcalpha) );
				  else if(_SelectChannel == 1)
				       ChannelRGB = fixed4(mainTexColor,(alphaTex.g * i.color.a * mtcalpha) );
				  else if(_SelectChannel == 2)
				       ChannelRGB = fixed4(mainTexColor,(alphaTex.b * i.color.a * mtcalpha) );
				  else if(_SelectChannel == 3)
				       ChannelRGB = fixed4(mainTexColor,(alphaTex.a * i.color.a * mtcalpha) );

				if (_DoubleChannel == 1)
				      ChannelRGB *= dalphaTex.r;
				   else if(_DoubleChannel == 2)
				      ChannelRGB *= dalphaTex.g;
				   else if(_DoubleChannel == 3)
				      ChannelRGB *= dalphaTex.b;
				   else if(_DoubleChannel == 4)
				      ChannelRGB *= dalphaTex.a;

				if(_usedistort == 0)
				      clip(tex2D (_SliceGuide, i.texcoord3).rgb - _SliceAmount);
				   else 
				      clip(tex2D (_SliceGuide, i.texcoord3 + uvoft).rgb - _SliceAmount);
				
				 return ChannelRGB * _Multiplier * _TintColor;		
			}
			ENDCG
		}
	}
}