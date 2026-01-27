Shader "Custom/Weapon/Knife_magic_sword_TrailRenderer_Color"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" { }
        [HDR] _TintColor ("TintColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull Off
        Tags { "Queue"="Transparent" "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _TintColor;


            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return (1,1,1,1);
            }
            ENDCG
        }
    }
}
