Shader "Custom/Weapon/Knife_Aura"
{
    Properties
    {
        [HDR]_AuraColor("AuraColor", Color) = (1,1,1,1)
        _AuraWidth ("AuraWidth", Range(0, 0.1)) = 0.01
        _AuraFlowMap("AuraFlowMap", 2D) = "white"{}
        _FlowSpeed("FlowSpeed", float) = 1
        _MaskTex ("MaskTex", 2D) = "white" { }
        _FresnelPower ("FresnelPower", Range(0, 10)) = 5
    }
    SubShader
    {
        Tags{ "RenderType"="Transparent" "Queue"="Transparent" }
        
        Stencil { Ref 250 Comp NotEqual }
        Blend One One
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _AuraColor;
                float _AuraWidth;
                float _FlowSpeed;
                float _FresnelPower;
            CBUFFER_END
            
            TEXTURE2D(_MaskTex); SAMPLER(sampler_MaskTex);
            TEXTURE2D(_AuraFlowMap); SAMPLER(sampler_AuraFlowMap);

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 worldNormal : TEXCOORD0; float3 worldPos : TEXCOORD1; float2 uv : TEXCOORD2; };

            Varyings vert (Attributes v) {
                Varyings o;
                float3 posOS = v.positionOS.xyz + v.normalOS * _AuraWidth;
                o.positionCS = TransformObjectToHClip(posOS);
                o.worldNormal = TransformObjectToWorldNormal(v.normalOS);
                o.worldPos = TransformObjectToWorld(posOS);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target {
                float3 viewDir = normalize(GetWorldSpaceViewDir(i.worldPos));
                float fresnel = pow(1.0 - saturate(dot(viewDir, normalize(i.worldNormal))), _FresnelPower);
                float2 flowUV = i.uv + _Time.y * _FlowSpeed;
                half3 noise = SAMPLE_TEXTURE2D(_AuraFlowMap, sampler_AuraFlowMap, flowUV).rgb;
                return half4(_AuraColor.rgb * noise * fresnel, 1.0);
            }
            ENDHLSL
        }
    }
}