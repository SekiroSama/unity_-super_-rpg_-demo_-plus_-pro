Shader "Custom/Weapon/Knife_magic_sword_RuneBlade"
{
   Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
        [HDR]_AuraColor("AuraColor", Color) = (1,1,1,1)
        _MainTex("MainTex", 2D) = ""{}
        _BumpMap("BumpMap", 2D) = ""{}
        _BumpScale("BumpScale", Range(0,1)) = 1
        _AuraWidth("AuraWidth", Range(0.01, 0.1)) = 0.1
        _AuraFlowMap("AuraFlowMap", 2D) = ""{}//噪波贴图（云雾图）
        _FlowSpeed("FlowSpeed", float) = 1
        //_DissolveVal ("DissolveVal", Range(0, 1)) = 0//溶解程度 在Properties配置会无视C#Shader广播
    }
    SubShader
    {
        Tags{ "RenderType"="Opaque" "Queue"="Geometry"}

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "Lighting.cginc"
        #include "AutoLight.cginc"
        float4 _MainColor;//漫反射颜色
        sampler2D _MainTex;//颜色纹理
        float4 _MainTex_ST;//颜色纹理的缩放和平移
        sampler2D _BumpMap;//法线纹理
        float4 _BumpMap_ST;//法线纹理的缩放和平移
        float _BumpScale;//凹凸程度
        sampler2D _AuraFlowMap;//噪波贴图（云雾图）
        float _DissolveVal = 0;//溶解程度
        ENDCG

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            struct v2f
            {
                float4 pos:SV_POSITION;
                float4 uv:TEXCOORD0;
                float4 TtoW0:TEXCOORD1;
                float4 TtoW1:TEXCOORD2;
                float4 TtoW2:TEXCOORD3;
                SHADOW_COORDS(4)
            };
            v2f vert (appdata_full v)
            {
                v2f data;
                //把模型空间下的顶点转到裁剪空间下
                data.pos = UnityObjectToClipPos(v.vertex);
                //计算纹理的缩放偏移
                data.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                data.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                //得到世界空间下的 顶点位置 用于之后在片元中计算视角方向（世界空间下的）
                //data.worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                //把模型空间下的法线、切线转换到世界空间下
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent);
                //计算副切线 计算叉乘结果后 垂直与切线和法线的向量有两条 通过乘以 切线当中的w，就可以确定是哪一条
                float3 worldBinormal = cross(normalize(worldTangent), normalize(worldNormal)) * v.tangent.w;
                //这个就是我们 切线空间到世界空间的 转换矩阵
                //data.rotation = float3x3( worldTangent.x, worldBinormal.x,  worldNormal.x,
                //                          worldTangent.y, worldBinormal.y,  worldNormal.y,
                //                          worldTangent.z, worldBinormal.z,  worldNormal.z);
                data.TtoW0 = float4(worldTangent.x, worldBinormal.x,  worldNormal.x, worldPos.x);
                data.TtoW1 = float4(worldTangent.y, worldBinormal.y,  worldNormal.y, worldPos.y);
                data.TtoW2 = float4(worldTangent.z, worldBinormal.z,  worldNormal.z, worldPos.z);
                TRANSFER_SHADOW(data);
                return data;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                clip(tex2D(_AuraFlowMap, i.uv.xy).r - _DissolveVal);

                //世界空间下光的方向
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                //世界空间下视角方向
                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                //通过纹理采样函数 取出法线纹理贴图当中的数据
                float4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                //将我们取出来的法线数据 进行逆运算并且可能会进行解压缩的运算，最终得到切线空间下的法线数据
                float3 tangentNormal = UnpackNormal(packedNormal);
                //乘以凹凸程度的系数
                tangentNormal.xy *= _BumpScale;
                tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
                //本质 就是在进行矩阵运算
                float3 worldNormal = float3(dot(i.TtoW0.xyz, tangentNormal), dot(i.TtoW1.xyz, tangentNormal), dot(i.TtoW2.xyz, tangentNormal));
                //接下来就来处理 带颜色纹理的 布林方光照模型计算
                //颜色纹理和漫反射颜色的 叠加
                fixed3 albedo = tex2D(_MainTex, i.uv.xy) * _MainColor.rgb;
                //兰伯特
                fixed3 lambertColor = _LightColor0.rgb * albedo.rgb * max(0, dot(worldNormal, normalize(lightDir)));
              
                UNITY_LIGHT_ATTENUATION(atten, i, worldPos);
              
               //布林方
                fixed3 color = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo + lambertColor * atten;
                return fixed4(color.rgb, 1);
            }
            ENDCG
        }
        Pass
        {
            Tags { "LightMode"="ForwardAdd" }
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd
            struct v2f
            {
                float4 pos:SV_POSITION;
                float4 uv:TEXCOORD0;
                float4 TtoW0:TEXCOORD1;
                float4 TtoW1:TEXCOORD2;
                float4 TtoW2:TEXCOORD3;
                SHADOW_COORDS(4)
            };
            v2f vert (appdata_full v)
            {
                v2f data;
                //把模型空间下的顶点转到裁剪空间下
                data.pos = UnityObjectToClipPos(v.vertex);
                //计算纹理的缩放偏移
                data.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                data.uv.zw = v.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                //得到世界空间下的 顶点位置 用于之后在片元中计算视角方向（世界空间下的）
                //data.worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                //把模型空间下的法线、切线转换到世界空间下
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent);
                //计算副切线 计算叉乘结果后 垂直与切线和法线的向量有两条 通过乘以 切线当中的w，就可以确定是哪一条
                float3 worldBinormal = cross(normalize(worldTangent), normalize(worldNormal)) * v.tangent.w;
                //这个就是我们 切线空间到世界空间的 转换矩阵
                //data.rotation = float3x3( worldTangent.x, worldBinormal.x,  worldNormal.x,
                //                          worldTangent.y, worldBinormal.y,  worldNormal.y,
                //                          worldTangent.z, worldBinormal.z,  worldNormal.z);
                data.TtoW0 = float4(worldTangent.x, worldBinormal.x,  worldNormal.x, worldPos.x);
                data.TtoW1 = float4(worldTangent.y, worldBinormal.y,  worldNormal.y, worldPos.y);
                data.TtoW2 = float4(worldTangent.z, worldBinormal.z,  worldNormal.z, worldPos.z);
                TRANSFER_SHADOW(data);
                return data;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                clip(tex2D(_AuraFlowMap, i.uv.xy).r - _DissolveVal);

                //世界空间下光的方向
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                //世界空间下视角方向
                float3 worldPos = float3(i.TtoW0.w, i.TtoW1.w, i.TtoW2.w);
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                //通过纹理采样函数 取出法线纹理贴图当中的数据
                float4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                //将我们取出来的法线数据 进行逆运算并且可能会进行解压缩的运算，最终得到切线空间下的法线数据
                float3 tangentNormal = UnpackNormal(packedNormal);
                //乘以凹凸程度的系数
                tangentNormal.xy *= _BumpScale;
                tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));
                //本质 就是在进行矩阵运算
                float3 worldNormal = float3(dot(i.TtoW0.xyz, tangentNormal), dot(i.TtoW1.xyz, tangentNormal), dot(i.TtoW2.xyz, tangentNormal));
                //接下来就来处理 带颜色纹理的 布林方光照模型计算
                //颜色纹理和漫反射颜色的 叠加
                fixed3 albedo = tex2D(_MainTex, i.uv.xy) * _MainColor.rgb;
                //兰伯特
                fixed3 lambertColor = _LightColor0.rgb * albedo.rgb * max(0, dot(worldNormal, normalize(lightDir)));
              
                UNITY_LIGHT_ATTENUATION(atten, i, worldPos);
              
               //布林方
                fixed3 color = UNITY_LIGHTMODEL_AMBIENT.rgb * albedo + lambertColor * atten;
                return fixed4(color.rgb, 1);
            }
            ENDCG
        }
        Pass
        {
            Tags{ "RenderType"="Transparent" "Queue"="Transparent"}
            Blend SrcAlpha One
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _AuraWidth;
            float _FlowSpeed;
            fixed3 _AuraColor;
            struct v2f
            {
                float4 pos:SV_POSITION;
                float3 wNormal: TEXCOORD0;
                float3 wPos: TEXCOORD1;
                float2 uv: TEXCOORD2;
            };
            v2f vert (appdata_full v)
            {
                v2f data;
                data.pos = UnityObjectToClipPos(v.vertex);
                data.wNormal = UnityObjectToWorldNormal(v.normal);
                data.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                data.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return data;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 noise = tex2D(_AuraFlowMap, float2(i.uv.x + _Time.y * _FlowSpeed, i.uv.y + _Time.y * _FlowSpeed)) ;

                return float4(_AuraColor * noise, 1 - _DissolveVal);
            }

            ENDCG
        }

    }
    Fallback "Diffuse"
}
