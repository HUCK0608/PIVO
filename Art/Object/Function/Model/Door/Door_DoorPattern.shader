Shader "BlueCube/Door/DoorPattern" {
	Properties {
		_MainTex2 ("Albedo (RGB)", 2D) = "white" {}
		_PatternFill("PatternFill", Range(1,6.8))=1
		_Color("Color",Color) = (0.046,0.210,0.433,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex2;

		struct Input {
			float2 uv_MainTex2;
		};

		float _PatternFill;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 cc = tex2D (_MainTex2, float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y*_PatternFill));
			o.Albedo = _Color.rgb;
			o.Emission = lerp(0, cc.rgb*1.8, cc.a);
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
