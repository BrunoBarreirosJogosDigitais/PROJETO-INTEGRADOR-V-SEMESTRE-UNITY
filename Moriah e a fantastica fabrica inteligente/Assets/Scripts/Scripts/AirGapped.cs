using UnityEngine;

public class AirGapped : MonoBehaviour
{
    [Header("Objeto Scriptável da Carta")]
    public CartaDefensiva dadosCartas;

    [Header("Atributos em Tempo de Execução")]
    public float vidaAtual;

    void Start()
    {
        if(dadosCartas != null)
        {
            vidaAtual = dadosCartas.vidaBloqueio;
        }
        else
        {
            Debug.LogWarning("[Air-Gapped] Nenhuma CartaDefensiva foi associada no Inspector!");
            vidaAtual = 250f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            InimigoTeste inimigo = collision.gameObject.GetComponent<InimigoTeste>();
            if(inimigo != null)
            {
               
            }
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        float vidaMax = (dadosCartas != null) ? dadosCartas.vidaBloqueio : 250f;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMax);

        Debug.Log($"[AIR-Gapped] Barreira de Isolamento sob ataque! HP Restante: {vidaAtual/vidaMax}");

        if(vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log("[AIR-Gapped] Barreira Destruida pelos invasores!");
        Destroy(gameObject);
    }

}
