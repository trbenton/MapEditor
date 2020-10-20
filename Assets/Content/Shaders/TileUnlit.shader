Shader "Custom/TileUnlit"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
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

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				//handle fog
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				//base color
				float4 pixelColor = tex2D(_MainTex, i.uv) * _Color;

				//apply fog
				UNITY_APPLY_FOG(i.fogCoord, pixelColor);

				return pixelColor;
			}
			ENDCG
		}
	}
	Fallback Off
}