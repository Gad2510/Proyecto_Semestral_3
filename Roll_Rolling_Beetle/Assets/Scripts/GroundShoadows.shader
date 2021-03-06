﻿Shader "Unlit/GroundShoadows"
{
    Properties
    {
        _Color("Color",Color)=(0,0,0,0)
        _ColorWarnnig("Color Advetencia",Color)=(1,0,0,0)
        _ShadowIntensity("ShadowIntensity", Range(0,1))=0.5
        _MainTex ("Texture", 2D) = "white" {}
        _SecTex ("Ground", 2D)="white"{}
        _Mask("Mascara", 2D)="black"{}
        _Water("Agua",2D)="white" {}
        _ShadowsMask("Mask", 2D)="white" {}
        
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
            sampler2D _SecTex;
            sampler2D _Mask;
            sampler2D _ShadowsMask;
            sampler2D _Water;
            float _ShadowIntensity;
            float4 _MainTex_ST;
            float4 _Cord;
            float4 _Color;
            float4 _ColorWarnnig;

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
                fixed4 mask=tex2D(_ShadowsMask,i.uv);

                
                // sample the texture
                fixed4 tex;
                if(mask.b>0.3){
                    tex=tex2D(_Water, i.uv*20);
				} 
                else{
                    fixed4 grass=tex2D(_MainTex, i.uv*40);
                    fixed4 ground=tex2D(_SecTex, i.uv*40);
                    fixed4 texmask=tex2D(_Mask, i.uv);
                    tex= lerp(grass,ground,texmask.r);
                    
				}
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, tex);
                
                fixed4 birdCol=_ColorWarnnig;
                birdCol.g=abs(sin(_Time.w));

                float4 col=lerp(_Color,_Color*_ShadowIntensity,mask.g);
                col=lerp(col,birdCol,mask.r);
                return tex*col;
            }
            ENDCG
        }
    }
}
