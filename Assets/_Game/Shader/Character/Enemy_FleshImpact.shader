Shader "Custom/Enemy/FleshImpactURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HitRadius ("受击振动范围", float) = 0.5
        _WaveSpeed ("振动速度", float) = 50
        _WaveFrequency ("波纹密度", float) = 10
        _HitFalloff("衰减指数", Range(0.5, 5)) = 2
        _StrengthScale("振幅倍率", Range(0, 10)) = 1
    }
    SubShader
    {
        // 1. 增加 Tags
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM // 2. 使用 HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            // 3. 包含 URP 核心库
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float fogFactor : TEXCOORD1; // URP 雾效因子
            };

            // 4. 将变量放入 CBUFFER 以支持 SRP Batcher
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _HitPos;
                float _HitStrength;
                float _HitRadius;
                float _WaveSpeed;
                float _WaveFrequency;
                float _HitFalloff;
                float _StrengthScale;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            v2f vert (appdata v)
            {
                v2f o;

                // 5. 坐标转换逻辑修改
                float3 worldPos = TransformObjectToWorld(v.vertex.xyz);
                float dist = distance(worldPos, _HitPos.xyz);

                float import = pow(saturate((_HitRadius - dist) / max(_HitRadius, 0.001)), _HitFalloff) * _HitStrength * _StrengthScale;
                import *= sin(_Time.y * _WaveSpeed + dist * _WaveFrequency);

                // 将位移后的顶点转换到裁剪空间
                float3 displacedWorldPos = worldPos + TransformObjectToWorldDir(v.normal) * import;
                o.vertex = TransformWorldToHClip(displacedWorldPos);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // 6. 采样方式修改
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}