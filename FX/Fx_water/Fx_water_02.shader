// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fx_water_02"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		Blend One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _TextureSample2;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord47 = i.uv_texcoord * float2( 1,0.8 );
			float2 panner48 = ( 1.0 * _Time.y * float2( 0,0.15 ) + uv_TexCoord47);
			float2 uv_TexCoord20 = i.uv_texcoord * float2( 1,0.8 );
			float2 panner19 = ( 1.0 * _Time.y * float2( 0,0.15 ) + uv_TexCoord20);
			float4 appendResult7 = (float4(0.0 , ( tex2D( _TextureSample2, panner19 ).r * 1.0 ) , 0.0 , 0.0));
			float4 temp_output_50_0 = saturate( ( ( ( tex2D( _TextureSample1, panner48 ) + float4( 0.03773582,0.03773582,0.03773582,0 ) ) * float4( 1,1,1,1 ) ) * tex2D( _TextureSample0, ( float4( i.uv_texcoord, 0.0 , 0.0 ) + appendResult7 ) ) ) );
			float4 temp_output_22_0 = ( ( temp_output_50_0 * temp_output_50_0 ) * 1.35 );
			o.Emission = ( temp_output_22_0 * temp_output_22_0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
211;706;1269;535;2119.898;-38.80619;1.505991;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-2024.65,473.0396;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.8;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;19;-1756.188,472.8041;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.15;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1087.794,-702.8306;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,0.8;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1475.382,464.697;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;9d2ff0b97876b5b429b8954f3277046f;8cf09ec37804ea14381048357436571e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-1158.248,201.0526;Float;True;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1103.392,448.8402;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;48;-590.7754,-408.5265;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.15;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;-870.2759,337.1509;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-855.529,-4.120205;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;29;-287.8896,-397.6779;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;None;22b916a8578744e408682acfca3aaf3a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;32;52.95682,-419.8778;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.03773582,0.03773582,0.03773582,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-544.971,154.513;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;362.3965,-290.9258;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;2.6698,269.5448;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;False;0;0;False;0;81a7dbf78d4a8de44b927558d2f732ef;22b916a8578744e408682acfca3aaf3a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;577.0106,197.633;Float;True;2;2;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;50;784.0486,193.8874;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;1073.742,595.9989;Float;False;Constant;_Float2;Float 2;7;0;Create;True;0;0;False;0;1.35;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;941.5925,180.6197;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;1225.438,55.2998;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;37;45.24681,1.537515;Float;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;1800.597,1.696247;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;34;2057.443,-129.4035;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Fx_water_02;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;4;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;0;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;20;0
WireConnection;17;1;19;0
WireConnection;18;0;17;1
WireConnection;48;0;47;0
WireConnection;7;0;16;0
WireConnection;7;1;18;0
WireConnection;29;1;48;0
WireConnection;32;0;29;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;31;0;32;0
WireConnection;1;1;6;0
WireConnection;30;0;31;0
WireConnection;30;1;1;0
WireConnection;50;0;30;0
WireConnection;14;0;50;0
WireConnection;14;1;50;0
WireConnection;22;0;14;0
WireConnection;22;1;49;0
WireConnection;38;0;22;0
WireConnection;38;1;22;0
WireConnection;34;2;38;0
ASEEND*/
//CHKSM=10EDE728EF4127F463352B6831848E4DD8409D02