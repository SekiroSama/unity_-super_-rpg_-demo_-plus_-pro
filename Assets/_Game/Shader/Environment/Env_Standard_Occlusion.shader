Shader "Custom/Environment/StandardOcclusion_URP"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white"{}
        _BumpMap("BumpMap", 2D) = "bump"{}
        _BumpScale("BumpScale", Range(0,1)) = 1
        _ClipRadius ("ClipRadius", Range(0, 1)) = 0.9
    }
    SubShader
    {
        // URP 识别标签
        Tags{ "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline" = "UniversalPipeline"}
        
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // URP 关键字，用于接收阴影和光照设置
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // 材质数据定义 (SRP Batcher 兼容)
            CBUFFER_START(UnityPerMaterial)
                float4 _MainColor;
                float4 _MainTex_ST;
                float4 _BumpMap_ST;
                float _BumpScale;
                float4 _PlayerPos;
                float _ClipRadius;
            CBUFFER_END

            TEXTURE2D(_MainTex);      SAMPLER(sampler_MainTex);
            TEXTURE2D(_BumpMap);      SAMPLER(sampler_BumpMap);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0; // xy: Main, zw: Bump
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
            };

            // 你的 Dither 算法 (保持不变，除了参数类型微调)
            float Dither4x4(float2 screenPos, float alpha)
            {
                float4x4 baier = float4x4(1, 9, 3, 11,
                                            13, 5, 15, 7,
                                            4, 12, 2, 10,
                                            16, 8, 14, 6);
                // screenPos 已经是像素坐标
                float x = fmod(screenPos.x, 4);
                float y = fmod(screenPos.y, 4);
                float menkan = baier[(int)x][(int)y] / 17.0;
                return alpha - menkan;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;

                // 1. 顶点位置转换 (Object -> Clip)
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                
                // 2. 顶点位置转换 (Object -> World)
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);

                // 3. 法线/切线转换 (Object -> World)
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.normalWS = normalInput.normalWS;
                output.tangentWS = normalInput.tangentWS;
                output.bitangentWS = normalInput.bitangentWS;

                // 4. UV 处理
                output.uv = input.uv; // 稍后在片元做 ST 计算或者在这里做都可以，为了对其逻辑，这里简化

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // --- 1. 遮挡半透明逻辑 (Occlusion Logic) ---
                // 重构 UV
                float2 uvMain = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                float2 uvBump = input.uv * _BumpMap_ST.xy + _BumpMap_ST.zw;

                float3 playerPos = _PlayerPos.xyz;
                
                float3 camPos = GetCameraPositionWS();
                float3 lineVec = camPos - playerPos;
                float3 pixelVec = input.positionWS - playerPos;
                
                // 投影计算
                float t = dot(pixelVec, lineVec) / dot(lineVec, lineVec);
                float3 closestPoint = playerPos + lineVec * saturate(t);
                float dist = distance(input.positionWS, closestPoint);
                
                float alpha = (t < 0 || t > 1) ? 1 : smoothstep(0, _ClipRadius, dist);
                
                // Dither 裁剪 (input.positionCS.xy 在 HLSL 中即为屏幕像素坐标)
                clip(Dither4x4(input.positionCS.xy, alpha));

                // --- 2. 法线贴图处理 ---
                half4 packedNormal = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uvBump);
                float3 localNormal = UnpackNormalScale(packedNormal, _BumpScale);
                
                // 构建 TBN 矩阵并转换法线到世界空间
                float3x3 TBN = float3x3(normalize(input.tangentWS), normalize(input.bitangentWS), normalize(input.normalWS));
                float3 worldNormal = normalize(mul(localNormal, TBN)); // 注意乘法顺序，HLSL通常是 mul(vec, matrix) 或 mul(matrix, vec) 取决于矩阵构造

                // --- 3. 光照计算 (Main Light) ---
                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                
                half3 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvMain).rgb * _MainColor.rgb;
                
                // 兰伯特漫反射 (N dot L)
                half NdotL = saturate(dot(worldNormal, mainLight.direction));
                half3 lightingColor = albedo * (mainLight.color * mainLight.distanceAttenuation * mainLight.shadowAttenuation * NdotL);
                
                // 环境光
                lightingColor += albedo * SampleSH(worldNormal); // URP 获取环境光的方法

                // --- 4. 额外光照循环 (Additional Lights) ---
                #ifdef _ADDITIONAL_LIGHTS
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, input.positionWS);
                    half NdotL_add = saturate(dot(worldNormal, light.direction));
                    lightingColor += albedo * (light.color * light.distanceAttenuation * light.shadowAttenuation * NdotL_add);
                }
                #endif

                return half4(lightingColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}