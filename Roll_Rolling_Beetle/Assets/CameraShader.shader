﻿Shader "Unlit/CameraShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SecTex ("Texture", 2D) = "black" {}
        _Alpha("AlphaChange",Range(1.0,0.0))=0.0
        _Mask("Mask", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Mask;
            sampler2D _SecTex;
            float4 _MainTex_ST;
            float _Alpha;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                fixed mask = tex2D(_Mask, i.uv).a;
                UNITY_APPLY_FOG(i.fogCoord, col);

                if(mask<_Alpha){
                    return  tex2D(_MainTex, i.uv);
				}
                else{
                    return tex2D(_SecTex, i.uv);
				}
                // apply fog
                
            }
            ENDCG
        }
    }
}