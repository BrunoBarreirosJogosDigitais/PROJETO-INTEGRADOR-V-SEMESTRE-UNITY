using System;
using System.Collections;
using UnityEngine;

public class DDosBots : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public float speed = 1.5f;
    public float vidaMaxima = 100f; 
    private float vidaAtual;
    public float distanciaParaParar = 5f;
    public float alcanceDeDisparo = 6f;

    [Header("Filtros de Colisão")]
    public string nomeTagSlotDefesa = "SlotDefesa";

    [Header("Tiro")]
    public GameObject prefabTiro;
    public Transform pontoDeTiro;
    public int tirosPorRajada = 3;
    public float intervaloEntreTiros = 0.15f;
    public float intervaloEntreRajadas = 2f;

    private GameObject alvo;

    void Start()
    {
        vidaAtual = vidaMaxima;

        if (!gameObject.CompareTag("Inimigos"))
        {
            gameObject.tag = "Inimigos";
        }

        alvo = GameObject.Find("PlacaMae");

        // Ignora colisões físicas com os slots de defesa do mapa
        IgnorarColisoesComSlots();

        StartCoroutine(LoopDeRajadas());
    }

    void Update()
    {
        if (alvo == null) return;

        Vector3 direcao = alvo.transform.position - transform.position;
        direcao.z = 0f;

        if (direcao.magnitude > distanciaParaParar)
        {
            transform.Translate(direcao.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        Debug.Log($"[DDoSBots] Sofreu {quantidadeDano} de dano. Vida Restante: {vidaAtual}/{vidaMaxima}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log("[DDoSBots] Bot DDoS foi destruido");
        Destroy(gameObject);
    }

    IEnumerator LoopDeRajadas()
    {
        yield return new WaitForSeconds(intervaloEntreRajadas);

        while (true)
        {
            if(alvo != null)
            {
                float distanciaAoAlvo = Vector3.Distance(transform.position, alvo.transform.position);

                if(distanciaAoAlvo <= alcanceDeDisparo)
                {
                    for (int i = 0; i < tirosPorRajada; i++)
                    {
                        Atirar();
                        yield return new WaitForSeconds(intervaloEntreTiros);
                    }

                    yield return new WaitForSeconds(intervaloEntreRajadas);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void Atirar()
    {
        if (alvo == null || prefabTiro == null) return;

        Vector3 origem = pontoDeTiro != null ? pontoDeTiro.position : transform.position;
        Vector3 direcao = (alvo.transform.position - origem);
        direcao.z = 0f;
        direcao.Normalize();

        GameObject tiro = Instantiate(prefabTiro, origem, Quaternion.identity);

        TiroInimigo tiroScript = tiro.GetComponent<TiroInimigo>();
        if (tiroScript != null)
        {
            tiroScript.danoDoTiro = 25; // Alinhado com o SDD para DDoS Bots[cite: 1]
        }

        tiroScript?.Iniciar(direcao);
    }

    void IgnorarColisoesComSlots()
    {
        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor == null) return;

        GameObject[] slots = GameObject.FindGameObjectsWithTag(nomeTagSlotDefesa);
        foreach (GameObject slot in slots)
        {
            Collider2D colisorSlot = slot.GetComponent<Collider2D>();
            if (colisorSlot != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorSlot, true);
            }
        }
    }

    // Colisão física normal para interagir com o cenário e a Placa-Mãe
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("PlacaMae"))
        {
            Destroy(gameObject);
        }
    }
}