Shader "BlueCube/Color_Texture"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Texture ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _Texture;

		struct Input
		{
			float2 uv_Texture;
			//float4 color:Color;
		};

        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed4 c = tex2D(_Texture, IN.uv_Texture);
  
            o.Albedo = c.rgb * _Color.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
