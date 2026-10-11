using UnityEngine;

[CreateAssetMenu(fileName = "NovaMelhoriaRH", menuName = "Moriah/RH/Dados da Melhoria RH")]
public class DadosMelhoriaRH : ScriptableObject
{
    public enum TipoBonusRH
    {
        MitigacaoEngenhariaSocial,
        EficienciaProdutiva,
        ResistenciaDDoS
    }

    [Header("Informações Básicas")]
    public string nomeMelhoria = "Simular Phishing";
    [TextArea] public string descricao;

    [Header("Economia")]
    public float preco = 1200f;
    public float producaoPorSegundo = 30f;

    [Header("Bônus e Mecânicas de RH")]
    public TipoBonusRH tipoBonus;
    [Tooltip("Valor de Bônus")]
    public float valorBonus = 0.20f;
}
