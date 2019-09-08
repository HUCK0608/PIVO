Shader "BlueCube/HintStone" {
	Properties {

	    _MainTex ("파란색", 2D) = "white" {}
		_MainTex2 ("굴절", 2D) = "white" {}
		_MainTex3 ("하얀띠", 2D) = "white" {}

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {
		float4 Disto = tex2D (_MainTex2, IN.uv_MainTex2);
		float DI = (Disto.r+Disto.g+Disto.b)/3;
	
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 WH = tex2D (_MainTex3, float2(IN.uv_MainTex.x+DI, IN.uv_MainTex.y+DI-_Time.y*0.2));

			o.Emission = lerp(c.rgb, c.rgb+WH*0.7, WH.a);
	
		}
		ENDCG
	}
	FallBack "Diffuse"
}
