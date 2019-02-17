Shader "BlueCube/WorldObject2" {
	Properties {
		_Color ("CanChangeColor", Color) = (1,1,1,1)
		_Color2 ("BlockColor", Color) = (1,1,1,1)
		_MainTex ("3D_Texture", 2D) = "white" {}
		_MainTex4 ("2D_Texture2", 2D) = "white" {}
		_MainTex5("BackgroundTexture", 2D) = "white" {}
		_MainTex2("Emission_Texture", 2D) = "white" {}
		_MainTex3("Spectrum", 2D) = "white" {}
		_Emission("Emission_Power", float) = 1
		_Emission2("Spectrum_Emission_Power", float) = 1
		_Speed("Emission_Speed", float) = 1
		_Speed2("Block_Emission_Speed", float) = 1
		_Choice("Choice", float) = 0
		[Space][Space][Space][Space]
		
		[Header(Outline_Setting)]
		[Toggle]_OutlineCheck("Outline_Check", float) = 0
		_OutlineColor("OutLine_Color", color) = (0,0,0,0)
		_OutlineSize("OutLine_Size", Range(0,0.9)) = 0
		_Stencil("Stencil ID", float) = 1
		[Space][Space][Space][Space]

		[Header(Specular_Setting)]
		[Toggle]_Specular("Specular_Check", float) = 0
		[Space]
		_SpecularTex("SpecularTexture", 2D) = "white" {}
		_SpecularSize("SpecularSize", float) = 1
		_SpecularVector("Specular_Dir", Vector) = (0.55,100,0.65,0)
		_SpecularRange("Specular_range", Range(0,10)) = 7
		_EmissionPower("Speculer_Emission_Power", float) = 2
	}

	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200
		
		Stencil {
        Ref [_Stencil]
        Pass Replace
      }
		
		CGPROGRAM
		#pragma surface surf Standard

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;
		sampler2D _MainTex4;
		sampler2D _MainTex5;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
			float2 uv_MainTex4;
			float2 uv_MainTex5;
			float3 worldPos;
			float3 worldNormal;
		};

		float4 _Color;
		float4 _Color2;
		float _Emission;
		float _Emission2;
		float _Speed;
		float _Speed2;
		float _Choice;

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex);
			float4 f = tex2D(_MainTex2, IN.uv_MainTex);
			float4 e = tex2D(_MainTex4, IN.uv_MainTex4);
			float4 d = tex2D(_MainTex3, float2(_Time.y * _Speed, 0.5));
			float4 g = tex2D(_MainTex3, float2(_Time.y * _Speed2, 0.5));
			float4 h = tex2D(_MainTex5, IN.uv_MainTex5);
						
			float2 topUV = float2(IN.worldPos.x, IN.worldPos.z);
			float2 frontUV = float2(IN.worldPos.x, IN.worldPos.y);
			float2 sideUV = float2(IN.worldPos.z, IN.worldPos.y);

			float4 topTex = tex2D(_MainTex2, topUV);
			float4 frontTex = tex2D(_MainTex2, frontUV);
			float4 sideTex = tex2D(_MainTex2, sideUV);
						
			if (_Choice == 0)
			{
				o.Albedo = c.rgb;
				o.Emission = f.rgb * _Emission;
			}
			else if (_Choice == 1)
			{
				o.Albedo = e.rgb;
			}
			else if (_Choice == 2)
			{
				o.Albedo = c.rgb;
				o.Emission = f.rgb * _Emission;
				o.Emission = (o.Emission + _Color) * _Emission2 * d.r;
			}
			else if (_Choice == 3)
			{
				o.Albedo = c.rgb;
				o.Emission = f.rgb * _Emission;
				o.Emission = (o.Emission + _Color2) * _Emission2 * g.r;
			}
			else if (_Choice == 4)
			{
				o.Albedo = h.rgb;
				o.Emission = f.rgb * _Emission;
			}
		}
		ENDCG

		
		//Outline_Pass

		//Stencil {
  //      Ref [_Stencil]
  //      Comp NotEqual
  //    }

		//cull front
		//		
		//CGPROGRAM
		//#pragma surface surf OLine vertex:vert noshadow noambient

		//#pragma target 3.0

		//float4 _OutlineColor;
		//float _OutlineSize;
		//float _OutlineCheck;
		//
		//void vert(inout appdata_full v)
		//{
		//	if (_OutlineCheck == 1)
		//		//v.vertex.xyz += v.vertex.xyz * _OutlineSize;
		//		v.vertex.xzy += v.normal * _OutlineSize;
		//}

		//struct Input {
		//	float3 color:COLOR;
		//};

		//void surf (Input IN, inout SurfaceOutput o) {
		//	o.Albedo = _OutlineColor.rgb;
		//	o.Alpha = _OutlineColor.a;
		//}
	
		//float4 LightingOLine(SurfaceOutput s, float3 lightDir, float atten)
		//{
		//	float4 final;
		//	if (_OutlineCheck == 0)
		//		final = 0;
		//	else
		//		final = float4(s.Albedo,s.Alpha);
		//	return final;
		//}
		//ENDCG

		//Specular_Pass
		
		/*Stencil {
		Ref[_Stencil]
        Pass Replace
      }

	  cull back

		CGPROGRAM
		#pragma surface surf SPC alpha:fade

		#pragma target 3.0

		sampler2D _SpecularTex;
		float _SpecularSize;
		float _EmissionPower;
		float4 _SpecularVector;
		float _SpecularRange;
		float _Check;
		float4 d;		
		float _Specular;

		struct Input {
			float3 worldPos;
		};


		void surf(Input IN, inout SurfaceOutput o)
		{
			float2 top_UV = float2(IN.worldPos.x, IN.worldPos.z);

			d = tex2D(_SpecularTex, top_UV * _SpecularSize);

			o.Emission = d.rgb + _EmissionPower;
			o.Alpha = d.r;
		}

		float4 LightingSPC(SurfaceOutput s, float3 lightDir, float3 viewDir, float3 atten)
		{
			float3 vnorl = saturate(dot(s.Normal, normalize(_SpecularVector.rgb + viewDir)));

			vnorl = pow(vnorl, pow(10, _SpecularRange));

			float4 final_return;
			final_return.rgb = s.Emission * vnorl * _LightColor0.rgb;
			final_return.a = s.Alpha;
			if (final_return.a != 0 && _Check == 0 && _Specular == 1)
				final_return.a = lerp(0, 1, vnorl.r);
			else
				final_return.a = 0;

			return final_return;
		}
		ENDCG*/
		

	}
	FallBack "Diffuse"
}
