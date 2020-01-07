Shader "BlueCube/DistortionGlass" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Distortion_Texture", 2D) = "white" {}
		_MainTex2("White_Texture", 2D) = "white" {}
		_DistortionPower("Distortion_Power", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		zwrite off

		GrabPass{}

		CGPROGRAM
		#pragma surface surf dis alpha:fade

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _GrabTexture;

		struct Input {
			float4 screenPos;
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		float4 _Color;
		float _DistortionPower;
		float _Alpha;

		void surf (Input IN, inout SurfaceOutput o) {
			float4 d = tex2D(_MainTex, IN.uv_MainTex);
			float4 w = tex2D(_MainTex2, IN.uv_MainTex2);
			float3 UV = IN.screenPos.rgb / IN.screenPos.a;
			float4 e = tex2D(_GrabTexture, UV.xy + (d.b * _DistortionPower) - (1-d.b * _DistortionPower) * 0.006);

			o.Emission = lerp(e.rgb, w.rgb * _Color - 0.7, d.b);
			//o.Emission = e.rgb * _Color;

			//_Alpha = lerp(0.1, 0, d.b);
			_Alpha = 1*e;
		}

		float4 Lightingdis(SurfaceOutput s, float3 lightDir, float3 atten)
		{
			return float4(0,0,0,_Alpha);
		}
		ENDCG
	}
	FallBack "Lagacy Shader/Transparent/Vertexlit"
}
