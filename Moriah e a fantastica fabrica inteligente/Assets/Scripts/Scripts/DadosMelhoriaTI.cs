using UnityEngine;

[CreateAssetMenu(fileName = "NovoContructoTI", menuName = "Moriah/AreaTI/Dados do Constructo TI")]
public class DadosMelhoriaTI : ScriptableObject
{
    public enum TipoBonusTI
    {
        BlindagemNucleo, //Edge Server (+50 de blindagem ao núcleo)
        ReducaoDanoRansomware, //Módulo de criptografia(Reduz dano de Ransomware)
        EconomiaCartasTD //Supercomputador Neural(-15% custo de energia das cartas)
    }

    [Header("Informações Básicas")]
    public string nomeConstructo = "Edge Server";
    [TextArea] public string descricao;

    [Header("Economia e Servidores")]
    public float preco = 1500;
    public float producaoPorSegundo = 45f;
    public int geracaoPontosServidor = 5;

    [Header("Bônus e Mecânicas de TI")]
    public TipoBonusTI tipoBonus;
    [Tooltip("Valor do bônus")]
    public float valorBonus = 50f;
}
