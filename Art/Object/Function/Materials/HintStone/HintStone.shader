Shader "BlueCube/HintStone" {
	Properties {

	    _MainTex ("파란색", 2D) = "white" {}
		_MainTex2 ("굴절", 2D) = "white" {}
		_MainTex3 ("하얀띠", 2D) = "white" {}
		_Monotone("흑백/컬러 조절", Range(0,1)) = 0
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

		float _Monotone;

		void surf (Input IN, inout SurfaceOutputStandard o) {
		float4 Disto = tex2D (_MainTex2, IN.uv_MainTex2);
		float DI = (Disto.r+Disto.g+Disto.b)/3;
		

			float4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 WH = tex2D (_MainTex3, float2(IN.uv_MainTex.x+DI, IN.uv_MainTex.y+DI-_Time.y*0.2));

			//0일때 흑백, 1일때 파랑
			o.Emission = lerp((lerp(c.rgb, c.rgb+WH*0.7, WH.a).r + lerp(c.rgb, c.rgb + WH * 0.7, WH.a).g + lerp(c.rgb, c.rgb + WH * 0.7, WH.a).b ) / 12 , lerp(c.rgb, c.rgb + WH * 0.7, WH.a), _Monotone);
	
		}
		ENDCG
	}
	FallBack "Diffuse"
}
