Shader "Custom/FazaTutorial"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Position ("Positon", Vector) = (0, 0, 0, 0)
        _Radius ("Radius", Float) = 1
    }

    SubShader
    {
        Pass
        {
            Tags
            {
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
            }
            
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            Lighting Off
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                half4 vertex : POSITION;
            };

            struct v2f
            {
                half4 pos : SV_POSITION;
            };

            half4 _Color;
            half2 _Position;
            half _Radius;

            v2f vert (appdata v)
            {
                v2f o;

                o.pos = TransformObjectToHClip(v.vertex);
                
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half dist = distance(_Position, i.pos);
                half mask = dist > _Radius;
                
                return _Color * mask;
            }
            ENDHLSL
        }
    }
}
