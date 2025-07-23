Shader "Custom/FazaDefault"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Assets/_Shaders/Core/Faza.cginc"

            struct appdata
            {
                fixed4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
                fixed3 normal : NORMAL;
            };

            struct v2f
            {
                fixed4 pos : SV_POSITION;
                fixed2 uv : TEXCOORD0;
                fixed3 normal : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _ShadowColor;
            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = mul(unity_ObjectToWorld, v.normal);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // at the minimum i want
                // 1) texture sampling
                // 2) basic diffuse shading with 1 light source
                // 3) shadow color control
                // 4) normal color control
                
                // 5) shiny bleaks
                // 6) and ideally and optionally SHADOWS
                
                fixed d = dot(normalize(_WorldSpaceLightPos0), i.normal);
                fixed4 tex = tex2D(_MainTex, i.uv);

                return lerp(_ShadowColor * tex, tex * _Color, d);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Color"
}
