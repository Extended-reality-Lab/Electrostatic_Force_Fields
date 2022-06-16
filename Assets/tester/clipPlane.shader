// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Tutorial/021_Clipping_Plane"{
    //show values to edit in inspector
    Properties{
        _Color ("Tint", Color) = (0, 0, 0, 1)
        //_RenderMode ()
        _MainTex ("Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0
        _Metallic ("Metalness", Range(0, 1)) = 0
        [HDR]_Emission ("Emission", color) = (0,0,0)

        [HDR]_CutoffColor("Cutoff Color", Color) = (1,0,0,0)
    }

    CGINCLUDE

	float4 _PlaneCenter;
	float4 _PlaneNormal;

	float Distance2Plane(float3 pt) {

		float3 n = _PlaneNormal.xyz;
		float3 pt2 = _PlaneCenter.xyz;

		float d = (n.x*(pt.x - pt2.x)) + (n.y*(pt.y - pt2.y)) + (n.z*(pt.z - pt2.z)) / sqrt(n.x*n.x + n.y*n.y + n.z*n.z);

		return d;
	}

	ENDCG

    SubShader{
        //the material is completely non-transparent and is rendered at the same time as the other opaque geometry
        Tags{ "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}

        // render faces regardless if they point towards the camera or away from it
        //ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100
        
        CGPROGRAM
        //the shader is a surface shader, meaning that it will be extended by unity in the background
        //to have fancy lighting and other features
        //our surface shader function is called surf and we use our custom lighting model
        //fullforwardshadows makes sure unity adds the shadow passes the shader might need
        //vertex:vert makes the shader use vert as a vertex shader function
        #pragma surface surf Standard fullforwardshadows alpha
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;

        half _Smoothness;
        half _Metallic;
        half3 _Emission;

        float4 _Plane;

        float4 _CutoffColor;

        //input struct which is automatically filled by unity
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float facing : VFACE;
        };

        //the surface shader function which sets parameters the lighting function then uses
        void surf (Input i, inout SurfaceOutputStandard o) {
            //calculate signed distance to plane
            float distance = dot(i.worldPos, _Plane.xyz);
            distance = distance + _Plane.w;
            //discard surface above plane
            clip(-distance);

            float facing = i.facing * 0.5 + 0.5;

            // //normal color stuff
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            col *= _Color;
            o.Albedo = col.rgb * facing;
            o.Metallic = _Metallic * facing;
            o.Smoothness = _Smoothness * facing;
            o.Emission = lerp(_CutoffColor, _Emission, facing);
        }
        ENDCG
    }
    FallBack "Standard" //fallback adds a shadow pass so we get shadows on other objects
}