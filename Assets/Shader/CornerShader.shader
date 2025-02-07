Shader "Unlit/CornerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CornerRadius ("Corner Radius", Float) = 120
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CornerRadius;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            bool inRect(float2 uv, float2 origin, float2 end)
            {
                return (uv.x < end.x && uv.y < end.y) && (uv.x > origin.x && uv.y > origin.y);
            }

            bool isHidden(float2 uv, float2 size, float radius)
            {
                float2 center = size * 0.5;
                float2 a = float2(radius, radius);
                float2 b = float2(size.x - radius, size.y - radius);
                float2 c = float2(size.x - radius, radius);
                float2 d = float2(radius, size.y - radius);

                float2 rd_origin = float2(size.x - radius, 0);
                float2 rd_end = float2(size.x, radius);

                float2 lu_origin = float2(0, size.y - radius);
                float2 lu_end = float2(radius, size.y);

                if (!inRect(uv, a, b))
                {
                    float dis = radius - 1.0;
                    if (inRect(uv, float2(0.0, 0.0), a))
                    {
                        dis = distance(a, uv);
                    }
                    if (inRect(uv, b, size))
                    {
                        dis = distance(b, uv);
                    }
                    if (inRect(uv, rd_origin, rd_end))
                    {
                        dis = distance(c, uv);
                    }
                    if (inRect(uv, lu_origin, lu_end))
                    {
                        dis = distance(d, uv);
                    }
                    return dis > radius;
                }
                return false;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 pixelSize = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y);
                float maxR = min(pixelSize.x, pixelSize.y) * 0.5;

                float cornerRadius = min(_CornerRadius, maxR);
                float2 size = float2(pixelSize.x, pixelSize.y);

                if (isHidden(uv * size, size, cornerRadius))
                {
                    return float4(0, 0, 0, 0); // Transparent pixel
                }
                else
                {
                    return tex2D(_MainTex, i.uv); // Return texture color
                }
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
