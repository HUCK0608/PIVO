Shader "BlueCube/VertexPaint"
{
    Properties
    {
        _MainTex3D("3D_Texture", 2D) = "white" {}
		_MainTex2D("2D_Texture", 2D) = "white" {}

		_ShadowColor ("DeepShadowColor", Color) = (1,1,1,1)
		_ShadowColor2("DeepShadowColor2", Color) = (1,1,1,1)
		[Toggle]_IsUse2DTexture("IsUse2DTexture", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		Stencil 
		{
			Ref[_Stencil]
			Pass Replace
		}

        CGPROGRAM
        //#pragma surface surf Lambert vertex:vert
		#pragma surface surf _ModifyShadow vertex:vert
        #pragma target 3.0

        sampler2D _MainTex3D;
		sampler2D _MainTex2D;

        struct Input
        {
            float2 uv_MainTex3D;
			float2 uv_MainTex2D;
			float4 VColor : Color;
        };

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

		float4 _ShadowColor;
		float4 _ShadowColor2;

		float4 VertexColor;
		float3 MaskG;
		float3 MaskB;

		float _IsUse2DTexture;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			float4 Tex3 = tex2D(_MainTex3D, IN.uv_MainTex3D);
			float4 Tex2 = tex2D(_MainTex2D, IN.uv_MainTex2D);
			VertexColor = IN.VColor;
			
			MaskG = Tex3.rgb * (1 - VertexColor.b);
			MaskB = Tex3.rgb * (1 - VertexColor.g);

			o.Albedo = lerp(Tex3.rgb - MaskG - MaskB, Tex2.rgb, _IsUse2DTexture);
            o.Alpha = lerp(Tex3.a, Tex2.a, _IsUse2DTexture);
        }

		float4 Lighting_ModifyShadow(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{
			float4 Final;

			float NdotL = saturate(dot(s.Normal, lightDir));
			//half NdotL = max(0, dot(s.Normal, lightDir));
			float ShadowRange = NdotL * atten;
			float3 ShadowColor = MaskB * _ShadowColor;
			float3 ShadowColor2 = MaskG * _ShadowColor2;
			
			Final.rgb = lerp(
				(s.Albedo * _LightColor0 * ShadowRange) + ShadowColor + ShadowColor2,
				s.Albedo * NdotL * _LightColor0 * atten,
				_IsUse2DTexture
			);

			Final.a = s.Alpha;

			return Final;
		}
        ENDCG
    }
    FallBack "Mobile/VertexLit"
}
