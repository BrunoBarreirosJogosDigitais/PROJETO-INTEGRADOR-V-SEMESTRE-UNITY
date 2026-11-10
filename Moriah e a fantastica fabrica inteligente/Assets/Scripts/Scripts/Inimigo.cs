using System;
using System.Collections;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public float speed = 1.5f; // Valor padrão para evitar velocidade zerada
    public float velocidadeOriginal;
    public float vidaMaxima = 50f;
    private float vidaAtual;
    public int danoAoNucleo = 15;

    [Header("Filtros de Colisão")]
    public string nomeTagCirculoInvocacao = "SlotDefesa"; // Nome da Tag ou parte do nome do objeto a ignorar

    public GameObject alvo;
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
        // Isso impede que o prefab guarde uma referência antiga ou quebrada
        alvo = null;
        AtualizarAlvoPrincipal();

        // Configuração automática para ignorar colisões físicas com os círculos de invocação da cena
        IgnorarColisoesComCirculos();
    }

    void Update()
    {
        if (alvo == null)
        {
            AtualizarAlvoPrincipal();
            if (alvo == null)
            {
                return;
            }
        }

        // Trava o eixo Z para evitar que o inimigo desvie em profundidade (ambiente 2D/3D misto)
        Vector3 posicaoAlvo = alvo.transform.position;
        posicaoAlvo.z = transform.position.z;

        Vector3 direcao = (posicaoAlvo - transform.position).normalized;
        transform.position += direcao * speed * Time.deltaTime;

        // VERIFICAÇÃO POR DISTÂNCIA PARA DAR DANO (ignorando a profundidade Z)
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

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        Debug.Log($"[Inimigo] Worm Sofreu {quantidadeDano} de dano. Vida restante: {vidaAtual / vidaMaxima}");

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
        Debug.Log($"[Inimigo] {gameObject.name} foi afetado por lentidão! Velocidade reduzida para: {speed}");

        yield return new WaitForSeconds(duracao);

        speed = velocidadeOriginal;
        estaLentificado = false;
        Debug.Log($"[Inimigo] {gameObject.name} recuperou a velocidade normal");
    }

    private void Morrer()
    {
        Debug.Log("[Inimigo] Worm foi destruido");
        Destroy(gameObject);
    }

    // Método para ignorar colisão física com os círculos de invocação / áreas de posicionamento
    void IgnorarColisoesComCirculos()
    {
        Collider2D meuColisor = GetComponent<Collider2D>();
        if (meuColisor == null) return;

        GameObject[] circulos = GameObject.FindGameObjectsWithTag(nomeTagCirculoInvocacao);
        foreach (GameObject circulo in circulos)
        {
            Collider2D colisorCirculo = circulo.GetComponent<Collider2D>();
            if (colisorCirculo != null)
            {
                Physics2D.IgnoreCollision(meuColisor, colisorCirculo, true);
            }
        }
    }

    public void DefinirAlvo(Transform novoAlvo)
    {
        if (novoAlvo != null)
        {
            alvo = novoAlvo.gameObject;
        }
    }

    // Impacto único e direto ao atingir o núcleo da Placa-Mãe
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("PlacaMae"))
        {
            if (GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoAoNucleo);
            }
            Destroy(this.gameObject);
        }
    }

    // Dano contínuo por intervalo de tempo enquanto estiver encostado na torre defensiva
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CartaDefensiva"))
        {
            float danoContinuo = danoAoNucleo * Time.deltaTime;
            collision.gameObject.SendMessage("ReceberDano", danoContinuo, SendMessageOptions.DontRequireReceiver);
        }
    }
}