Shader "BlueCube/Wink"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		GrabPass{}

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _GrabTexture;

        struct Input
        {
			float4 screenPos;
            float2 uv_MainTex;
        };

        float4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float3 UV = IN.screenPos.rgb / IN.screenPos.a;
			float4 MainTexture = tex2D (_GrabTexture, UV) * _Color;
            o.Albedo = MainTexture.rgb;
            o.Alpha = MainTexture.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
