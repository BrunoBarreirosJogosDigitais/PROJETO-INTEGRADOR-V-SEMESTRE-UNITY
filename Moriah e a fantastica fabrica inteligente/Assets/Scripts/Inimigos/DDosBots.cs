using System;
using System.Collections;
using UnityEngine;

public class DDosBots : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public float speed = 1.5f;
    public float velocidadeOriginal;
    public float vidaMaxima = 100f;
    private float vidaAtual;
    public float distanciaParaParar = 5f; // Mantém a distância de parada original para atirar de longe
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
    private bool estaLentificado = false;

    void Start()
    {
        vidaAtual = vidaMaxima;
        velocidadeOriginal = speed;

        if (!gameObject.CompareTag("Inimigos"))
        {
            gameObject.tag = "Inimigos";
        }

        AtualizarAlvoPrincipal();

        // Ignora colisões físicas com os slots de defesa do mapa
        IgnorarColisoesComSlots();

        StartCoroutine(LoopDeRajadas());
    }

    void Update()
    {
        if (alvo == null)
        {
            AtualizarAlvoPrincipal();
            if (alvo == null) return;
        }

        Vector3 direcao = alvo.transform.position - transform.position;
        direcao.z = 0f;

        // Respeita a distância de parada para posicionar o bot antes de abrir fogo
        if (direcao.magnitude > distanciaParaParar)
        {
            transform.Translate(direcao.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    void AtualizarAlvoPrincipal()
    {
        GameObject placaObj = GameObject.Find("PlacaMae");
        if (placaObj != null)
        {
            alvo = placaObj;
        }
    }

    public void DefinirAlvo(Transform novoAlvo)
    {
        if (novoAlvo != null)
        {
            alvo = novoAlvo.gameObject;
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

    // Método obrigatório para aceitar o efeito de lentidão do Honeypot
    public void AplicarLentidao(float fatorLentidao, float duracao)
    {
        if (!estaLentificado)
        {
            StartCoroutine(RotinaLentidao(fatorLentidao, duracao));
        }
    }

    private IEnumerator RotinaLentidao(float fator, float duracao)
    {
        estaLentificado = true;
        speed = velocidadeOriginal * (1f - fator);
        Debug.Log($"[DDoSBots] {gameObject.name} foi afetado por lentidão! Velocidade reduzida para: {speed}");

        yield return new WaitForSeconds(duracao);

        speed = velocidadeOriginal;
        estaLentificado = false;
        Debug.Log($"[DDoSBots] {gameObject.name} recuperou a velocidade normal");
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
            if (alvo != null)
            {
                float distanciaAoAlvo = Vector3.Distance(transform.position, alvo.transform.position);

                if (distanciaAoAlvo <= alcanceDeDisparo)
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
            tiroScript.danoDoTiro = 25; // Alinhado com o SDD para DDoS Bots
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("PlacaMae"))
        {
            Destroy(gameObject);
        }
    }
}