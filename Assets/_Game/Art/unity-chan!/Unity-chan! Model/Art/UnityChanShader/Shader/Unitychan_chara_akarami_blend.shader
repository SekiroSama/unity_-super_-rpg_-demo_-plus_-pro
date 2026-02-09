Shader "URP/UnityChan/BlushTransparent"
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
            "Queue" = "Transparent+3" // 确保在睫毛之后渲染，处于面部最顶层
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            // 还原原版的混合模式：半透明混合 + 亮部叠加
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
                // 1. 采样贴图颜色
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                
                // 2. 获取主光源
                Light mainLight = GetMainLight();
                
                // 3. 计算 NdotL 映射
                // 即使是腮红，也需要根据面部法线接收光照，否则在暗处会显得太亮
                float nl = dot(normalize(input.normalWS), mainLight.direction) * 0.5 + 0.5;
                
                // 采样 Falloff 控制，保持与皮肤一致的光影色调
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