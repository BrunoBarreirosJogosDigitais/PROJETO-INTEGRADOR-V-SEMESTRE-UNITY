using UnityEngine;

[CreateAssetMenu(fileName = "NovaCarta", menuName = "Moriah/TI/Carta Defensiva")]
//Permite criar novas instâncias de cartas defensivas diretamente na Unity pelo menos de conteexto
public class CartaDefensiva : ScriptableObject
{
    public string nomeCarta = "Firewall";
    [TextArea] public string descricao;// Descrição da lora e macânica exibida no painel
    public Sprite imagemCarta; // Icone visual exibido no deck de cartas

    public int custoServidor = 20;
    public float dano = 0f;
    public int vidaBloqueio = 200;
    public float alcance = 3f;
    public float tempoRecarga = 1f;

    [Header("Prefab de Defesa no Mapa")]
    public GameObject prefabTorreParaInstanciar; // Referência da prefab a ser instanciada
}
