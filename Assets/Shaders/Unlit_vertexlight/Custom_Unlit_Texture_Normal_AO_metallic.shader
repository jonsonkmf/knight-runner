Shader "Custom/Unlit/Vertex light/Texture bump AO metallic"
{
    Properties
    {
        _MainTex("Main texture", 2D) = "black" {}
        _Color("Color", Color) = (1,1,1,1)
        [Toggle] _UseAlpha("Use alpha transparency", Float) = 1
        
        [Space]
        _BumpMap ("Normal map", 2D) = "white" {}
        _MetallicGlossMap("Metalness", 2D) = "black" {}
        _Metallic("Metalness power", Range(0, 1)) = 0
        _RoughTex("Roughness", 2D) = "black" {}
        [PowerSlider(6)] _Roughness("Roughness", Range(0, 1)) = 1
        _AoTex("AO map", 2D) = "white"{}
        _AoLevel("AO power", Range(0.0, 1.0)) = 1
                
        [Header(Fresnel)]
        [Toggle] _Fresnel("Enable fresnel", Float) = 0
        [Toggle] _FresnelInverse("Inverse", Float) = 0
        _FresnelSmooth("Smooth", Range(0, 3)) = 0
        [PowerSlider(4)] _FresnelPower("Fresnel power", Range(0.01, 30)) = 1
        
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
                float3 viewD : TEXCOORD2;
                
                half3 tspace0 : TEXCOORD6; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD7; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD8; // tangent.z, bitangent.z, normal.z
                fixed4 diff : COLOR0; // ambient lighting color
                fixed4 directionLight : COLOR1; // direction lighting color
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _UseAlpha;
            sampler2D _BumpMap;
            sampler2D _MetallicGlossMap;
            float _Metallic;
            sampler2D _RoughTex;
            sampler2D _AoTex;
            float _AoLevel;
            
            fixed _Fresnel;
            fixed _FresnelInverse;
            float _FresnelSmooth;
            float _FresnelPower;
            float _Roughness;
            
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
                
                float3 world = mul(unity_ObjectToWorld, v.vertex);
                o.viewD = normalize(world - _WorldSpaceCameraPos);
                half nl = max(0, dot(o.normal, _WorldSpaceLightPos0.xyz));
                o.directionLight = nl * _LightColor0;
                o.diff = UNITY_LIGHTMODEL_AMBIENT;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;          
                fixed ao = tex2D(_AoTex, i.uv).r;
                float metal = tex2D(_MetallicGlossMap, i.uv).r * _Metallic;          
                float rough = tex2D(_RoughTex, i.uv).r * lerp(0, 5, _Roughness);          

                // нормали + карта нормалей
                float4 n = tex2D (_BumpMap, i.uv);
                float3 normal = UnpackNormal (n);
                half3 worldNormal;
                worldNormal.x = dot(i.tspace0, normal);
                worldNormal.y = dot(i.tspace1, normal);
                worldNormal.z = dot(i.tspace2, normal);

                // освещение
                col.rgb *= (i.diff + i.directionLight) * lerp(1.0, ao, _AoLevel);    
                                
                // проекция направления на пиксель на нормаль пикселя 
                float fresnel = abs(dot(worldNormal, i.viewD));
                fresnel = clamp(saturate(fresnel), 0, 1);
                // инверсия френеля
                fresnel = lerp(fresnel, 1 - fresnel, _FresnelInverse);
                // сила эффекта
                fresnel = pow(fresnel, _FresnelPower);
                
                // направление отражения
                float3 reflectDir = reflect(i.viewD, worldNormal);
                // размытие общее 
                //float smooth = (_Roughness * (1.7 - 0.7 * _Roughness) * 6) * rough * UNITY_SPECCUBE_LOD_STEPS;
                float smooth = rough * UNITY_SPECCUBE_LOD_STEPS;
                float smooth2 = rough * UNITY_SPECCUBE_LOD_STEPS * 2.0;
                // размытие от силы френеля (обратное)
                smooth += _FresnelSmooth * (fresnel * (_FresnelInverse) + (1 - fresnel) * (1 - _FresnelInverse)) * _Fresnel;
                smooth2 += _FresnelSmooth * (fresnel * (_FresnelInverse) + (1 - fresnel) * (1 - _FresnelInverse)) * _Fresnel;
                // отражение отсновной пробы
                float4 refl1 = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflectDir, smooth);
                // отражение соседней пробы 
                float3 refl2 = Unity_GlossyEnvironment(UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0), unity_SpecCube1_HDR, reflectDir, smooth2);
              
                float3 refl1Color = DecodeHDR(refl1, unity_SpecCube0_HDR);
                // цвет отражения - лерп между пробами с учётом веса
                //float3 reflectionColor = refl1Color;
                float3 reflectionColor = lerp(refl1Color, refl2, 1 - saturate(unity_SpecCube0_BoxMin.w));
              
                float4 ff = float4(reflectionColor, 1);
                // прозрачность отражения от силы френеля
                ff.a *= lerp(0, fresnel, _Fresnel) * metal;
                ff.a = clamp(ff.a, 0, 1);
                
                col = AdditiveMix(col, ff);
                col.a = col.a * _UseAlpha + (1 - _UseAlpha);
                return col;
            }
            ENDCG
        }
    }
}
