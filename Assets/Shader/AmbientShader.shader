Shader "Custom/DayNightWithFogAndFlashlight"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _AmbientIntensity ("Ambient Intensity", Range(0, 1)) = 1.0
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _FogDensity ("Fog Density", Range(0, 1)) = 0.1
        _UseFog ("Enable Fog", Float) = 0.0 // Control fog effect
        _FlashlightColor ("Flashlight Color", Color) = (1, 1, 1, 1)
        _FlashlightIntensity ("Flashlight Intensity", Range(0, 5)) = 1.0
        _FlashlightRadius("Flashligh Radius", Range(0, 20)) = 3
        _UseFlashlight ("Enable Flashlight", Float) = 0.0 // Enable/Disable flashlight
        _PlayerPosition ("Player Position", Vector) = (0, 0, 0, 0) // Player position
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
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };

            // Properties
            sampler2D _MainTex;
            float _AmbientIntensity;
            float _FogDensity;
            float _UseFog;
            float4 _FogColor;
            float _FlashlightIntensity;
            float _UseFlashlight;
            float _FlashlightRadius;
            float4 _FlashlightColor;
            float4 _PlayerPosition; // 

            // Shader input and output
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.vertex.xyz)); // Normalize the normal
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Apply ambient lighting based on Day/Night cycle
                float3 ambientColor = _AmbientIntensity * float3(1.0, 1.0, 1.0); // White light for day

                // Sample the base texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
                texColor.rgb *= ambientColor; // Apply ambient lighting to texture

                // Fog effect: apply if enabled
                if (_UseFog > 0.5)
                {
                    float3 cameraPos = _WorldSpaceCameraPos.xyz;
                    float distance = length(i.worldPos - cameraPos);
                    float fogFactor = exp(-_FogDensity * distance);
                    fogFactor = saturate(fogFactor);

                    // Blend the texture color with the fog color
                    texColor.rgb = lerp(_FogColor.rgb, texColor.rgb, fogFactor);
                }

                // Flashlight effect: apply if enabled
                if (_UseFlashlight > 0.5)
                {
                    float dist = distance(i.worldPos, _PlayerPosition.xyz);

                    if (dist < _FlashlightRadius) {
                        texColor.rgb *= _FlashlightColor.rgb * _FlashlightIntensity;
                    }


                    // Use player forward direction to calculate flashlight direction
                    // float3 flashlightDir = normalize(_PlayerPosition.xyz); // Player's facing direction
                    
                    // // Check if the surface normal is roughly aligned with the flashlight direction
                    // float dotProduct = dot(i.normal, flashlightDir);

                    // // Apply flashlight effect if the surface is facing the flashlight direction
                    // if (dotProduct > 0.1) // Adjust threshold for surface facing flashlight direction
                    // {
                    //     texColor.rgb *= _FlashlightColor.rgb * _FlashlightIntensity; // Apply flashlight effect uniformly
                    // }
                    else
                    {
                        texColor.a = 0.0; // If not facing the flashlight direction, make the object transparent
                    }
                }

                return texColor;
            }
            ENDCG
        }
    }
}
