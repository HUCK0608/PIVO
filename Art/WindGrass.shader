Shader "Custom/WindGrass"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Amount ("흔들리는 정도", Range(0,1)) = 0.1
		_Range("버텍스컬러 범위", Range(0,1)) = 0.5

	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float4 color : COLOR;
		};

		float _Amount;
		float _Range;

		void vert(inout appdata_full v)
		{
			v.color.rgb = 0;
			v.color.r += v.vertex.y + _Range;

			if (v.color.r >= 1)
			{
				v.vertex.z -= abs(sin(_Time.y)) * _Amount;

			}

		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG

    }
    FallBack "Diffuse"
}
