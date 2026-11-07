Shader "Hidden/Daltonismo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4x4 _MatrizSimulacao;
            float4x4 _MatrizCorrecao;
            float _Intensidade;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float4 original = tex2D(_MainTex, i.uv);

                float3 simulado = mul((float3x3)_MatrizSimulacao, original.rgb);

                float3 erro = original.rgb - simulado;

                float3 corrigido = original.rgb + mul((float3x3)_MatrizCorrecao, erro);

                return float4(saturate(lerp(original.rgb, corrigido, _Intensidade)), original.a);
            }
            ENDCG
        }
    }
}