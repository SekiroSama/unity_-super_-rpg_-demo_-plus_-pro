Shader "Unlit/流光效果"
{
    Properties
    {
        _MainTex ("BloomTex", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Float) = 1.0
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
                return color;
            }
            ENDCG
        }
    }
}
//1. 引入时间变量：利用 Unity 内置的时间变量 _Time。
//    * 知识点：_Time 是一个 float4 变量，其分量定义为 (t/20, t, 2t, 3t)，其中 t 代表场景加载开始经过的时间。
//2. UV 偏移：让 UV 坐标沿着一个固定的方向（如 U 方向或 V 方向）持续递增。
//    * 公式逻辑：NewUV = OriginalUV + Speed * _Time
//3. 采样贴图：使用偏移后的 UV 坐标对流光纹理贴图进行采样。
//4. 结果：纹理会在模型表面产生滑动的视觉效果，即“流光”