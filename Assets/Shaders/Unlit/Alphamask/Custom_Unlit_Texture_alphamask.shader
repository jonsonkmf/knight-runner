Shader "Custom/Unlit/Texture + Alpha mask"
{
    Properties
    {
        _MainTex("Main texture", 2D) = "black" {}
        _Color("Color", Color) = (1,1,1,1)
        [Toggle] _UseAlpha("Use alpha transparency", Float) = 1

        _AlphaMaskTex ("Alpha mask texture", 2D) = "white" {}
        _Cutoff ("Cutoff value", Range(0.0, 1.01)) = 0.0
        [MaterialToggle] _InverseMask ("Inverse mask", Float) = 0

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
        Tags
        {
            "RenderType" = "Transparent" "Queue" = "Transparent"
        }
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
        ZWrite [_ZWrite]
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Toolz.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _UseAlpha;

            sampler2D _AlphaMaskTex;
            float4 _AlphaMaskTex_ST;
            float _Cutoff;
            float _InverseMask;

            v2f vert(appdata_tan v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normal = normalize(UnityObjectToWorldNormal(v.normal));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw) * _Color;
                col.a = col.a * _UseAlpha + (1 - _UseAlpha);

                float4 alpha_mask = tex2D(_AlphaMaskTex, i.uv * _AlphaMaskTex_ST.xy + _AlphaMaskTex_ST.zw);
                col.a = col.a * step(alpha_mask.a + _InverseMask * (1.0 - alpha_mask.a * 2.0), _Cutoff);

                return col;
            }
            ENDCG
        }
    }
}
