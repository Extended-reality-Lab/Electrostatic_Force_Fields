// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "InsideVisible" 
{
    Properties 
    {
        _Color ("Tint", Color) = (0, 0, 0, 1)
        //_RenderMode ()
        _MainTex ("Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0
        _Metallic ("Metalness", Range(0, 1)) = 0
        [HDR]_Emission ("Emission", color) = (0,0,0)

        [HDR]_CutoffColor("Cutoff Color", Color) = (1,0,0,0)
    }

    SubShader 
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
        LOD 100

        Pass 
        {
            CGPROGRAM

            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata_t 
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f 
            {
                float4 vertex  : SV_POSITION;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;

                o.vertex     = UnityObjectToClipPos(v.vertex);
                v.texcoord.x = 1 - v.texcoord.x;
                o.texcoord   = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color; // multiply by _Color
                return col;
            }

            ENDCG
        }
        Pass 
        {
            CGPROGRAM
            //the shader is a surface shader, meaning that it will be extended by unity in the background
            //to have fancy lighting and other features
            //our surface shader function is called surf and we use our custom lighting model
            //fullforwardshadows makes sure unity adds the shadow passes the shader might need
            //vertex:vert makes the shader use vert as a vertex shader function
            #pragma surface surf Standard fullforwardshadows
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
    }
}