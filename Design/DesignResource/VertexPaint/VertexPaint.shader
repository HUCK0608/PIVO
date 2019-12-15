Shader "Custom/VertexPaint"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_ShadowColor ("DeepShadowColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf ModifyShadow vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float4 VColor : Color;
        };

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
		}

        float4 _Color;
		float4 _ShadowColor;
		float4 VertexColor;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			float4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			VertexColor = IN.VColor;

			o.Albedo = c.rgb * VertexColor.r;
            o.Alpha = c.a;
        }

		half4 LightingModifyShadow(SurfaceOutput s, half3 lightDir, half atten)
		{
			half4 Final;

			half NdotL = max(0, dot(s.Normal, lightDir));			
			half ShadowRange = NdotL * atten;
			half3 ShadowColor = (1 - VertexColor.r) * _ShadowColor;
			
			Final.rgb = (s.Albedo * _LightColor0 * ShadowRange) + ShadowColor;
			Final.a = s.Alpha;

			return Final;
		}
        ENDCG
    }
    FallBack "Diffuse"
}
