using UnityEngine;

//Permite criar instancias deste arquivo diretamente no editor da Unity pelo menu contextual
[CreateAssetMenu(fileName = "NovaMaquina", menuName = "Moriah/Fabrica/Dados da Maquina")]
public class DadosMaquina : ScriptableObject
{
    public string nomeMaquina = "Esteira"; //Nome identificador da máquina no jogo
    [TextArea] public string descricao; //Descrição detalhada exibida na interface de compra

    [Header("Economia")]
    public float preco = 250f; //Custo para construção
    public float producaoPorSegundo = 5f; //Taxa de receita bruta gerada
    public float manutencaoPorSegundo = 2f; //Custo operacional descontado da renda

    [Header("Seguranca")]
    [Range(0, 10)] public float aumentoAmeaca = 0.5f; // O quanto está máquina específica
}
