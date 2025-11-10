Shader "Sprites/ReplaceWithColorGlow"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,0,1)
        _GlowStrength ("Glow Strength", Range(0, 1)) = 0.4
        _GlowSize ("Glow Size", Range(0, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _GlowColor;
            float _GlowStrength;
            float _GlowSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                float alpha = tex.a;

                // "aura" simples: amostra pixels ao redor para simular brilho
                float glow = 0;
                for (float x = -_GlowSize; x <= _GlowSize; x++)
                {
                    for (float y = -_GlowSize; y <= _GlowSize; y++)
                    {
                        glow += tex2D(_MainTex, i.uv + float2(x, y) * _MainTex_TexelSize.xy).a;
                    }
                }

                glow /= pow((_GlowSize * 2 + 1), 2);

                fixed4 col = _Color;
                col.a *= alpha;

                fixed4 glowCol = _GlowColor;
                glowCol.a *= glow * _GlowStrength;

                // mistura brilho + sprite
                return col + glowCol * (1 - alpha);
            }
            ENDCG
        }
    }
}
