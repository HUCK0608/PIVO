Shader "Custom/HideCorgiPlane"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		GrabPass{}

		CGPROGRAM
		#pragma surface surf Lambert noshadow

		#pragma target 3.0

		sampler2D _GrabTexture;
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float4 screenPos;
        };

        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float2 screenUV = IN.screenPos.rgb / IN.screenPos.a;
			o.Emission = tex2D(_GrabTexture, screenUV).rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "LegacyShader/VertexLit"
}
