Shader "Custom/Weapon/Knife_magic_sword_FlowingLight"
{
    Properties
    {
        _MainTex ("BloomTex", 2D) = "white" {}
        [HDR]_Color ("Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Float) = 1.0
        //_DissolveVal ("DissolveVal", Range(0, 1)) = 0//溶解程度
    }
    SubShader
    {
        //透明物品
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend One One
        //背面剔除关闭  默认设置是：Cull Back
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Speed;
            float _DissolveVal;//溶解程度

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //缩放偏移的内置宏
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv = float2(i.uv.x + _Time.x * _Speed, i.uv.y); // UV 偏移，控制流光速度和方向
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                color.rgb *= 1.0 - _DissolveVal; // 根据溶解程度调整透明度
                return color;
            }
            ENDCG
        }
    }
}