using UnityEngine;

public class ProjetilAntivirus : MonoBehaviour
{
    public float velocidadeProjetil = 8f;
    public float tempoDeVida = 5f; // Destrói o projétil se passar muito tempo sem acertar nada
    private Transform alvo;
    private float dano;

    [Header("Filtros de Colisão")]
    public string nomeTagSlotDefesa = "SlotDefesa";
    public string tagCartaDefensiva = "CartaDefensiva";

    public void ConfigurarAlvo(Transform novoAlvo, float danoRecebido)
    {
        alvo = novoAlvo;
        dano = danoRecebido;

        // Destrói o projétil após um tempo limite para evitar vazamento de memória
        Destroy(gameObject, tempoDeVida);
    }

    void Start()
    {
        // Garante que o projétil ignore fisicamente os slots de defesa e outras cartas ao ser disparado
        IgnorarColisoes();
    }

    void Update()
    {
        if (alvo == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, alvo.position, velocidadeProjetil * Time.deltaTime);
    }

    void IgnorarColisoes()
    {
        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor == null) return;

        // Ignora colisões com os slots de defesa
        GameObject[] slots = GameObject.FindGameObjectsWithTag(nomeTagSlotDefesa);
        foreach (GameObject slot in slots)
        {
            Collider2D colisorSlot = slot.GetComponent<Collider2D>();
            if (colisorSlot != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorSlot, true);
            }
        }

        // Ignora colisões com outras cartas defensivas no caminho
        GameObject[] cartas = GameObject.FindGameObjectsWithTag(tagCartaDefensiva);
        foreach (GameObject carta in cartas)
        {
            // Não ignora a si mesmo caso esteja no mesmo objeto (por segurança)
            if (carta == gameObject) continue;

            Collider2D colisorCarta = carta.GetComponent<Collider2D>();
            if (colisorCarta != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorCarta, true);
            }
        }
    }

    // Usando OnCollisionEnter2D para colisão física normal
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            collision.gameObject.SendMessage("ReceberDano", dano, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}