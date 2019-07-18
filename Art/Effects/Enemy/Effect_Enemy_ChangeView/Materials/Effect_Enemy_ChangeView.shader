Shader "BlueCube/Effect_Enemy_ChangeView" {
	Properties {
		
		_MainTex ("noise", 2D) = "white" {}

		_MainTex2("distortion", 2D) = "white" {}

		_colorTest("color",color) = (1,1,1,1)

		_scale("Main range",float) = 0


	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"= "Transparent" }
		LOD 200

		CGPROGRAM
	
		#pragma surface surf Standard alpha:fade

		

		sampler2D _MainTex;
		sampler2D _MainTex2;
		float4 _colorTest;
		float _scale;
	



		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			};

	


		void surf (Input IN, inout SurfaceOutputStandard o) {
			
		
			float4 d = tex2D(_MainTex2, IN.uv_MainTex2);
			//디스토션 텍스쳐 
		
			float4 c = tex2D(_MainTex, float2(IN.uv_MainTex.x,IN.uv_MainTex.y -_Time.y*0.25+d.r));
			//노이즈 텍스쳐 

			//distortion 값을 더 해줘서 일그러지게 만드는 준비문장...?

			//float4 f = tex2D(_MainTex3,float2(IN.uv_MainTex2.x,IN.uv_MainTex2.y-_Time.y*0.25*d.r));

			float _grd = lerp(1.8,0,(IN.uv_MainTex.y*_scale));
				//선형보간....그라디언트...색상이 자연스럽게 나타나게...?.. 그리고 y축 범위조절..
			
			
			float t ;

			if (c.a*_grd < 0.2)
			//if 만약 ~한다면 ~하게 만들어라?
			{
				t = 0; 
			}

			else if (c.a*_grd < 0.25)
			//if문 조건에 맞지 않았을 경우의 결과?
			{
				_colorTest = _colorTest;
			}

			else
			// 모든 조건이 아닐 때 이거 적용
			{
				t = 1;
			}
			
			
			o.Emission = _colorTest * d.rgb;
			
		
			o.Alpha =t;
		}
		ENDCG

	
	}
	FallBack "Diffuse"
}
