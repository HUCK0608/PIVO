Shader "BlueCube/WindGrass"
{
    Properties
    {
        _MainTex ("3D_Texture", 2D) = "white" {}
		_MainTex2("2D_Texture", 2D) = "white" {}
		[Toggle]_IsUse2DTexture("IsUse2DTexture", float) = 0
		_Amount ("흔들리는 정도", Range(0,1)) = 0.1
		_Range("버텍스컬러 범위", Range(0,1)) = 0

	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard vertex:vert addshadow
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_MainTex2;
			float4 vertex:POSITION;
			float4 color : COLOR;
		};

		float _Amount;
		float _Range;
		float _IsUse2DTexture;
		

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
			float4 c = tex2D (_MainTex, IN.uv_MainTex);
			float4 d = tex2D(_MainTex2, IN.uv_MainTex2);
			o.Albedo = lerp(c.rgb, d.rgb, _IsUse2DTexture);
            o.Alpha = c.a;
        }

        ENDCG

    }
    FallBack "Diffuse"
}
