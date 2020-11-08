Shader "Custom/GameTile"
{
	Properties
	{
		_UnderlayColor("Underlay Color", Color) = (1,1,1,1)
		_OverlayColor("Overlay Color", Color) = (1,1,1,1)
		_OverlayTex("Overlay Texture", 2D) = "white" {}
		_OverlayTexInfo("X Min, X Max, Y Min, Y Max", Vector) = (0, 1, 0, 1)
		_OverlayTexEnabled("Overlay Texture Enabled", Float) = 0.0
		_OverlayMask("Tile Mask", 2D) = "white" {}
		_OverlayMaskInfo("X Min, X Max, Y Min, Y Max", Vector) = (0, 1, 0, 1)
		_OverlayMaskColor("Tile Mask Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
		}

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#pragma target 2.0

			#include "UnityCG.cginc"

			fixed4 _UnderlayColor;
			fixed4 _OverlayColor;
			sampler2D _OverlayTex;
			fixed4 _OverlayTexInfo;
			fixed _OverlayTexEnabled;
			sampler2D _OverlayMask;
			fixed4 _OverlayMaskInfo;
			fixed4 _OverlayMaskColor;
			float4 _OverlayTex_ST;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 texUv : TEXCOORD1;
				float2 maskUv : TEXCOORD2;
				UNITY_FOG_COORDS(3)
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _OverlayTex);

				//handle tex uv mapping
				float4 texAtlasInfo = UNITY_ACCESS_INSTANCED_PROP(_OverlayTexInfo_arr, _OverlayTexInfo);
				float2 texUv = v.texcoord;
				texUv.x = lerp(texAtlasInfo.x, texAtlasInfo.y, texUv.x);
				texUv.y = lerp(texAtlasInfo.z, texAtlasInfo.w, texUv.y);
				o.texUv = texUv;

				//handle mask uv mapping
				float4 maskAtlasInfo = UNITY_ACCESS_INSTANCED_PROP(_OverlayMaskInfo_arr, _OverlayMaskInfo);
				float2 maskUv = v.texcoord;
				maskUv.x = lerp(maskAtlasInfo.x, maskAtlasInfo.y, maskUv.x);
				maskUv.y = lerp(maskAtlasInfo.z, maskAtlasInfo.w, maskUv.y);
				o.maskUv = maskUv;

				//handle fog
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				//base color
				float4 isMasked = tex2D(_OverlayMask, i.maskUv) == _OverlayMaskColor;
				float4 pixelColor = isMasked ? (UNITY_ACCESS_INSTANCED_PROP(_OverlayTexEnabled_arr, _OverlayTexEnabled) ? tex2D(_OverlayTex, i.texUv)
					                         : UNITY_ACCESS_INSTANCED_PROP(_OverlayColor_arr, _OverlayColor)) : UNITY_ACCESS_INSTANCED_PROP(_UnderlayColor_arr, _UnderlayColor);

				//apply fog
				UNITY_APPLY_FOG(i.fogCoord, pixelColor);

				return pixelColor;
			}
			ENDCG
		}
	}
	Fallback Off
}