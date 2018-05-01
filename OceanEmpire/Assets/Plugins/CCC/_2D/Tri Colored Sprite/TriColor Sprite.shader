// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Ocean Empire/TriColor Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _ColorR("Color R", Color) = (1,1,1,1)
		[PerRendererData] _ColorG("Color G", Color) = (1,1,1,1)
		[PerRendererData] _ColorB("Color B", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile _ PIXELSNAP_ON
		#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					float2 texcoord  : TEXCOORD0;
				};

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
			#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
			#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;
				fixed4 _ColorR;
				fixed4 _ColorG;
				fixed4 _ColorB;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);
					
			#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
			#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 tex = SampleSpriteTexture(IN.texcoord);
					//fixed4 c = (tex.r * _ColorR) + (tex.g * _ColorG) + (tex.b * _ColorB);
					fixed4 c = (tex.r * _ColorR * _ColorR.a) + (tex.g * _ColorG * _ColorG.a) + (tex.b * _ColorB * _ColorB.a);
					//fixed4 c = pc + half4(0,0,0,1);
					c.a = tex.a * max(max(_ColorR.a, _ColorG.a), _ColorB.a);
					c.rgb *= c.a;
					return c;
				}
				ENDCG
			}
		}
}