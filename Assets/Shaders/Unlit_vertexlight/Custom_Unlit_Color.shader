Shader "Custom/Unlit/Vertex light/Color"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0)

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
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityImageBasedLighting.cginc"
            #include "UnityShaderVariables.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                fixed4 diff : COLOR0; // ambient lighting color
                fixed4 directionLight : COLOR1; // direction lighting color
            };

            float4 _Color;
            
            v2f vert (appdata_tan v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));                
                half nl = max(0, dot(o.normal, _WorldSpaceLightPos0.xyz));
                o.directionLight = nl * _LightColor0;
                o.diff = UNITY_LIGHTMODEL_AMBIENT;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                col.rgb *= (i.diff + i.directionLight);    
                return col;
            }
            ENDCG
        }
    }
}
