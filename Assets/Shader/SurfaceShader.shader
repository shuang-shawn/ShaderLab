Shader "Tutorial/005_surface_2" {
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}


			CGPROGRAM


			#pragma surface surf Standard


			sampler2D _MainTex;
			fixed4 _Color;


			struct Input {
				float2 uv_MainTex;
			};


			void surf(Input i, inout SurfaceOutputStandard o) {
				fixed4 col = tex2D(_MainTex, i.uv_MainTex);
				col *= _Color;
				o.Albedo = col.rgb;
			}
			ENDCG
	}
}
