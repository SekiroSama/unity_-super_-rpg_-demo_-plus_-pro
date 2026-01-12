Shader "Custom/Enemy/FleshImpact"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HitRadius ("受击振动范围", float) = 0.5
        _WaveSpeed ("振动速度", float) = 50
        _WaveFrequency ("波纹密度", float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _HitPos;
            float _HitStrength;
            float _HitRadius;
            float _WaveSpeed;
            float _WaveFrequency;

            v2f vert (appdata v)
            {
                v2f o;

                float3 wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float dist = distance(wpos, _HitPos.xyz);

                float import = saturate((_HitRadius - dist) / _HitRadius) * _HitStrength;
                import *= sin(_Time.y * _WaveSpeed + dist * _WaveFrequency);

                o.vertex = UnityObjectToClipPos(v.vertex + import * v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
