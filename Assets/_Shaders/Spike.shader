Shader "Custom/Spike"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BottomColor ("Bottom Color", Color) = (0.5,0.5,0.5,1)
        _Shit ("Shit", Float) = 0
        _Ass ("Ass", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION; // local space (object space) [blender file coords]
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // clip space
                float4 heightT : TEXCOORD0;
            };

            // clip space -> ndc (normalized device coordinates)
            // 1) take the clip space coord
            // 2) devide it by 4th component W
            // 3) 0,0 will be in the center
            // 4) z is just depth buffer value

            float4 _TopColor;
            float4 _BottomColor;

            float minY;
            float maxY;
            float _Shit;
            float _Ass;

            v2f vert (appdata v)
            {
                v2f o;


                
                //float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                //o.pos = UnityObjectToClipPos(v.vertex);
                o.pos = float4(_Time[0], _Time[1], _Time[2], 1);

                o.heightT = v.vertex.xxxx;
                return o;
                //minY = unity_ObjectToWorld._m13;

                // float t = (worldPos.y - minY) / 1.5;    
                // o.heightT = (t - 0.5) * 2;
                //o.heightT = frac(worldPos.y / _Shit) + _Ass;
                // o.heightT = t;

                // o.heightT =
                
                //return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(1, 1, 1, 1);
                // return lerp(_BottomColor, _TopColor, i.heightT);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Color"
}
