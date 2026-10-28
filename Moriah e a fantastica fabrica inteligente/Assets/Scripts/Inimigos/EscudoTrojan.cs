using UnityEngine;

public class EscudoTrojan : MonoBehaviour
{
    [Header("Escudo")]
    public int vidaMaxima = 5;

    [Header("Identificação dos tiros")]
    public string nomeTiroJogador = "Tiro"; // Deixa "Tiro" por enquanto, ainda não temos um nome do prefab do tiro do jogador.
    public string nomeTiroIgnorado = "TiroInimigo"; // Aí sim, o nome tá certo. Os tiros dos inimigos não vão interferir no escudo do trojan.

    private int vidaAtual;
    private SpriteRenderer sr;
    private Collider2D col;
    private Color corOriginal;

    public bool Ativo => vidaAtual > 0;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        vidaAtual = vidaMaxima;

        if (sr != null) corOriginal = sr.color;
    }

    public void ReceberDano(int dano = 1)
    {
        if(!Ativo) return;

        vidaAtual -= dano;
        AtualizarVisual();

        if (!Ativo)
        {
            // Desativa o escudo
            if (sr != null) sr.enabled = false;
            if (col != null) col.enabled = false;
        }
    }

    void AtualizarVisual()
    {
        if(sr == null) return;

        float proporcao = Mathf.Clamp01((float)vidaAtual / vidaMaxima);
        Color c = corOriginal;
        c.a = corOriginal.a * Mathf.Lerp(0.3f, 1f, proporcao);
        sr.color = c;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Ativo) return;

        string nome = collision.name;

        if(nome.StartsWith(nomeTiroIgnorado)) return;
        if(!nome.StartsWith(nomeTiroJogador)) return;

        Destroy(collision.gameObject);
        ReceberDano();
    }
}
