Shader "Custom/world_speculer" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SpeculerVector("Speculer_Dir", Vector) = (0,0,0,0)
		_xCtrl("X_Ctrl", float) = 0
		_yCtrl("Y_Ctrl", float) = 0
		_SpeculerRange("Speculer_range", Range(0,10)) = 7
		_EmissionPower("Emission", float) = 1.8
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		LOD 200

		CGPROGRAM
		#pragma surface surf ws alpha:fade

		#pragma target 3.0

		sampler2D _MainTex;

		float4 _Color;
		float4 _SpeculerVector;
		float _xCtrl;
		float _yCtrl;
		float _SpeculerRange;
		float _EmissionPower;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			//float2 top_UV = float2(IN.worldPos.x, IN.worldPos.z) * 0.15f;

			//float4 c = tex2D(_MainTex, float2(top_UV.x + _xCtrl, top_UV.y + _yCtrl));
			float4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Emission = c.rgb + _EmissionPower;
			o.Alpha = lerp(0,1,c.r);
			//o.Alpha = 1;
		}

		float4 Lightingws(SurfaceOutput s, float3 lightDir, float3 viewDir, float3 atten)
		{
			float3 vnorl = saturate(dot(s.Normal, normalize(_SpeculerVector.rgb + viewDir)));
			//float3 vnorl = saturate(dot(s.Normal,normalize(lightDir.rgb + viewDir)));
						
			//vnorl = pow(vnorl, 10000000);
			vnorl = pow(vnorl, pow(10, _SpeculerRange));

			float4 final_return;

			final_return.a = s.Alpha;
			if (final_return.a != 0)
			{
				final_return.a = lerp(0, 1, vnorl.r);
			}
			else
				final_return.a = 0;
			
			final_return.rgb = s.Emission * vnorl * _LightColor0.rgb;

			return final_return;			
		}
		ENDCG
	}
	FallBack "Legacy Shader / Transparent / VertexLit"
}
