Shader "portal/Portal
"
{
	Properties
	{
		_Color("Color", Color) = (0.000000,0.000000,1.000000,1.000000)
		_Shininess("shininess ", range(0.0,1000.0)) = 1.0
		_ShininessColor("ShininessColor", Color) = (0.000000,0.000000,1.000000,1.000000)
		_mainTex("Main TeX", 2D) = "white" {}
		_normalMap("Normal Map", 2D) = "bump" {}

		_OutlineColor("Outline color", Color) = (0.0,0.0,0.0,1)
		_OutlineWidth("outline width", range(1.0,2.0)) = 1.01

	}

		CGINCLUDE
		#include "UnityCG.cginc"


			struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : POSITION;
			float4 color : COLOR;
			float3 normal : NORMAL;
		};

		float4 _OutlineColor;
		float _OutlineWidth;

		v2f vert(appdata v)
		{
			v.vertex.xyz *= _OutlineWidth; 

			v2f O;
			O.pos = UnityObjectToClipPos(v.vertex);
			O.color = _OutlineColor;

			return O;

		}

		ENDCG

		SubShader
	{
			pass //render outline  **************************************************
		{
			ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag 



				half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

				ENDCG
		}
		//   NEW PASS for color  **************************************************


		Pass
	{
		Tags{ "LightMode" = "ForwardBase" }

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc" 

		
	uniform float4 _LightColor0;
	fixed4 _Color;
	float _Shininess;
	float4 _ShininessColor;

	sampler2D _mainTex;
	float4 _mainTex_ST;
	sampler2D _normalMap;
	float4 _normalMap_ST;

	


	//input structs
	struct vertexInput
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD3;
		float3 tangent : TANGENT;
		float3 biTangent : TEXCOORD0;
		float3 tangentSpaceLight : TEXCOORD2;
		float4 texcoord : TEXCOORD1;
	};
	struct vertexOutput
	{
		float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float4 col : COLOR;
		float4 posWorld : TEXCOORD1;
		float3 normDir: TEXCOORD0;
		float2 uv : TEXCOORD3;
		float3 tangentSpaceLight : TEXCOORD2;
		float4 Tex : TEXCOORD4;

	};



	//vertex function
	vertexOutput vert(vertexInput v)
	{
		vertexOutput o;

		o.vertex = UnityObjectToClipPos(v.vertex);
		o.normal = UnityObjectToWorldNormal(v.normal);  

		o.uv = TRANSFORM_TEX(v.uv,_mainTex);




		return o;

	}

	//fragment function
	float4 frag(vertexOutput i) : COLOR
	{



		fixed4 uvColor = tex2D(_mainTex,i.uv);


	return float4((uvColor.xyz), 1.0);


	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}
