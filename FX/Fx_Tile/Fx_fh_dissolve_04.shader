// Upgrade NOTE: upgraded instancing buffer 'Fx_fh_dissolve_04' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FX/Fx_fh_dissolve_04"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Main_Tex_Opa("Main_Tex_Opa", Range( 0 , 1)) = 0
		_Main_Tex_mul("Main_Tex_mul", Range( 0 , 1)) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0
		[HDR]_Color("Color", Color) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Sparkle_Str("Sparkle_Str", Float) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma multi_compile_instancing
		#pragma surface surf Unlit keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample1;
		uniform float _Sparkle_Str;
		uniform float4 _Color;
		uniform float _Cutoff = 0;

		UNITY_INSTANCING_BUFFER_START(Fx_fh_dissolve_04)
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTex_ST)
#define _MainTex_ST_arr Fx_fh_dissolve_04
			UNITY_DEFINE_INSTANCED_PROP(float, _Main_Tex_mul)
#define _Main_Tex_mul_arr Fx_fh_dissolve_04
			UNITY_DEFINE_INSTANCED_PROP(float, _Main_Tex_Opa)
#define _Main_Tex_Opa_arr Fx_fh_dissolve_04
		UNITY_INSTANCING_BUFFER_END(Fx_fh_dissolve_04)

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 _MainTex_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTex_ST_arr, _MainTex_ST);
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST_Instance.xy + _MainTex_ST_Instance.zw;
			float2 panner14 = ( 1.0 * _Time.y * float2( 0.05,0 ) + float2( 1,1 ));
			float2 uv_TexCoord13 = i.uv_texcoord + panner14;
			float _Main_Tex_mul_Instance = UNITY_ACCESS_INSTANCED_PROP(_Main_Tex_mul_arr, _Main_Tex_mul);
			float2 panner17 = ( 1.0 * _Time.y * float2( 0.2,0 ) + float2( 1,1 ));
			float2 uv_TexCoord18 = i.uv_texcoord + panner17;
			float4 temp_output_16_0 = ( ( ( tex2D( _MainTex, uv_MainTex ) + tex2D( _TextureSample0, uv_TexCoord13 ) ) * _Main_Tex_mul_Instance ) + ( tex2D( _TextureSample1, uv_TexCoord18 ) * _Sparkle_Str ) );
			o.Emission = ( temp_output_16_0 * _Color ).rgb;
			float _Main_Tex_Opa_Instance = UNITY_ACCESS_INSTANCED_PROP(_Main_Tex_Opa_arr, _Main_Tex_Opa);
			float4 temp_output_8_0 = ( _Color.a * temp_output_16_0 * _Main_Tex_Opa_Instance );
			o.Alpha = temp_output_8_0.r;
			clip( temp_output_8_0.r - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;108;1136;903;902.9575;-208.2156;2.170996;True;True
Node;AmplifyShaderEditor.PannerNode;14;-1815.731,898.8131;Float;False;3;0;FLOAT2;1,1;False;2;FLOAT2;0.05,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;17;-1831.014,1152.467;Float;False;3;0;FLOAT2;1,1;False;2;FLOAT2;0.2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1556.736,837.6984;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1571.34,1075.232;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1266.202,552.2935;Float;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;None;de8784b748a99df48b18e18b1e14622b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-1302.167,804.6826;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;f12f08fd865b7c44887212821155b4fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-1311.285,1042.003;Float;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;False;0;None;eeaf18c2955783a47969051a3ce96ca4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-882.0372,1125.608;Float;False;Property;_Sparkle_Str;Sparkle_Str;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-579.0528,645.1579;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-502.1122,479.1141;Float;False;InstancedProperty;_Main_Tex_mul;Main_Tex_mul;2;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-305.0475,629.3661;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-684.3734,957.4362;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-427.916,1237.473;Float;False;Property;_Color;Color;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.544811,1.88887,2.538759,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-137.2186,894.7549;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;376.6684,1345.303;Float;False;InstancedProperty;_Main_Tex_Opa;Main_Tex_Opa;1;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;365.0661,887.5875;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;696.9339,1273.547;Float;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;999.8616,978.4509;Float;False;True;6;Float;ASEMaterialInspector;0;0;Unlit;Fx_fh_dissolve_04;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0;True;False;0;True;Transparent;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;1;14;0
WireConnection;18;1;17;0
WireConnection;10;1;13;0
WireConnection;15;1;18;0
WireConnection;11;0;1;0
WireConnection;11;1;10;0
WireConnection;2;0;11;0
WireConnection;2;1;3;0
WireConnection;19;0;15;0
WireConnection;19;1;22;0
WireConnection;16;0;2;0
WireConnection;16;1;19;0
WireConnection;4;0;16;0
WireConnection;4;1;5;0
WireConnection;8;0;5;4
WireConnection;8;1;16;0
WireConnection;8;2;9;0
WireConnection;0;2;4;0
WireConnection;0;9;8;0
WireConnection;0;10;8;0
ASEEND*/
//CHKSM=0DE6372AD83E124A0E2DCBDDB057DD904DD78A92