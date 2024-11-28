Shader "Custom/DayNightWithFog"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _AmbientIntensity ("Ambient Intensity", Range(0, 1)) = 1.0
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _FogDensity ("Fog Density", Range(0, 1)) = 0.1
        _UseFog ("Enable Fog", Float) = 0.0 // Control fog effect
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
            };

            sampler2D _MainTex;
            float _AmbientIntensity;
            float _FogDensity;
            float _UseFog;
            float4 _FogColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Apply ambient lighting
                texColor.rgb *= _AmbientIntensity;

                // Calculate fog
                if (_UseFog > 0.5)
                {
                    float3 cameraPos = _WorldSpaceCameraPos.xyz;
                    float distance = length(i.worldPos - cameraPos);
                    float fogFactor = exp(-_FogDensity * distance);
                    fogFactor = saturate(fogFactor);

                    // Blend texture color with fog color
                    texColor = lerp(_FogColor, texColor, fogFactor);
                }

                return texColor;
            }
            ENDCG
        }
    }
}
