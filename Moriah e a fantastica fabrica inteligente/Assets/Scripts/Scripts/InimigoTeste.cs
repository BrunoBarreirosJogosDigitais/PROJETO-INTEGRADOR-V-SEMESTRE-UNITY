using UnityEngine;

public class InimigoTeste : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public string nomeInimigo = "Worm de Teste";
    public float vidaMaxima = 80f;
    public float vidaAtual;
    public float velocidade = 3f;
    public float danoNoFirewall = 20f; // Dano que ele causa na torre

    private Transform alvoTorre;

    private void Start()
    {
        vidaAtual = vidaMaxima;

        if (!gameObject.CompareTag("Inimigos"))
        {
            gameObject.tag = "Inimigos";
        }

        // Ignora automaticamente a colisão física com todos os objetos que têm a tag "SlotDefesa" na cena
        GameObject[] slots = GameObject.FindGameObjectsWithTag("SlotDefesa");
        Collider2D meuCollider = GetComponent<Collider2D>();

        foreach (GameObject slot in slots)
        {
            Collider2D colliderSlot = slot.GetComponent<Collider2D>();
            if (meuCollider != null && colliderSlot != null)
            {
                Physics2D.IgnoreCollision(meuCollider, colliderSlot, true);
            }
        }
    }

    private void Update()
    {
        // Se não tiver um alvo (torre) na cena, tenta encontrar o Firewall mais próximo automaticamente
        if (alvoTorre == null)
        {
            EncontrarTorreMaisProxima();
            return; // Fica parado se não achar nenhuma torre
        }

        // Move-se em direção à torre encontrada
        transform.position = Vector2.MoveTowards(transform.position, alvoTorre.position, velocidade * Time.deltaTime);
    }

    private void EncontrarTorreMaisProxima()
    {
        // Procura automaticamente por objetos que tenham o script Firewall na cena
        Firewall[] torresNaCena = FindObjectsByType<Firewall>(FindObjectsSortMode.None);

        if (torresNaCena.Length > 0)
        {
            float menorDistancia = Mathf.Infinity;
            Transform maisProxima = null;

            foreach (var torre in torresNaCena)
            {
                float distancia = Vector2.Distance(transform.position, torre.transform.position);
                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    maisProxima = torre.transform;
                }
            }

            alvoTorre = maisProxima;
        }
    }

    // Se estiver usando colisão física normal (sem Trigger)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SlotDefesa") || collision.gameObject.GetComponent<Firewall>() != null)
        {
            Firewall firewall = collision.gameObject.GetComponent<Firewall>();
            if (firewall != null)
            {
                // Causa dano contínuo na torre ao encostar nela
                firewall.ReceberDano(danoNoFirewall * Time.deltaTime);
            }
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMaxima);

        Debug.Log($"[{nomeInimigo}] Recebeu dano do Firewall! HP Atual: {vidaAtual}/{vidaMaxima}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log($"[{nomeInimigo}] Destruído pelas defesas!");
        Destroy(gameObject);
    }
}