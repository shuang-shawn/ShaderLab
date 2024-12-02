Shader "Custom/FlashlightBeamSimple"
{
    Properties
    {
        _BeamColor ("Beam Color", Color) = (1, 1, 1, 1) // Color of the flashlight beam
        _BeamOpacity ("Beam Opacity", Range(0, 1)) = 0.5 // Transparency of the beam
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency
            Cull Back                      // Hide the inside of the cone
            ZWrite Off                     // Disable depth writing for transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION; // Vertex position
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // Screen-space position
            };

            float4 _BeamColor;
            float _BeamOpacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // Transform vertex to clip space
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return float4(_BeamColor.rgb, _BeamOpacity); // Apply beam color with opacity
            }
            ENDCG
        }
    }
}
