Shader "Custom/Spike"
{
    Properties
    {
        _BottomColor ("Bottom Color", Color) = (0, 0, 0, 1)
        _TopColor ("Top Color", Color) = (1, 1, 1, 1)
        _Height ("Height", Float) = 1.5
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
                fixed4 vertex : POSITION; // local space (object space) [blender file coords]
            };

            struct v2f
            {
                fixed4 pos : SV_POSITION; // clip space
                fixed height : TEXCOORD0;
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;
            fixed _Height;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.pos = UnityObjectToClipPos(v.vertex);
                
                o.height = v.vertex.y / _Height;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed t = easeInQuart(i.height);
                return lerp(_BottomColor, _TopColor, t);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Color"
}
