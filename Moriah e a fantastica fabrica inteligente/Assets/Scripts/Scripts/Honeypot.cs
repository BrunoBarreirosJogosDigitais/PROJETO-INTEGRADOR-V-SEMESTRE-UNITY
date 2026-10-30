using UnityEngine;

public class Honeypot : MonoBehaviour
{
    [Header("Objeto Scriptável da Carta")]
    public CartaDefensiva dadosDaCarta;

    [Header("Atributos em Tempo de Execução")]
    public float vidaAtual;
    public float danoExplosao = 70f;
    public float raioExplosao = 3.5f;
    public float fatorLentisao = 0.5f;
    public float duracaoLentidao = 3f;

    [Header("Comportamento de Isca")]
    public float raioAtracao = 5.0f;

    [Header("Filtros de Colisão")]
    public string nomeTagSlotDefesa = "SlotDefesa";

    private bool jaExplodiu = false;

    void Start()
    {
        if(dadosDaCarta != null)
        {
            vidaAtual = dadosDaCarta.vidaBloqueio;
            danoExplosao = dadosDaCarta.dano;
        }
        else
        {
            Debug.LogWarning("[Honeypot] Nenhuma CartaDefensiva associada no Inspector");
            vidaAtual = 50f;
            danoExplosao = 70f;
        }

        IgnorarColisoesComSlots();
    }

    private void Update()
    {
        if (jaExplodiu) return;

        AtrairInimigosProximos();
    }

    void AtrairInimigosProximos()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, raioAtracao);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Inimigos"))
            {
                // Se for um inimigo corpo a corpo tipo worm ou Ransomwares
                Inimigo inimigoScript = col.GetComponent<Inimigo>();
                if (inimigoScript != null)
                {
                    inimigoScript.DefinirAlvo(transform);
                }

                // Se for o DDoS Bot (inimigo de longo alcance)
                DDosBots ddosScript = col.GetComponent<DDosBots>();
                if (ddosScript != null)
                {
                    ddosScript.DefinirAlvo(transform);
                }

                //Se for o Trojan(inimigo com escudo)
                Trojan trojanScript = col.GetComponent<Trojan>();
                if(trojanScript != null)
                {
                    trojanScript.DefinirAlvo(transform);
                }
            }
        }
    }

    void IgnorarColisoesComSlots()
    {
        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor == null) return;

        GameObject[] slots = GameObject.FindGameObjectsWithTag(nomeTagSlotDefesa);
        foreach (GameObject slot in slots)
        {
            Collider2D colisorSlot = slot.GetComponent<Collider2D>();   
            if(colisorSlot != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorSlot, true);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            float danoContato = 5f * Time.deltaTime;
            collision.gameObject.SendMessage("ReceberDano", danoContato, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        if (jaExplodiu) return;

        vidaAtual -= quantidadeDano;
        float vidaMax = (dadosDaCarta != null) ? dadosDaCarta.vidaBloqueio : 50f;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMax);

        Debug.Log($"[Honeypot] Armadilha atacada! HP Restante: {vidaAtual}/{vidaMax}");

        if(vidaAtual <= 0f)
        {
            ExplodirEAplicarSlow();
        }
    }

    private void ExplodirEAplicarSlow()
    {
        jaExplodiu = true;
        Debug.Log("[Honeypot] ARMADILHA DETONADA! Aplicando dano em área e lentidão!");

        Collider2D[] alvosAtingidos = Physics2D.OverlapCircleAll(transform.position, raioExplosao);

        foreach (Collider2D col in alvosAtingidos)
        {
            if (col.CompareTag("Inimigos"))
            {
                // Aplica o dano em área da explosão
                col.SendMessage("ReceberDano", danoExplosao, SendMessageOptions.DontRequireReceiver);

                // Aplica a lentidão para inimigos padrão (Worm, etc.)
                Inimigo inimigoScript = col.GetComponent<Inimigo>();
                if (inimigoScript != null)
                {
                    inimigoScript.AplicarLentidao(fatorLentisao, duracaoLentidao);
                }

                // Aplica a lentidão para os DDoS Bots
                DDosBots ddosScript = col.GetComponent<DDosBots>();
                if (ddosScript != null)
                {
                    ddosScript.AplicarLentidao(fatorLentisao, duracaoLentidao);
                }

                // Aplica a lentidão para o Trojan de forma direta e segura
                Trojan trojanScript = col.GetComponent<Trojan>();
                if (trojanScript != null)
                {
                    trojanScript.AplicarLentidao(fatorLentisao, duracaoLentidao);
                }
            }
        }

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioExplosao);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioAtracao);
    }

}
