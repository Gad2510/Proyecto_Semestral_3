Shader "Unlit/GroundShoadows"
{
    Properties
    {
        _Color("Color",Color)=(0,0,0,0)
        _ColorWarnnig("Color Advetencia",Color)=(1,0,0,0)
        _ShadowIntensity("ShadowIntensity", Range(0,1))=0.5
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowsMask("Mask", 2D)="white" {}

        _Cord("Cordenadas",Vector)=(0,0,0,0)
        
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
            sampler2D _ShadowsMask;
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
                fixed2 uv = ((i.uv) + fixed2(_Cord.x,_Cord.y))/_Cord.z;
                fixed maskBird=tex2D(_ShadowsMask,uv).r;

                // sample the texture
                fixed4 tex = tex2D(_MainTex, i.uv*20);
                fixed maskShadows=tex2D(_ShadowsMask,uv).g;
                
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, tex);
                
                fixed4 birdCol=_ColorWarnnig;
                birdCol.g=abs(sin(_Time.w));

                float4 col=lerp(_Color,_Color*_ShadowIntensity,maskShadows);
                
                col=lerp(col,birdCol,maskBird);
                return tex*col;
            }
            ENDCG
        }
    }
}
