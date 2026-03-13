
Shader "Custom/SpaceGradient" {
    Properties {
        _TopColor ("Top Color", Color) = (0.02, 0.02, 0.08, 1)
        _MidColor ("Mid Color", Color) = (0.05, 0.03, 0.12, 1)
        _BotColor ("Bottom Color", Color) = (0.01, 0.01, 0.04, 1)
        _GradientPower ("Gradient Power", Range(0.1, 5)) = 1.5
    }
    SubShader {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            float4 _TopColor;
            float4 _MidColor;
            float4 _BotColor;
            float _GradientPower;
            
            struct appdata {
                float4 vertex : POSITION;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldDir : TEXCOORD0;
            };
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldDir = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float3 dir = normalize(i.worldDir);
                float t = dir.y * 0.5 + 0.5; // 0 at bottom, 1 at top
                t = pow(t, _GradientPower);
                
                fixed4 col;
                if (t > 0.5) {
                    col = lerp(_MidColor, _TopColor, (t - 0.5) * 2.0);
                } else {
                    col = lerp(_BotColor, _MidColor, t * 2.0);
                }
                return col;
            }
            ENDCG
        }
    }
}