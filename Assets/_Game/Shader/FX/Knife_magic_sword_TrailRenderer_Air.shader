Shader "Custom/Weapon/Knife_magic_sword_TrailRenderer_Air"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _DistortionTex ("DistortionTex", 2D) = "white" { }
        _Strength ("Strength", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Cull Off
        Tags { "Queue"="Transparent" "RenderType" = "Opaque" }
        GrabPass { "_GrabTexture" }

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
                float4 grabPos : TEXCOORD1;
            };

            //sampler2D _MainTex;
            //float4 _MainTex_ST;
            sampler2D _GrabTexture;
            sampler2D _DistortionTex;
            sampler2D _DistortionTex_ST;
            float _Strength;


            v2f vert (appdata v)
            {
                v2f o;
                o.uv = v.uv;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 扭曲法线值TrailRenderer的特性：它的面片通常是永远面朝摄像机，所以可以直接采样法线并舍去z
                fixed4 packedNormal = tex2D(_DistortionTex, i.uv);
                //UnpackNormal得到的是切线空间的法线，我们只需要x和y分量来进行屏幕空间的偏移
                fixed3 normal = UnpackNormal(packedNormal);
                // sample the texture
                i.grabPos.xy += normal.xy * _Strength;
                return tex2Dproj(_GrabTexture, i.grabPos);
            }
            ENDCG
        }
    }
}
