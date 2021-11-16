Shader "Custom/Unlit/Color + Alpha mask"
{
    Properties
    {
        [HideInInspector] __dirty( "", Int ) = 1
        _Color("Color", Color) = (0,0,0,0)

        _AlphaMaskTex ("Alpha mask texture", 2D) = "white" {}
        _Cutoff ("Cutoff value", Range(0.0, 1.01)) = 0.0
        [MaterialToggle] _InverseMask ("Inverse mask", Float) = 0

        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"

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
        Tags
        {
            "RenderType"="Opaque" "Queue" = "Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Cull [_Cull]
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            sampler2D _AlphaMaskTex;
            float4 _AlphaMaskTex_ST;
            float _Cutoff;
            float _InverseMask;

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Color;
                float4 alpha_mask = tex2D(_AlphaMaskTex, i.uv * _AlphaMaskTex_ST.xy + _AlphaMaskTex_ST.zw);
                col.a = col.a * step(alpha_mask.a + _InverseMask * (1.0 - alpha_mask.a * 2.0), _Cutoff);

                return col;
            }
            ENDCG
        }
    }
}
