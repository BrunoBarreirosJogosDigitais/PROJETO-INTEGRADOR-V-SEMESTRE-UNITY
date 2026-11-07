using UnityEngine;

public class AirGapped : MonoBehaviour
{
    [Header("Objeto Scriptável da Carta")]
    public CartaDefensiva dadosCartas;

    [Header("Atributos em Tempo de Execução")]
    public float vidaAtual;

    void Start()
    {
        if (dadosCartas != null)
        {
            vidaAtual = dadosCartas.vidaBloqueio;
        }
        else
        {
            Debug.LogWarning("[Air-Gapped] Nenhuma CartaDefensiva foi associada no Inspector!");
            vidaAtual = 250f; // Valor robusto padrão para a torre tanque
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Apenas valida o contato físico com os inimigos para bloquear a passagem e tankar o dano
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            // O Air-Gapped não retribui dano, ele apenas absorve o impacto ( tanka )
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        float vidaMax = (dadosCartas != null) ? dadosCartas.vidaBloqueio : 250f;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMax);

        Debug.Log($"[AIR-Gapped] Barreira de Isolamento sob ataque! HP Restante: {vidaAtual}/{vidaMax}");

        if (vidaAtual <= 0f)
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