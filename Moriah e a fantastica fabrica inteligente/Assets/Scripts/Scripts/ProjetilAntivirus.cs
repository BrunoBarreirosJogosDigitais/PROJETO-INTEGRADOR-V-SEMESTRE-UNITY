using UnityEngine;

public class ProjetilAntivirus : MonoBehaviour
{
    public float velocidadeProjetil = 8f;
    public float tempoDeVida = 5f; // Destr�i o proj�til se passar muito tempo sem acertar nada
    private Transform alvo;
    private float dano;

    [Header("Filtros de Colis�o")]
    public string nomeTagSlotDefesa = "SlotDefesa";
    public string tagCartaDefensiva = "CartaDefensiva";
    public string tagPlacaMae = "PlacaMae";

    public void ConfigurarAlvo(Transform novoAlvo, float danoRecebido)
    {
        alvo = novoAlvo;
        dano = danoRecebido;

        // Destr�i o proj�til ap�s um tempo limite para evitar vazamento de mem�ria
        Destroy(gameObject, tempoDeVida);
    }

    void Start()
    {
        // Garante que o proj�til ignore fisicamente os slots de defesa e outras cartas ao ser disparado
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

        // Ignora colis�es com os slots de defesa
        GameObject[] slots = GameObject.FindGameObjectsWithTag(nomeTagSlotDefesa);
        foreach (GameObject slot in slots)
        {
            Collider2D colisorSlot = slot.GetComponent<Collider2D>();
            if (colisorSlot != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorSlot, true);
            }
        }

        // Ignora colis�es com outras cartas defensivas no caminho
        GameObject[] cartas = GameObject.FindGameObjectsWithTag(tagCartaDefensiva);
        foreach (GameObject carta in cartas)
        {
            // N�o ignora a si mesmo caso esteja no mesmo objeto (por seguran�a)
            if (carta == gameObject) continue;

            Collider2D colisorCarta = carta.GetComponent<Collider2D>();
            if (colisorCarta != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorCarta, true);
            }
        }

        GameObject[] placas = GameObject.FindGameObjectsWithTag(tagPlacaMae);
        foreach (GameObject placa in placas)
        {
            Collider2D colisorPlaca = placa.GetComponent<Collider2D>();
            if (colisorPlaca != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorPlaca, true);
            }
        }
    }

    // Usando OnCollisionEnter2D para colis�o f�sica normal
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            collision.gameObject.SendMessage("ReceberDano", dano, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}