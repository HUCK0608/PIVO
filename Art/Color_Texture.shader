Shader "BlueCube/Color_Texture"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_Texture ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

		sampler2D _Texture;

		struct Input
		{
			float2 uv_Texture;
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
