Shader "PIVO/StageSelect/Floor"
{
    Properties
    {
        _Color0("Perfect Clear Color", Color) = (1,1,1,1)		// 완전 클리어 색
		_Color1("Clear Color", Color) = (1,1,1,1)				// 클리어 색
		_Color2("Lock Color", Color) = (1,1,1,1)				// 잠김 색

        _MainTex0 ("Default Texture", 2D) = "white" {}			// 기본 텍스처
		_MainTex1 ("Clear Texture", 2D) = "white" {}			// 클리어 텍스처

		[Toggle]_IsPerfectClear("IsPerfectClear", Float) = 0
		[Toggle]_IsClear("IsClear", Float) = 0
		[Toggle]_IsUnlock("IsUnlock", Float) = 0
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

		float4 _Color0;
		float4 _Color1;
		float4 _Color2;

		float _IsPerfectClear;
		float _IsClear;
		float _IsUnlock;

        struct Input
        {
            float2 uv_MainTex0;
			float2 uv_MainTex1;

        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
			float4 defaultTexColor = tex2D(_MainTex0, IN.uv_MainTex0);
			float4 perfectClearTexColor = tex2D(_MainTex1, IN.uv_MainTex1);

			float3 finalColor = saturate(clamp(_IsPerfectClear, defaultTexColor, perfectClearTexColor).rgb +
								lerp(lerp(_Color2, _Color1, _IsUnlock), _Color0, saturate(_IsPerfectClear + _IsClear)));

			o.Albedo = finalColor;
			o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
