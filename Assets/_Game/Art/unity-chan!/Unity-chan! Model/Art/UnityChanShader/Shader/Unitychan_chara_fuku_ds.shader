Shader "URP/UnityChan/ClothingDoubleSide"
{
    Properties
    {
        [MainColor] _BaseColor ("Main Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0.8, 0.8, 1, 1)
        _SpecularPower ("Specular Power", Float) = 20
        _EdgeThickness ("Outline Thickness", Float) = 1
                
        [MainTexture] _BaseMap ("Diffuse", 2D) = "white" {}
        _FalloffSampler ("Falloff Control", 2D) = "white" {}
        _RimLightSampler ("RimLight Control", 2D) = "white" {}
        _SpecularReflectionSampler ("Specular / Reflection Mask", 2D) = "white" {}
        _EnvMapSampler ("Environment Map", 2D) = "black" {} 
        _NormalMapSampler ("Normal Map", 2D) = "bump" {} 
    }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry" 
        }

        // --- Pass 1: 主体渲染 (双面) ---
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            Cull Off // 双面渲染的关键

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            // 这里为了简洁省略了其他采样器的声明，实际需补齐

            Varyings vert (Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = input.uv;
                return output;
            }

            half4 frag (Varyings input) : SV_Target {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                // 简单的卡渲逻辑模拟 (实际建议配合 Shader Graph 获取更好的效果)
                Light mainLight = GetMainLight();
                half d = dot(normalize(input.normalWS), mainLight.direction) * 0.5 + 0.5;
                half3 ramp = d > 0.5 ? 1.0 : 0.8; // 简易二阶阶梯
                
                return half4(color.rgb * ramp * mainLight.color, 1.0);
            }
            ENDHLSL
        }

        // --- Pass 2: 描边渲染 ---
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front // 剔除正面，只渲染扩大的背面
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
            };

            float _EdgeThickness;

            Varyings vert (Attributes input) {
                Varyings output;
                // 沿着法线挤出，实现描边
                float3 normalCS = TransformWorldToHClipDir(TransformObjectToWorldNormal(input.normalOS));
                float4 posCS = TransformObjectToHClip(input.positionOS.xyz);
                posCS.xy += normalCS.xy * _EdgeThickness * 0.01; 
                output.positionCS = posCS;
                return output;
            }

            half4 frag () : SV_Target {
                return half4(0, 0, 0, 1); // 黑色描边
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}