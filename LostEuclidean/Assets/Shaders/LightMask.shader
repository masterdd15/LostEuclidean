Shader "Custom/LightMask"
{
    Properties
    {
        [IntRange] _StencilValue ("Stencil Value", Range(0, 255)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }
        
        Pass
        {
            Blend Zero One
            ZWrite Off

            Stencil
            {
                Ref[_StencilValue]
                Comp Always
                Pass Replace
                Fail Keep
                ZFail Keep
            }
        }
    }

}
