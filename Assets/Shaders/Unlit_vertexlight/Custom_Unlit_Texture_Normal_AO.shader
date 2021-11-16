Shader "Custom/Unlit/Vertex light/Texture bump AO"
{
    Properties
    {
        _MainTex("Main texture", 2D) = "black" {}
        _Color("Color", Color) = (1,1,1,1)
        [Toggle] _UseAlpha("Use alpha transparency", Float) = 1
        
        [Space]
        _BumpMap ("Normal map", 2D) = "white" {}
        _AoTex("AO map", 2D) = "white"{}
        _AoLevel("AO power", Range(0.0, 1.0)) = 1
        
        [Space]
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
        [Enum(Off,0,On,1)]_ZWrite ("ZWrite", Float) = 1.0

        [Header(Stencil)]
		_Stencil ("Stencil ID", Float) = 0
		_ReadMask ("ReadMask", Int) = 255
		_WriteMask ("WriteMask", Int) = 255
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("Stencil Fail", Int) = 0
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("Stencil ZFail", Int) = 0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }        
        Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
        Cull [_Cull]
        ZWrite [_ZWrite]
   		Stencil
		{
			Ref [_Stencil]
			ReadMask [_ReadMask]
			WriteMask [_WriteMask]
			Comp [_StencilComp]
			Pass [_StencilOp]
			Fail [_StencilFail]
			ZFail [_StencilZFail]
		}
		//ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Toolz.cginc"
            #include "UnityImageBasedLighting.cginc"
            #include "UnityShaderVariables.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                
                half3 tspace0 : TEXCOORD6; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD7; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD8; // tangent.z, bitangent.z, normal.z
                fixed4 diff : COLOR0; // ambient lighting color
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed _UseAlpha;
            sampler2D _BumpMap;
            sampler2D _AoTex;
            float _AoLevel;
            fixed4 _Color;
            
            v2f vert (appdata_tan v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));                

                // ==== нормали и касательные ====
                half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
                half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(o.normal, wTangent) * tangentSign;
                o.tspace0 = half3(wTangent.x, wBitangent.x, o.normal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, o.normal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, o.normal.z);
                
                half nl = max(0, dot(o.normal, _WorldSpaceLightPos0.xyz));
                o.diff = UNITY_LIGHTMODEL_AMBIENT;
                
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;          
                fixed ao = tex2D(_AoTex, i.uv).r;

                // нормали + карта нормалей
                float4 n = tex2D (_BumpMap, i.uv);
                float3 normal = UnpackNormal (n);
                half3 worldNormal;
                worldNormal.x = dot(i.tspace0, normal);
                worldNormal.y = dot(i.tspace1, normal);
                worldNormal.z = dot(i.tspace2, normal);

                // освещение
                col.rgb *= (i.diff + max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz))) * lerp(1.0, ao, _AoLevel);    
                col.a = col.a * _UseAlpha + (1 - _UseAlpha);
                return col;
            }
            ENDCG
        }
    }
}
