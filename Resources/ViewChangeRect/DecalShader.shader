Shader "PIVO/Decal" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("MainTexture", 2D) = "white" {}
		_Spectrum("SpectrumTexture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200

		Lighting Off
		ZWrite Off
		Blend DstColor One

		CGPROGRAM
		#pragma surface surf Unlit
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Spectrum;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			float4 MainTex = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float4 Spectrum = tex2D(_Spectrum, float2(_Time.y * 0.7, 0.5));
			o.Albedo = MainTex.rgb * Spectrum;
			//o.Emission = MainTex.rgb * Spectrum;
			o.Alpha = 1;
		}

		float4 LightingUnlit(SurfaceOutput s, float3 lightDir, float atten)
		{
			float4 final;
			final.rgb = s.Albedo;
			final.a = s.Alpha;

			return final;
		}
		ENDCG
	}
	FallBack "Legacy Shader/Diffuse/Transparent/Vertexlit"
}
