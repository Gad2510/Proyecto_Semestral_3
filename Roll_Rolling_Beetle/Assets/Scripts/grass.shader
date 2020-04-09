Shader "Unlit/grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WindIntensity("Wind", Range(5.0, 20.0))=10.0
        _MoveIntensity("Movement", Range(1.0,100.0))=10.0
        _RangeMove("RangeEffect", float)=7.0
        _PlayerPosition("Player",vector)=(0,0,0,0)
    }
    SubShader
    {
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 
        Cull Off
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
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
                float3 worldPos:TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _PlayerPosition;
            float _WindIntensity;
            float _MoveIntensity;
            float _RangeMove;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos=mul(unity_ObjectToWorld, v.vertex).xyz;

                if(distance(_PlayerPosition,o.worldPos)<_RangeMove){
                    fixed3 moved=normalize(o.worldPos - _PlayerPosition.xyz).xyz *_MoveIntensity;
                    moved=mul(unity_WorldToObject,moved);
                    o.vertex.y+=moved.y;
				}

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float intensidad= lerp(1000,_WindIntensity,saturate(distance(_PlayerPosition,i.worldPos)));
                fixed2 uv=fixed2(i.uv.x+((sin(_Time.w)/intensidad)*i.uv.y),i.uv.y);
                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
