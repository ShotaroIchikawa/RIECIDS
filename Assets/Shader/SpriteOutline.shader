// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Custom/SpriteOutline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Offset("Offset", Range(-0.4, 0.4)) = 0.2
		_OutLineColor("Outline Color", Color) = (1, 1, 1, 1)
		_Alpha("Alpha", Range(0.0, 1.0)) = 1
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
		Fog{ Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile DUMMY PIXELSNAP_ON
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex	: SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed4 _OutLineColor;
	half _Alpha;
	half _Offset;

	v2f vert(appdata IN)
	{
		fixed scale = 1 + _Offset;

		float2 tex = IN.texcoord * scale;
		tex -= (scale - 1) / 2;
		
		v2f OUT;
		
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = tex;
		OUT.color = IN.color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
		return OUT;
	}
//
	sampler2D _AlphaTex;
	float _AlphaSplitEnabled;

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
		const fixed THRESHOLD = 0.1;
	float _off = _Offset / 20;
	fixed4 base = SampleSpriteTexture(IN.texcoord) * IN.color;
	if (IN.texcoord.x > 1 - _off || IN.texcoord.x < _off || IN.texcoord.y > 1 - _off || IN.texcoord.y < _off)
	{
		base = _OutLineColor;
	}


	fixed4 main_col = base;
	main_col.a = _Alpha * max(0, sign(main_col.a - THRESHOLD));

	return main_col;
	}
		ENDCG
	}
	}
}