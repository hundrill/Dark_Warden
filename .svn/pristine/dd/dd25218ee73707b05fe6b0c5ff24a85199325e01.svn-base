Shader "Unlit/DropNormalOutput"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Name "StandardLit"
			HLSLPROGRAM

			#pragma prefer_hlslcc gles3
			#pragma exclude_renderers d3d11_9x
			#pragma target 4.5
			#pragma vertex vert
			#pragma fragment frag
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct VertexInput
			{
				float4 vertex   : POSITION;
				float3 normal	: NORMAL;
			};

			struct VertexOutput
			{
				float4 position : SV_POSITION;
				float3 viewNormal : TEXCOORD0;
				float weight : TEXCOORD1;
			};

			VertexOutput vert(VertexInput i)
			{
				VertexOutput o;
				float3 posWS = TransformObjectToWorld(i.vertex.xyz).xyz;
				o.position = mul(unity_MatrixVP, float4(posWS, 1));
				o.viewNormal = mul(GetWorldToViewMatrix(), i.normal);
				o.weight = (i.vertex.y + 0.33) / (0.305 + 0.33);
				return o;
			}

			half4 frag(VertexOutput i) : SV_Target0
			{
				float3 normVS = normalize(i.viewNormal);
				normVS.xy = normVS.xy * 0.5 + 0.5;
				normVS.z = saturate(pow(1 - i.weight, 3.0));
				normVS.z = sqrt(normVS.z);
				return half4(normVS, 1);
			}

			ENDHLSL
        }
    }
}
