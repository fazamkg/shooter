Shader "Custom/FazaDefault"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _DiffuseLightDir ("Diffuse Light Direction", Vector) = (0, 0, 0)
    }

    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
                "LightMode" = "UniversalForward"
            }
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                half4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                half3 normal : TEXCOORD1;
                half3 worldPos : TEXCOORD2;
                half4 shadowCoord : TEXCOORD3;
            };

            half4 _Color;
            half4 _ShadowColor;
            half3 _DiffuseLightDir;
            sampler2D _MainTex;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                o.normal = TransformObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.shadowCoord = TransformWorldToShadowCoord(o.worldPos.xyz);
                
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                Light mainLight = GetMainLight(i.shadowCoord);
                half3 diffuse_light_dir = normalize(_DiffuseLightDir);
                half3 normal = i.normal;
                half shadow = mainLight.shadowAttenuation;
                half diffuse = dot(diffuse_light_dir, normal) * shadow;
                half4 tex = tex2D(_MainTex, i.uv);

                half4 res = lerp(_ShadowColor * tex, tex * _Color, diffuse);
                return res;
            }
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            half3 _DiffuseLightDir;

            struct Attributes
            {
                half4 vertex : POSITION;
                half3 normal : NORMAL;
            };

            struct Varyings
            {
                half4 pos : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                half3 positionWS = TransformObjectToWorld(input.vertex.xyz);
                half3 normalWS = TransformObjectToWorldNormal(input.normal);
                output.pos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _DiffuseLightDir));
                return output;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return 0.0h;
            }
            ENDHLSL
        }
    }
}
