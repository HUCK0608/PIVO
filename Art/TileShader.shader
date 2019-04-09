Shader "BlueCube/TileShader"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
		_Color2("Color2", Color) = (1,1,1,1)
		_Color3("Color3", Color) = (1,1,1,1)
		_Color4("Color4", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        float4 _Color1;
		float4 _Color2;
		float4 _Color3;
		float4 _Color4;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            float4 c = tex2D (_MainTex, IN.uv_MainTex);

			if (c.r == 1)
				c = _Color1;
			else if (c.g == 1)
				c = _Color2;
			else if (c.b == 1)
				c = _Color3;
			else
				c = _Color4;

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
