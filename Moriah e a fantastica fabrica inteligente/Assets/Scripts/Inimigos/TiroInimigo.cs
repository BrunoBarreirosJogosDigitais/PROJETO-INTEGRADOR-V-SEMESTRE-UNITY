using UnityEngine;

public class TiroInimigo : MonoBehaviour
{
    public float velocidade = 6f;
    public float tempoDeVida = 5f;
    public int danoDoTiro = 25; // Alinhado com a tabela do SDD para DDoS Bots[cite: 1]

    [Header("Filtros de Colisão")]
    public string nomeTagSlotDefesa = "SlotDefesa";
    public string nomeTagInimigos = "Inimigos";

    private Vector3 direcao;
    private Transform alvoPlacaMae;

    public void Iniciar(Vector3 dir)
    {
        direcao = dir;

        // Gira o sprite para apontar na direção do movimento
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo);

        // Tenta achar a placa-mãe para validação por distância em segundo plano
        GameObject placaObj = GameObject.Find("PlacaMae");
        if (placaObj != null) alvoPlacaMae = placaObj.transform;

        Destroy(gameObject, tempoDeVida);
    }

    void Start()
    {
        IgnorarColisoes();
    }

    void Update()
    {
        transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);

        // VERIFICAÇÃO POR DISTÂNCIA PARA O TIRO DAR DANO NO NÚCLEO MESMO EM SEGUNDO PLANO
        if (alvoPlacaMae != null)
        {
            float distancia = Vector3.Distance(transform.position, alvoPlacaMae.position);
            if (distancia <= 0.6f)
            {
                if (GerenciadorJogo.Instancia != null)
                {
                    GerenciadorJogo.Instancia.ReceberDanoPlaca(danoDoTiro);
                }
                Destroy(gameObject);
            }
        }
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

        // Ignora colisões com outros inimigos
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag(nomeTagInimigos);
        foreach (GameObject inimigo in inimigos)
        {
            Collider2D colisorInimigo = inimigo.GetComponent<Collider2D>();
            if (colisorInimigo != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorInimigo, true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("PlacaMae"))
        {
            if (GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoDoTiro);
            }

            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("CartaDefensiva"))
        {
            collision.gameObject.SendMessage("ReceberDano", (float)danoDoTiro, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}