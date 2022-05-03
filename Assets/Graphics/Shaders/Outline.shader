// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColour("Outline Colour", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // Tags {
            //     "LightMode" = "SRPDefaultUnlit"
            // }
            // ZWrite Off
            ZTest Less
            // Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            float4 _OutlineColour;
            float _OutlineWidth;

            float3 Rejection(float3 a, float3 b)
            {
                return a - (dot(a, b) / dot(b, b) * b);
            }

            v2f vert(appdata v)
            {
                v2f o;
                float3 forward = mul(unity_WorldToObject, WorldSpaceViewDir(v.vertex));
                float3 vertexPos = v.vertex + v.normal * _OutlineWidth;
                vertexPos += Rejection(v.vertex, v.normal) * _OutlineWidth * 2;
                vertexPos -= forward;
                o.vertex = UnityObjectToClipPos(vertexPos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColour;
            }
            ENDCG
        }
    }
}
