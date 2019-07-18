Shader "Custom/Effect_Shader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Tex1", 2D) = "white" {}
		_MainTex2("Tex2", 2D) = "white" {}
		_DT("distortion", 2D) = "white" {}
		_Alpha("Alpha", range(0,1)) = 1
		_EffectRange("Effect_Range", Range(0,10)) = 0
		_EffectRange2("Effect_Range2", Range(0,10)) = 0
	}



	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		//Cull off
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade keepalpha

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DT;
		sampler2D _Normal;
		sampler2D _CameraDepthTexture;
		float4 _Normal_ST;
		float4 _Color;
		float _Alpha;
		float _EffectRange;

		struct Input {
			float2 uv_texcoord;
			float2 uv_MainTex;
			float3 worldPos;
			float2 uv_DT;
			float4 screenPos;
		};		

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 side_UV = float2(IN.worldPos.x*0.5, IN.worldPos.y*0.5);
			//텍스쳐 사이즈 수정할때 곱하기 숫자 붙이기
			float4 c = tex2D(_MainTex, float2(side_UV.x, side_UV.y - _Time.y*0.25)) * _Color;
			
			//float4 c = tex2D (_MainTex, float2(IN.uv_MainTex.x,IN.uv_MainTex.y+_Time.y)) * _Color;


			float2 uv_Normal = IN.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = Normal;

			float4 ch_screenPos = float4(IN.screenPos.rgb,IN.screenPos.a);
			float screenDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(IN.screenPos))));
			float distanceDepth = abs( ( screenDepth - LinearEyeDepth( ch_screenPos.b/ ch_screenPos.a ) ) / _EffectRange );
			
			float4 final;			
			final = (lerp(float4(c.rgb, _Alpha), float4(c.rgb, 0), clamp(distanceDepth, 0.0, 1.0)));
			final = lerp(final, float4(0, 0, 0, 0), c.r);


		
			o.Emission = final+final+final;
			o.Alpha = final.a;
		}
		ENDCG

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade keepalpha

		#pragma target 3.0

		sampler2D _MainTex2;
		sampler2D _Normal;
		sampler2D _CameraDepthTexture;
		sampler2D _DT;
		float4 _Normal_ST;
		float4 _Color;
		float _Alpha;
		float _EffectRange2;

		struct Input {
			float2 uv_texcoord;
			float2 uv_MainTex2;
			float4 screenPos;
			float2 uv_DT;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o) {
			
			float2 side_UV = float2(IN.worldPos.x, IN.worldPos.y);
			float4 c = tex2D(_MainTex2, float2(side_UV.x, side_UV.y - _Time.y*0.25)) * _Color;
			//float4 c = tex2D(_MainTex2, float2(IN.uv_MainTex2.x, IN.uv_MainTex2.y+_Time.y*0.25)) * _Color;

			float2 uv_Normal = IN.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 Normal = UnpackNormal(tex2D(_Normal, uv_Normal));
			o.Normal = Normal;

			float4 ch_screenPos = float4(IN.screenPos.rgb, IN.screenPos.a);
			float screenDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos))));
			float distanceDepth = abs((screenDepth - LinearEyeDepth(ch_screenPos.b / ch_screenPos.a)) / _EffectRange2);

			float4 final;
			final = (lerp(float4(c.rgb, _Alpha), float4(c.rgb, 0), clamp(distanceDepth, 0.0, 1.0)));
			final = lerp(final, float4(0, 0, 0, 0), c.r);

			o.Emission = final + final+final + final;
			o.Alpha = final.a;
		}
		ENDCG



			/*CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha:fade keepalpha

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _MainTex2;
			sampler2D _Normal;
			sampler2D _CameraDepthTexture;
			sampler2D _DT;
			float4 _Normal_ST;
			float4 _Color;
			float _Alpha;
			float _EffectRange2;

			struct Input {
			float2 uv_texcoord;
			float2 uv_MainTex2;
			float2 uv_MainTex;
			float4 screenPos;
			float2 uv_DT;
		};

			void surf(Input IN, inout SurfaceOutputStandard o) {

				float4 c= tex2D(_MainTex2, IN.uv_MainTex2);
				float4 d = tex2D(_MainTex, float2(IN.uv_MainTex.x, IN.uv_MainTex.y - _Time.y + d.r));
				

				float _DT = lerp(0.5, 0, IN.uv_MainTex.y);

				float t;

				if (c.a*_DT <0.2)
				{
					t = 0;
				}

				else if (c.a*_DT < 0.2)
				{
					_Color = _Color + 1.5;
				}

				else
				{
					t = 1;
				}

				o.Emission = _Color;
				o.Alpha = t * _DT;
			}
		ENDCG*/
	
	}
	FallBack "Legacy Shader/Diffuse/Transparent/Vertexlit"
}
