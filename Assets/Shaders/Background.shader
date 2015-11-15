Shader "Mask/Background"
{
    Properties
    {
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0) 
    }
    SubShader
    {
        Pass
        {
            ColorMask RGBA
            ZWRITE on
           
            Tags
            {
                "Queue" = "Geometry"
            }
           
            Stencil
            {
                Ref 1
                Comp equal
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            fixed4 _Color;
 
            struct vert_input
            {
                fixed4 vertex : POSITION;
            };
 
            struct frag_input
            {
                fixed4 pos : SV_POSITION;
            };
 
            frag_input vert(vert_input v)
            {
                frag_input vo;
                vo.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return vo;
            }
 
            fixed4 frag(frag_input fi) : COLOR
            {
                return _Color;
            }
            ENDCG
        }
    }
}