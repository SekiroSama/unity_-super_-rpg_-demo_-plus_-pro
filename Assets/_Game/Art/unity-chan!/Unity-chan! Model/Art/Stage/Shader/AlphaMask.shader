Shader "Custom/URP_AlphaMask" {
    Properties {
        [MainTexture] _BaseMap ("Base (RGB)", 2D) = "white" {}
        _AlphaMask ("Mask (A)", 2D) = "white" {}
    }

    SubShader {
        Tags { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
        }

        Pass {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            // 透明混合设置
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_AlphaMask);
            SAMPLER(sampler_AlphaMask);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _AlphaMask_ST;
            CBUFFER_END

            Varyings vert (Attributes input) {
                Varyings output;
                // 转换顶点到裁剪空间
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag (Varyings input) : SV_Target {
                // 计算各自的 UV（考虑 Tiling 和 Offset）
                float2 mainUV = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                float2 maskUV = input.uv * _AlphaMask_ST.xy + _AlphaMask_ST.zw;

                // 采样主纹理
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, mainUV);
                
                // 从遮罩纹理采样 Alpha 通道
                half maskAlpha = SAMPLE_TEXTURE2D(_AlphaMask, sampler_AlphaMask, maskUV).a;

                // 最终颜色输出
                return half4(color.rgb, maskAlpha);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Unlit"
}