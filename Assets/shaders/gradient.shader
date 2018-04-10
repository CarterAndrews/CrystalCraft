// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BasicGradient"
{
	Properties
	{
		_TopColor("Top Color", Color) = (1, 1, 1, 0.7)
		_BottomColor("Bottom Color", Color) = (1, 1, 1, 0.7)
		_RampTex("Ramp Texture", 2D) = "white" {}
	}

		SubShader
	{
		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		struct vertexIn {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(vertexIn input)
	{
		v2f output;

		output.pos = UnityObjectToClipPos(input.pos);
		output.uv = input.uv;

		return output;
	}

	fixed4 _TopColor, _BottomColor;
	sampler2D _RampTex;

	fixed4 frag(v2f input) : COLOR
	{
		return lerp(_BottomColor, _TopColor, tex2D(_RampTex, input.uv).a);
	}
		ENDCG
	}
	}
}