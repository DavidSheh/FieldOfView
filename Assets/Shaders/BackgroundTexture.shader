Shader "Mask/BackgroundTexture" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    Category {

    	ColorMask RGBA
        ZWRITE on
       
        Tags
        {
            "Queue" = "Transparent"
        }
       
        Stencil
        {
            Ref 1
            Comp equal
        }

       	SubShader {
            Pass {
               SetTexture [_MainTex] {
                    constantColor [_Color]
                    Combine texture * constant, texture * constant 
                 }
            }
        } 
    }
}