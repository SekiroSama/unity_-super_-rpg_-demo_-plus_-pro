Shader "URP/UnityChan/HairDoubleSided"
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
        [NoScaleOffset] _EnvMapSampler ("Environment Map", 2D) = "black" {} 
        [Normal] _NormalMapSampler ("Normal Map", 2D) = "bump" {} 
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
            Cull Off // 还原原本的 Double-sided 效果

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float4 tangentWS : TEXCOORD3; // w 用于符号
                float4 shadowCoord : TEXCOORD4;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_FalloffSampler); SAMPLER(sampler_FalloffSampler);
            TEXTURE2D(_SpecularReflectionSampler); SAMPLER(sampler_SpecularReflectionSampler);
            TEXTURE2D(_NormalMapSampler); SAMPLER(sampler_NormalMapSampler);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _ShadowColor;
                float _SpecularPower;
                float _EdgeThickness;
            CBUFFER_END

            Varyings vert (Attributes input) {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                
                // 处理切线空间用于法线贴图
                output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
                
                output.shadowCoord = GetShadowCoord(vertexInput);
                return output;
            }

            half4 frag (Varyings input) : SV_Target {
                // 1. 采样与准备
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                float3 viewDirWS = normalize(input.viewDirWS);
                
                // 2. 处理法线
                half4 normalSample = SAMPLE_TEXTURE2D(_NormalMapSampler, sampler_NormalMapSampler, input.uv);
                float3 normalTS = UnpackNormal(normalSample);
                float3 bitangentWS = cross(input.normalWS, input.tangentWS.xyz) * input.tangentWS.w;
                float3x3 tbn = float3x3(input.tangentWS.xyz, bitangentWS, input.normalWS);
                float3 normalWS = normalize(mul(normalTS, tbn));

                // 3. 光照计算
                Light mainLight = GetMainLight(input.shadowCoord);
                
                // 卡渲阴影映射
                float nl = dot(normalWS, mainLight.direction) * 0.5 + 0.5;
                half shadowFactor = SAMPLE_TEXTURE2D(_FalloffSampler, sampler_FalloffSampler, float2(nl, 0.5)).r;
                float combinedShadow = shadowFactor * mainLight.shadowAttenuation;
                half3 diffuseColor = lerp(_ShadowColor.rgb * baseColor.rgb, baseColor.rgb, combinedShadow);

                // 4. 高光 (Specular) 逻辑
                float3 halfDir = normalize(mainLight.direction + viewDirWS);
                float nh = saturate(dot(normalWS, halfDir));
                half specMask = SAMPLE_TEXTURE2D(_SpecularReflectionSampler, sampler_SpecularReflectionSampler, input.uv).r;
                half specular = pow(nh, _SpecularPower) * specMask;

                // 5. 组合最终颜色
                half3 finalRGB = (diffuseColor + (specular * mainLight.color)) * mainLight.color;
                
                return half4(finalRGB, baseColor.a);
            }
            ENDHLSL
        }

        // --- Pass 2: Outline (描边) ---
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front

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

            CBUFFER_START(UnityPerMaterial)
                float _EdgeThickness;
            CBUFFER_END

            Varyings vert (Attributes input) {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                // 头发描边通常比皮肤稍厚，修正偏移系数
                positionWS += normalWS * _EdgeThickness * 0.0012;
                output.positionCS = TransformWorldToHClip(positionWS);
                return output;
            }

            half4 frag () : SV_Target {
                return half4(0.2, 0.15, 0.1, 1.0); // 头发深色描边
            }
            ENDHLSL
        }

        // 修不好直接用fullback
        // --- Pass 3: Shadow Caster ---
        // Pass
        // {
        //     Name "ShadowCaster"
        //     Tags { "LightMode" = "ShadowCaster" }

        //     ZWrite On
        //     ZTest LEqual

        //     HLSLPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag

        //     1. 核心库
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        //     2. 【必需】手动引入材质通用库 (位于 core 包)
        //     这一步是为了防止 Lighting.hlsl 报 LerpWhiteTo 错误
        //     #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"

        //     3. 【必需】引入光照总库 (位于 universal 包)
        //     它会自动引入 Shadows.hlsl，并确保所有函数(如 GetShadowPositionHClip)可用
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        //     struct Attributes
        //     {
        //         float4 positionOS : POSITION;
        //         float3 normalOS : NORMAL;
        //     };

        //     struct Varyings
        //     {
        //         float4 positionCS : SV_POSITION;
        //     };

        //     Varyings vert (Attributes input)
        //     {
        //         Varyings output;

        //         1. 先转世界坐标
        //         float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
        //         float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

        //         2. 传给阴影函数 (现在引用了 Lighting.hlsl，这个函数一定存在)
        //         output.positionCS = GetShadowPositionHClip(positionWS, normalWS);

        //         return output;
        //     }

        //     half4 frag () : SV_Target
        //     {
        //         return 0;
        //     }
        //     ENDHLSL
        // }
    }
    FallBack "Universal Render Pipeline/Lit"
}