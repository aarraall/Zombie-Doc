// Toony Colors Pro+Mobile Shaders
// (c) 2013,2014 Jean Moreno

Shader "Toony Colors Pro/Normal/OneDirLight/Basic-Transparent"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}

	//COLORS
	_Color("Highlight Color", Color) = (0.8,0.8,0.8,1)
		_SColor("Shadow Color", Color) = (0.0,0.0,0.0,1)

		[PerRendererData]_AlphaValue("Alpha", Range(0,1)) = 1
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 200

		Pass{
		ZWrite On
		ColorMask 0
	}

		CGPROGRAM

#include "TGP_Include.cginc"

		//nolightmap nodirlightmap		LIGHTMAP
		//noforwardadd					ONLY 1 DIR LIGHT (OTHER LIGHTS AS VERTEX-LIT)
#pragma surface surf ToonyColors nolightmap nodirlightmap noforwardadd alpha:fade

		sampler2D _MainTex;
	float _AlphaValue;

	struct Input
	{
		half2 uv_MainTex : TEXCOORD0;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex);

		o.Albedo = c.rgb;

		o.Alpha = c.a * _AlphaValue;
	}

	ENDCG
	}

		Fallback "Transparent/VertexLit"
}
