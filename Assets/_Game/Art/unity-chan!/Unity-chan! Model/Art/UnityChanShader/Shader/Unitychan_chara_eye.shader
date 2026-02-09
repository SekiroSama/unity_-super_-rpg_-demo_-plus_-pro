Shader "URP/UnityChan/EyeOpaque"
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
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // 包含 URP 核心库
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
                float3 viewDirWS : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_FalloffSampler); SAMPLER(sampler_FalloffSampler);
            TEXTURE2D(_RimLightSampler); SAMPLER(sampler_RimLightSampler);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _ShadowColor;
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // 1. 获取基础纹理颜色
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = normalize(input.viewDirWS);
                
                // 2. 获取主灯光数据
                Light mainLight = GetMainLight();
                
                // 3. 计算 NdotL 并通过 Falloff 贴图映射阴影
                // 这里还原了 CharaSkin 的核心：兰伯特光照结果决定了阴影的分布
                float nl = dot(normalWS, mainLight.direction) * 0.5 + 0.5;
                half shadowFactor = SAMPLE_TEXTURE2D(_FalloffSampler, sampler_FalloffSampler, float2(nl, 0.5)).r;
                
                // 混合阴影色与基础色
                half3 diffuseColor = lerp(_ShadowColor.rgb * baseColor.rgb, baseColor.rgb, shadowFactor);
                
                // 4. 边缘光 (Rim Light) 逻辑采样
                float nv = 1.0 - saturate(dot(normalWS, viewDirWS));
                half rimFactor = SAMPLE_TEXTURE2D(_RimLightSampler, sampler_RimLightSampler, float2(nv, 0.5)).r;
                
                // 5. 组合最终颜色
                // 卡渲通常将边缘光作为加色（Additive）混合
                half3 finalRGB = diffuseColor + (rimFactor * mainLight.color);
                
                // 乘以灯光颜色
                finalRGB *= mainLight.color;

                return half4(finalRGB, 1.0); // Opaque 版本 Alpha 固定为 1
            }
            ENDHLSL
        }

        // 建议添加 ShadowCaster Pass 以支持在 URP 中投射阴影
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert (Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
                return output;
            }

            half4 frag () : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}