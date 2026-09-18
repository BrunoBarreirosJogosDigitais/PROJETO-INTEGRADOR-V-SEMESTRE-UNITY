using UnityEngine;

[CreateAssetMenu(fileName = "NovaCarta", menuName = "Moriah/TI/Carta Defensiva")]
public class CartaDefensiva : ScriptableObject
{
    public string nomeCarta = "Firewall";
    [TextArea] public string descricao;

    public int custoServidor = 20;
    public float dano = 0f;
    public int vidaBloqueio = 200;
    public float alcance = 3f;
    public float tempoRecarga = 1f;
}
