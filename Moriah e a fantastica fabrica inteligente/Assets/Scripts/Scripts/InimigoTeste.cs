using System.Collections;
using UnityEngine;

public class InimigoTeste : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public string nomeInimigo = "Worm de Teste";
    public float vidaMaxima = 80f;
    public float vidaAtual;
    public float velocidade = 3f;
    public float danoDeContato = 20f;

    private Transform alvoTorre;
    private float velocidadeOriginal;
    private bool estaLento = false;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        velocidadeOriginal = velocidade; // Salva a velocidade base configurada no Inspector

        if (!gameObject.CompareTag("Inimigos"))
        {
            gameObject.tag = "Inimigos";
        }

        // Ignora a colisão física com os slots do mapa
        GameObject[] slots = GameObject.FindGameObjectsWithTag("SlotDefesa");
        Collider2D meuCollider = GetComponent<Collider2D>();

        if (meuCollider != null)
        {
            foreach (GameObject slot in slots)
            {
                Collider2D colliderSlot = slot.GetComponent<Collider2D>();
                if (colliderSlot != null)
                {
                    Physics2D.IgnoreCollision(meuCollider, colliderSlot, true);
                }
            }
        }
    }

    private void Update()
    {
        // Se não tiver alvo, busca pelo Honeypot ou AirGapped na cena
        if (alvoTorre == null)
        {
            EncontrarDefesaMaisProxima();
            return;
        }

        // Caminha em direção à defesa usando a velocidade atual (afetada por slow)
        transform.position = Vector2.MoveTowards(transform.position, alvoTorre.position, velocidade * Time.deltaTime);
    }

    private void EncontrarDefesaMaisProxima()
    {
        Honeypot[] honeypots = FindObjectsByType<Honeypot>(FindObjectsSortMode.None);
        AirGapped[] airGappeds = FindObjectsByType<AirGapped>(FindObjectsSortMode.None);

        float menorDistancia = Mathf.Infinity;
        Transform maisProxima = null;

        // Verifica primeiro os Honeypots (dando prioridade de engodo)
        foreach (var hp in honeypots)
        {
            float dist = Vector2.Distance(transform.position, hp.transform.position);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                maisProxima = hp.transform;
            }
        }

        // Se não achou Honeypot, verifica AirGapped
        if (maisProxima == null)
        {
            foreach (var ag in airGappeds)
            {
                float dist = Vector2.Distance(transform.position, ag.transform.position);
                if (dist < menorDistancia)
                {
                    menorDistancia = dist;
                    maisProxima = ag.transform;
                }
            }
        }

        alvoTorre = maisProxima;
    }

    // Aplica dano por contato físico contínuo
    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("ReceberDano", danoDeContato * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
    }

    // Método universal de dano (recebido do Honeypot, Antivírus ou Firewall)
    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMaxima);

        Debug.Log($"[{nomeInimigo}] Recebeu dano! HP: {vidaAtual}/{vidaMaxima}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    public void AplicarLentidao(float fator, float duracao)
    {
        if (!estaLento)
        {
            StartCoroutine(RotinaLentidao(fator, duracao));
        }
    }

    private IEnumerator RotinaLentidao(float fator, float duracao)
    {
        estaLento = true;
        velocidadeOriginal = velocidade;
        velocidade *= fator; // Reduz a velocidade (ex: velocidade * 0.5f)

        Debug.Log($"[{nomeInimigo}] Desacelerado pela explosão da Honeypot! Velocidade atual: {velocidade}");

        yield return new WaitForSeconds(duracao);

        velocidade = velocidadeOriginal; // Restaura a velocidade normal após o tempo
        estaLento = false;
        Debug.Log($"[{nomeInimigo}] Recuperou a velocidade normal: {velocidade}");
    }

    private void Morrer()
    {
        Debug.Log($"[{nomeInimigo}] Destruído!");
        Destroy(gameObject);
    }
}