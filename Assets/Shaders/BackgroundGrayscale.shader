Shader "Mask/BackgroundGrayScale"
{
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Grayscale ("Grayscale", Float) = 0
    }
    SubShader {

        Pass
        {  
            Tags
            {
                "Queue" = "Geometry"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
             
            #include "UnityCG.cginc"
             
            uniform float _Grayscale;

            sampler2D _MainTex;
 
            struct v2f {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
            
            float4 _MainTex_ST;
             
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                return o;
            }
             
            half4 frag (v2f i) : COLOR
            {
                half4 texcol = tex2D (_MainTex, i.uv);
                if (_Grayscale == 1)
                    texcol.rgb = dot(texcol.rgb, float3(0.3, 0.59, 0.11));
                return texcol;
            }
            ENDCG
        }
    }
}