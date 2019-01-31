Shader "PIVO/ToonShader" {
	Properties {		
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ShadowCtrl("Shadow_Control", Range(0,1)) = 0.5
		_Ramp("RampColor", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0,1)) = 0.7
		_ShadowColor("Shadow_Color", Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf Toon noshadow

		#pragma target 3.0

		sampler2D _MainTex;

		float _ShadowCtrl;
		float4 _Ramp;
		float _Brightness;
		float _ShadowColor;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		float4 LightingToon(SurfaceOutput s, float3 lightDir, float3 viewDir, float3 atten)
		{
			float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;			
			float4 final_ndotl = 0;
			
			if (ndotl < _ShadowCtrl)
				ndotl = _ShadowColor;
			else
				ndotl = 1;

			if (ndotl < 1)
			{
				final_ndotl.rgb = ndotl * _Ramp.rgb * s.Albedo*_LightColor0.rgb *atten;
			}
			else
				final_ndotl.rgb = _Brightness * s.Albedo *atten;

			return final_ndotl;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
