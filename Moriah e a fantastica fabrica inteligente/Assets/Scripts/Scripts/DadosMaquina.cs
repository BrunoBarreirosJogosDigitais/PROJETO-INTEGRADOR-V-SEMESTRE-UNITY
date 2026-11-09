using UnityEngine;

[CreateAssetMenu(fileName = "NovaMaquina", menuName = "Moriah/Fabrica/Dados da Maquina")]
public class DadosMaquina : ScriptableObject
{
    public string nomeMaquina = "Esteira";
    [TextArea] public string descricao;

    [Header("Economia")]
    public float preco = 250f;
    public float producaoPorSegundo = 5f;
    public float manutencaoPorSegundo = 2f;

    [Header("Seguranca")]
    [Range(0, 10)] public float aumentoAmeaca = 0.5f;
}
