Shader "Custom/FazaShellGlow"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _ShellSizeMin ("Shell Size Min", Float) = 1
        _ShellSizeMax ("Shell Size Max", Float) = 1
        _SizeSpeed ("Size Speed", Float) = 1
        _Speed ("Speed", Float) = 1
    }

    SubShader
    {
        Pass
        {
            Tags
            {
                "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "LightMode" = "UniversalForward"
            }
            
            Blend One One
            Cull Off
            ZWrite Off
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                half4 pos : SV_POSITION;
                half2 uv  : TEXCOORD0;
            };

            half4 _Color;
            half _ShellSizeMin;
            half _ShellSizeMax;
            half _SizeSpeed;
            half _Speed;
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            v2f vert (appdata v)
            {
                v2f o;

                half size = lerp(_ShellSizeMin, _ShellSizeMax, saturate(sin(_Time.y * _SizeSpeed)));
                o.pos = TransformObjectToHClip(v.vertex + v.normal * size);
                o.uv = v.uv;
                
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half2 offset = float2(_Time.y * _Speed, _Time.y * _Speed);
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + offset);
                return tex * _Color;
            }
            ENDHLSL
        }
    }
}
