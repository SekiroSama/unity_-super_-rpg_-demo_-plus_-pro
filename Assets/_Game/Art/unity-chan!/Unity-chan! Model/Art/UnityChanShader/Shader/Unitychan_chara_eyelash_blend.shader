Shader "URP/UnityChan/EyelashTransparent"
{
    Properties
    {
        [MainColor] _BaseColor ("Main Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0.8, 0.8, 1, 1)
    
        [MainTexture] _BaseMap ("Diffuse", 2D) = "white" {}
        _FalloffSampler ("Falloff Control", 2D) = "white" {}
        _RimLightSampler ("RimLight Control", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent+2" // 确保在眼睛 (Transparent+1) 之后渲染
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            // 还原混合模式
            Blend SrcAlpha OneMinusSrcAlpha, One One
            ZWrite Off
            Cull Back
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_FalloffSampler); SAMPLER(sampler_FalloffSampler);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _ShadowColor;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // 1. 采样贴图颜色与透明度
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                
                // 2. 获取主光源信息
                Light mainLight = GetMainLight();
                
                // 3. 简单的卡渲阴影计算
                // N·L 决定了睫毛是被提亮还是变暗（ShadowColor）
                float nl = dot(normalize(input.normalWS), mainLight.direction) * 0.5 + 0.5;
                
                // 采样 Falloff 控制（模拟 CharaSkin 逻辑）
                half shadowFactor = SAMPLE_TEXTURE2D(_FalloffSampler, sampler_FalloffSampler, float2(nl, 0.5)).r;
                half3 finalRGB = lerp(_ShadowColor.rgb * baseColor.rgb, baseColor.rgb, shadowFactor);
                
                // 4. 叠加光照颜色
                finalRGB *= mainLight.color;

                return half4(finalRGB, baseColor.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Unlit"
}