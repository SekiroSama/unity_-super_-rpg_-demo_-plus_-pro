Shader "Custom/Weapon/Knife_Main"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = "white"{}
        _BumpMap("BumpMap", 2D) = "bump"{}
        _BumpScale("BumpScale", Range(0,1)) = 1
        //_DissolveVal ("DissolveVal", Range(0, 1)) = 0
        _AuraFlowMap("AuraFlowMap", 2D) = "white"{}
    }
    SubShader
    {
        Tags{ "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "Queue"="Geometry"}
        
        Stencil { Ref 250 Comp Always Pass Replace }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                half4 _MainColor;
                float4 _MainTex_ST;
                float _BumpScale;
                float _DissolveVal = 0;
            CBUFFER_END
            
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);
            TEXTURE2D(_AuraFlowMap); SAMPLER(sampler_AuraFlowMap);

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float4 tangentOS : TANGENT; float2 uv : TEXCOORD0; };
            struct Varyings { float4 positionCS : SV_POSITION; float4 uv : TEXCOORD0; float3 worldPos : TEXCOORD1; half3 worldNormal : NORMAL; half4 worldTangent : TANGENT; };

            Varyings vert (Attributes v) {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.worldPos = TransformObjectToWorld(v.positionOS.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normalOS);
                o.worldTangent = half4(TransformObjectToWorldDir(v.tangentOS.xyz), v.tangentOS.w);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
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
    }
}