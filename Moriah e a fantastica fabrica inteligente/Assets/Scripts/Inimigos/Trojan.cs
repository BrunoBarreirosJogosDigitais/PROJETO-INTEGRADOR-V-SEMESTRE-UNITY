using System;
using System.Collections;
using UnityEngine;

public class Trojan : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 1.5f;
    public float velocidadeOriginal;
    public float suavizacao = 5f;

    [Header("Atributos de Vida e Dano")]
    public float vidaMaxima = 180f;
    private float vidaAtual;
    public int danoAoNucleo = 18;

    [Header("Escudo")]
    public EscudoTrojan escudo;

    [Header("Filtros de Colisão")]
    public string nomeTagSlotDefesa = "SlotDefesa";

    [Header("Desvio de obstáculos")]
    public GameObject[] prefabsParaEvitar;
    public float raioDeDeteccao = 2.5f;
    public float forcaDoDesvio = 2f;

    [Header("Teste")]
    public bool simularDanoNoEscudo;
    public float intervaloDanoSimulado = 1f;

    private GameObject alvo;
    private Vector2 direcaoAtual;
    private float timerDano;
    private bool estaLentificado = false;

    void Start()
    {
        vidaAtual = vidaMaxima;
        velocidadeOriginal = speed;

        if (!gameObject.CompareTag("Inimigos"))
        {
            gameObject.tag = "Inimigos";
        }

        // FORÇA A LIMPEZA E BUSCA AUTOMÁTICA AO NASCER
        alvo = null;
        AtualizarAlvoPrincipal();

        if (escudo == null)
            escudo = GetComponentInChildren<EscudoTrojan>();

        if (alvo != null)
            direcaoAtual = ((Vector2)alvo.transform.position - (Vector2)transform.position).normalized;

        IgnorarColisoesComSlots();
    }

    void Update()
    {
        TestarDanoNoEscudo();

        if (alvo == null)
        {
            AtualizarAlvoPrincipal();
            if (alvo == null)
            {
                return;
            }
        }

        // Trava o eixo Z para evitar desvio em profundidade
        Vector3 posicaoAlvo = alvo.transform.position;
        posicaoAlvo.z = transform.position.z;

        Vector3 direcao = (posicaoAlvo - transform.position).normalized;
        transform.position += direcao * speed * Time.deltaTime;

        // VERIFICAÇÃO POR DISTÂNCIA IGNORANDO O Z
        float distanciaAoNucleo = Vector3.Distance(
            new Vector3(transform.position.x, transform.position.y, 0),
            new Vector3(alvo.transform.position.x, alvo.transform.position.y, 0)
        );

        if (distanciaAoNucleo <= 0.8f)
        {
            if (GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoAoNucleo);
            }
            Destroy(gameObject);
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

    Vector2 CalcularDirecao()
    {
        Vector2 pos = transform.position;
        Vector2 paraAlvo = ((Vector2)alvo.transform.position - pos).normalized;
        Vector2 desvio = Vector2.zero;

        Collider2D[] proximos = Physics2D.OverlapCircleAll(pos, raioDeDeteccao);

        foreach (Collider2D c in proximos)
        {
            if (c.transform.IsChildOf(transform)) continue;
            if (!EhObstaculo(c.gameObject)) continue;

            Vector2 paraObstaculo = (Vector2)c.transform.position - pos;
            float distancia = paraObstaculo.magnitude;
            if (distancia < 0.001f) continue;

            if (Vector2.Dot(paraObstaculo.normalized, paraAlvo) < 0f) continue;

            float peso = 1f - Mathf.Clamp01(distancia / raioDeDeteccao);

            Vector2 afastar = -paraObstaculo.normalized;

            float lado = paraAlvo.x * paraObstaculo.y - paraAlvo.y * paraObstaculo.x;
            Vector2 perpDireita = new Vector2(paraAlvo.y, -paraAlvo.x);
            Vector2 lateral = lado > 0f ? perpDireita : -perpDireita;

            desvio += (afastar * 0.5f + lateral) * peso;
        }

        Vector2 resultado = paraAlvo + desvio * forcaDoDesvio;
        return resultado.sqrMagnitude > 0.0001f ? resultado.normalized : paraAlvo;
    }

    bool EhObstaculo(GameObject go)
    {
        foreach (GameObject prefab in prefabsParaEvitar)
        {
            if (prefab != null && go.name.StartsWith(prefab.name))
                return true;
        }
        return false;
    }

    public void ReceberDano(float quantidadeDano)
    {
        if (escudo != null && escudo.Ativo)
        {
            escudo.ReceberDano();
            Debug.Log("[Trojan] O escudo criptográfico interceptou o dano!");
            return;
        }

        vidaAtual -= quantidadeDano;
        Debug.Log($"[Trojan] Sofreu {quantidadeDano} de dano. Vida restante: {vidaAtual}/{vidaMaxima}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

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
        Debug.Log($"[Trojan] {gameObject.name} foi afetado por lentidão! Velocidade reduzida para: {speed}");

        yield return new WaitForSeconds(duracao);

        speed = velocidadeOriginal;
        estaLentificado = false;
        Debug.Log($"[Trojan] {gameObject.name} recuperou a velocidade normal");
    }

    private void Morrer()
    {
        Debug.Log("[Trojan] Inimigo Trojan foi destruído!");
        Destroy(gameObject);
    }

    void TestarDanoNoEscudo()
    {
        if (!simularDanoNoEscudo || escudo == null || !escudo.Ativo) return;

        timerDano += Time.deltaTime;
        if (timerDano >= intervaloDanoSimulado)
        {
            timerDano = 0f;
            escudo.ReceberDano();
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
            if (GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoAoNucleo);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CartaDefensiva"))
        {
            float danoContinuo = danoAoNucleo * Time.deltaTime;
            collision.gameObject.SendMessage("ReceberDano", danoContinuo, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeDeteccao);
    }
}