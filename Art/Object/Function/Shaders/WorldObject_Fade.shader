Shader "BlueCube/WorldObject_Fade" {
	Properties {
		_AlbedoColor("Albedo Color", Color) = (1, 1, 1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_EmissionTex("Emission Texture", 2D) = "white" {}
		_Color("Emission Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade

		sampler2D _MainTex;
		float4 _AlbedoColor;
		sampler2D _EmissionTex;
		float4 _Color;

		struct Input {
			float2 uv_MainTex;
			float2 uv_EmissionTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float4 c = tex2D (_MainTex, IN.uv_MainTex) * _AlbedoColor;
			float4 e;

			if (_Color.g == 1)
				e = tex2D(_EmissionTex, IN.uv_EmissionTex);
			else
				e.rgb = float3(1, 0, 0);

			o.Emission = e.rgb;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
