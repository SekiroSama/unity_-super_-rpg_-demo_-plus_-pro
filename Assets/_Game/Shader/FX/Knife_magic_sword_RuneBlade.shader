Shader "Custom/Weapon/Knife_magic_sword_RuneBlade_URP"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        [HDR]_AuraColor("AuraColor", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white"{}
        _BumpMap("BumpMap", 2D) = "bump"{}
        _BumpScale("BumpScale", Range(0,1)) = 1
        _AuraWidth ("AuraWidth", Range(0, 0.1)) = 0.01
        _AuraFlowMap("AuraFlowMap", 2D) = "white"{}
        _FlowSpeed("FlowSpeed", float) = 1
        _DissolveVal ("DissolveVal", Range(0, 1)) = 0
        _MaskTex ("MaskTex", 2D) = "white" { }
        _FresnelPower ("FresnelPower", Range(0, 10)) = 5
    }

    SubShader
    {
        Tags{ "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "Queue"="Geometry"}

        // ---------------- Pass 1: 刀身主体 ----------------
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float4 uv         : TEXCOORD0; 
                float3 worldPos   : TEXCOORD1;
                half3  worldNormal: NORMAL;
                half4  worldTangent: TANGENT;
            };

            // 重要：所有 Properties 里的变量必须全部放进这里
            CBUFFER_START(UnityPerMaterial)
                half4 _MainColor;
                half4 _AuraColor;
                float4 _MainTex_ST;
                float4 _BumpMap_ST;
                float _BumpScale;
                float _AuraWidth;
                float _FlowSpeed;
                float _DissolveVal;
                float _FresnelPower;
            CBUFFER_END

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);
            TEXTURE2D(_AuraFlowMap); SAMPLER(sampler_AuraFlowMap);

            Varyings vert (Attributes v) {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.worldPos = TransformObjectToWorld(v.positionOS.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normalOS);
                o.worldTangent = half4(TransformObjectToWorldDir(v.tangentOS.xyz), v.tangentOS.w);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _BumpMap);
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                clip(SAMPLE_TEXTURE2D(_AuraFlowMap, sampler_AuraFlowMap, i.uv.xy).r - _DissolveVal);
                
                half3 viewDir = normalize(GetWorldSpaceViewDir(i.worldPos));
                half3 bitangent = cross(i.worldNormal, i.worldTangent.xyz) * i.worldTangent.w;
                half3x3 TBN = half3x3(i.worldTangent.xyz, bitangent, i.worldNormal);
                half3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, i.uv.zw), _BumpScale);
                half3 worldNormal = normalize(mul(normalTS, TBN));

                Light light = GetMainLight();
                half3 albedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy).rgb * _MainColor.rgb;
                half3 diffuse = light.color * albedo * saturate(dot(worldNormal, light.direction));
                half3 ambient = SampleSH(worldNormal) * albedo;

                return half4(diffuse + ambient, 1.0);
            }
            ENDHLSL
        }

        // ---------------- Pass 2: 菲涅尔发光外壳 ----------------
        Pass
        {
            Name "Aura"
            // 在 URP 多 Pass 渲染中，第二个 Pass 通常使用 SRPDefaultUnlit 或 UniversalForward
            Tags{ "LightMode"="SRPDefaultUnlit" "Queue"="Transparent" "RenderType"="Transparent" }
            
            Blend One One   // 线性减淡（加法混合）
            ZWrite Off      // 关闭深度写入
            Cull Off        // 双面渲染，增加发光厚度

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 worldNormal: TEXCOORD0;
                float3 worldPos   : TEXCOORD1;
                float2 uv         : TEXCOORD2;
            };

            // 共享同一个 CBUFFER
            CBUFFER_START(UnityPerMaterial)
                half4 _MainColor;
                half4 _AuraColor;
                float4 _MainTex_ST;
                float4 _BumpMap_ST;
                float _BumpScale;
                float _AuraWidth;
                float _FlowSpeed;
                float _DissolveVal;
                float _FresnelPower;
            CBUFFER_END
            
            TEXTURE2D(_MaskTex); SAMPLER(sampler_MaskTex);
            TEXTURE2D(_AuraFlowMap); SAMPLER(sampler_AuraFlowMap);

            Varyings vert (Attributes v) {
                Varyings o;
                // 外扩顶点
                float3 posOS = v.positionOS.xyz + v.normalOS * _AuraWidth;
                o.positionCS = TransformObjectToHClip(posOS);
                o.worldNormal = TransformObjectToWorldNormal(v.normalOS);
                o.worldPos = TransformObjectToWorld(posOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                // 计算菲涅尔
                float3 viewDir = normalize(GetWorldSpaceViewDir(i.worldPos));
                float fresnel = pow(1.0 - saturate(dot(viewDir, normalize(i.worldNormal))), _FresnelPower);
                
                // 遮罩与噪波
                half mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv).r;
                float2 flowUV = i.uv + _Time.y * _FlowSpeed;
                half3 noise = SAMPLE_TEXTURE2D(_AuraFlowMap, sampler_AuraFlowMap, flowUV).rgb;

                // 结果计算
                half3 finalRGB = _AuraColor.rgb * noise * fresnel * mask * (1.0 - _DissolveVal);
                return half4(finalRGB, 1.0);
            }
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}