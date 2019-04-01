Shader "Custom/StageSelect_floor"
{
    Properties
    {
        _Color0 ("Color", Color) = (1,1,1,1)
		_Color1("Color", Color) = (1,1,1,1)
		_Color2("Color", Color) = (1,1,1,1)

        _MainTex0 ("Albedo (RGB)", 2D) = "white" {}
		_MainTex1 ("Albedo (RGB)", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex0;
		sampler2D _MainTex1;

        struct Input
        {
            float2 uv_MainTex0;
			float2 uv_MainTex1;

        };

        float4 _Color0;
		float4 _Color1;
		float4 _Color2;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 c = tex2D(_MainTex0, IN.uv_MainTex0);

			if (c.r == 0)
			{
				c.rgb = _Color0.rgb;
			};

           
			o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
