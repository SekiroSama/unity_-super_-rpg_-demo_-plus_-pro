Shader "URP/UnityChan/Skin_Standard"
{
    Properties
    {
        [MainColor] _BaseColor ("Main Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0.8, 0.8, 1, 1)
        _OutLineColor ("OutLine Color", Color) = (0, 0, 0, 0)
        _EdgeThickness ("Outline Thickness", Float) = 1
                
        [MainTexture] _BaseMap ("Diffuse", 2D) = "white" {}
        _FalloffSampler ("Falloff Control", 2D) = "white" {}
        _RimLightSampler ("RimLight Control", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Opaque" 
            "Queue"="Geometry" 
        }

        // --- Pass 1: Forward Lit (主体渲染) ---
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // URP 阴影必须开启这些关键字
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

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
                float3 viewDirWS : TEXCOORD2;
                // URP 阴影坐标声明
                float4 shadowCoord : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_FalloffSampler); SAMPLER(sampler_FalloffSampler);
            TEXTURE2D(_RimLightSampler); SAMPLER(sampler_RimLightSampler);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _ShadowColor;
                float _EdgeThickness;
            CBUFFER_END

            Varyings vert (Attributes input) {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);
                
                // 正确获取 URP 阴影坐标
                output.shadowCoord = GetShadowCoord(vertexInput);
                return output;
            }

            half4 frag (Varyings input) : SV_Target {
                // 1. 采样与准备
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                float3 normalWS = normalize(input.normalWS);
                float3 viewDirWS = normalize(input.viewDirWS);

                // 2. 获取主灯光数据
                // URP 中推荐使用这个函数获取包含阴影衰减的灯光信息
                Light mainLight = GetMainLight(input.shadowCoord);
                
                // 3. 核心卡渲模型 (N·L 映射)
                float nl = dot(normalWS, mainLight.direction) * 0.5 + 0.5;
                half shadowFactor = SAMPLE_TEXTURE2D(_FalloffSampler, sampler_FalloffSampler, float2(nl, 0.5)).r;
                
                // 融合阴影衰减 (外部物体的阴影)
                float combinedShadow = shadowFactor * mainLight.shadowAttenuation;
                
                half3 diffuseColor = lerp(_ShadowColor.rgb * baseColor.rgb, baseColor.rgb, combinedShadow);

                // 4. 边缘光 (Rim Light)
                float nv = 1.0 - saturate(dot(normalWS, viewDirWS));
                half rimFactor = SAMPLE_TEXTURE2D(_RimLightSampler, sampler_RimLightSampler, float2(nv, 0.5)).r;
                
                // 5. 颜色输出
                half3 finalRGB = (diffuseColor + (rimFactor * mainLight.color)) * mainLight.color;
                
                return half4(finalRGB, 1.0);
            }
            ENDHLSL
        }

        // --- Pass 2: Outline (标准 URP 描边) ---
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
                float4 _OutLineColor;
                float _EdgeThickness;
            CBUFFER_END

            Varyings vert (Attributes input) {
                Varyings output;
                // 标准 URP 顶点转换
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                // 沿法线方向挤出
                positionWS += normalWS * _EdgeThickness * 0.001;
                output.positionCS = TransformWorldToHClip(positionWS);
                return output;
            }

            half4 frag () : SV_Target {
                return _OutLineColor; // 描边颜色
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}