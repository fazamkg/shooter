Shader "Custom/FazaDefault"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _DiffuseLightDir ("Diffuse Light Direction", Vector) = (0, 0, 0)
        _SpecularLightDir ("Specular Light Direction", Vector) = (0, 0, 0)
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
        _Shininess ("Shininess", Float) = 32
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
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
            };

            float4 _Color;
            float4 _ShadowColor;
            float3 _DiffuseLightDir;
            float3 _SpecularLightDir;
            float4 _SpecColor;
            float _Shininess;
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

            float4 frag (v2f i) : SV_Target
            {
                Light mainLight = GetMainLight(i.shadowCoord);
                float3 diffuse_light_dir = normalize(_DiffuseLightDir);
                float3 specular_light_dir = normalize(_SpecularLightDir);
                float3 normal = i.normal;
                float shadow = mainLight.shadowAttenuation;
                float diffuse = dot(diffuse_light_dir, normal) * shadow;
                float4 tex = tex2D(_MainTex, i.uv);
                float3 view_direction = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 half_vector = normalize(specular_light_dir + view_direction);
                float specular = pow(saturate(dot(normal, half_vector)), _Shininess);

                float4 res = lerp(_ShadowColor * tex, tex * _Color, diffuse);
                res += _SpecColor * specular;
                res.a = 1.0;
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

            float3 _DiffuseLightDir;

            struct Attributes
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.vertex.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normal);
                output.pos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _DiffuseLightDir));
                return output;
            }

            float4 frag(Varyings i) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}
