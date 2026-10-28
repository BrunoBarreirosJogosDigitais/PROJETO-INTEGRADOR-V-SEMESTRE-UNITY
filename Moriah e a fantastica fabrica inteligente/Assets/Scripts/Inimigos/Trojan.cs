using System;
using UnityEngine;

public class Trojan : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 1.5f;
    public float suavizacao = 5f; // quanto maior, mais rápido ele muda de direção.

    [Header("Atributos de Vida e Dano")]
    public float vidaMaxima = 180f;
    private float vidaAtual;
    public int danoAoNucleo = 18;

    [Header("Escudo")]
    public EscudoTrojan escudo;

    [Header("Desvio de obstáculos")]
    public GameObject[] prefabsParaEvitar;
    public float raioDeDeteccao = 2.5f;
    public float forcaDoDesvio = 2f;

    [Header("Teste")]
    public bool simularDanoNoEscudo; // ISSO É SOMENTE PRA TESTAR
    public float intervaloDanoSimulado = 1f;

    private GameObject alvo;
    private Vector2 direcaoAtual;
    private float timerDano;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alvo = GameObject.Find("PlacaMae");

        if(escudo == null)
            escudo = GetComponentInChildren<EscudoTrojan>();
        
        if(alvo != null)
            direcaoAtual = ((Vector2)alvo.transform.position - (Vector2)transform.position).normalized; 
    }

    // Update is called once per frame
    void Update()
    {
        TestarDanoNoEscudo();

        if (alvo == null) return;

        Vector2 desejada = CalcularDirecao();

        float t = Mathf.Clamp01(suavizacao * Time.deltaTime);
        direcaoAtual = Vector2.Lerp(direcaoAtual, desejada, t);

        if(direcaoAtual.sqrMagnitude < 0.0001f) direcaoAtual = desejada;
        else direcaoAtual.Normalize();

        transform.Translate(direcaoAtual * speed * Time.deltaTime, Space.World);
    }

    Vector2 CalcularDirecao()
    {
        Vector2 pos = transform.position;
        Vector2 paraAlvo = ((Vector2)alvo.transform.position - pos).normalized;
        Vector2 desvio = Vector2.zero;

        Collider2D[] proximos = Physics2D.OverlapCircleAll(pos, raioDeDeteccao);

        foreach(Collider2D c in proximos)
        {
            if(c.transform.IsChildOf(transform)) continue; // ignora a si mesmo e seus filhos, no caso o trojan e o escudo
            if(!EhObstaculo(c.gameObject)) continue; // ignora se não for um obstáculo

            Vector2 paraObstaculo = (Vector2)c.transform.position - pos;
            float distancia = paraObstaculo.magnitude;
            if (distancia < 0.001f) continue;

            // Isso ignora as torres que ficaram para tras
            if (Vector2.Dot(paraObstaculo.normalized, paraAlvo) < 0f) continue;

            float peso = 1f - Mathf.Clamp01(distancia / raioDeDeteccao);

            Vector2 afastar = -paraObstaculo.normalized;

            // Calcula a direção lateral para desviar do obstáculo
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
        foreach(GameObject prefab in prefabsParaEvitar)
        {
            if (prefab != null && go.name.StartsWith(prefab.name))
                return true;
        }
        return false;
    }

    public void ReceberDano(float quantidadeDano)
    {
        if(escudo != null && escudo.Ativo)
        {
            escudo.ReceberDano();
            Debug.Log("[Trojan] O escudo criptográfico interceptou o dano!");
            return;
        }

        vidaAtual -= quantidadeDano;
        Debug.Log($"[Trojan] Sofreu {quantidadeDano} de dano. Vida restante: {vidaAtual}/{vidaMaxima}");

        if(vidaAtual <= 0f )
        {
            Morrer();
        }
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("PlacaMae"))
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeDeteccao);
    }
}
