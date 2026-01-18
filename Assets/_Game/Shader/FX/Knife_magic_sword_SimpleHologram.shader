Shader "Unlit/Knife_magic_sword_SimpleHologram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]
        _HoloColor("_HoloColor", Color) = (1,1,1,1)
        _ScrollSpeed("_ScrollSpeed", Range(0,20)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        Pass
        {
            Blend One One
            ZWrite Off 

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
            fixed3 _HoloColor;
            float _ScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed3 col = tex2D(_MainTex, i.uv + _Time.y * _ScrollSpeed).rgb * _HoloColor;
                return float4(col, 1);
            }
            ENDCG
        }
    }
}
