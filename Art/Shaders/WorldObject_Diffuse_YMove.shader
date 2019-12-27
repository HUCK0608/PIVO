Shader "BlueCube/WorldObject_Diffuse_YMove"
{
	//worldobject_diffuse와 쉐이딩방식은 동일
	Properties{
		_MainTex("3D_Texture", 2D) = "white" {}
		_MainTex4("2D_Texture2", 2D) = "white" {}
		_MainTex2("Emission_Texture", 2D) = "white" {}
		_Emission("Emission_Power", float) = 1
		[Toggle]_IsUse2DTexture("IsUse2DTexture", float) = 0
		_Amount("위아래 움직이는 정도", Range(0,1)) = 0.5
	}

		SubShader{
			Tags { "RenderType" = "Opaque"}
			LOD 200

			Stencil {
			Ref[_Stencil]
			Pass Replace
		  }

			CGPROGRAM
			#pragma surface surf Lambert vertex:vert addshadow
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _MainTex2;
			sampler2D _MainTex4;


			struct Input {
				float2 uv_MainTex;
				float2 uv_MainTex2;
				float2 uv_MainTex4;

			};

			float _Emission;
			float _IsUse2DTexture;
			float _Amount;

			void vert(inout appdata_full v)
			{
				v.vertex.y += sin(abs(_Time.y))*_Amount;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				float4 c = tex2D(_MainTex, IN.uv_MainTex);
				float4 f = tex2D(_MainTex2, IN.uv_MainTex);
				float4 e = tex2D(_MainTex4, IN.uv_MainTex4);

				o.Albedo = lerp(c.rgb, e.rgb, _IsUse2DTexture);
				o.Emission = lerp(f.rgb, 0, _IsUse2DTexture) * _Emission;

			}
			ENDCG

		}
    FallBack "Diffuse"
}
