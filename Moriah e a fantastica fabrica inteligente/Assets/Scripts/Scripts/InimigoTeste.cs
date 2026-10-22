using UnityEngine;

public class InimigoTeste : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public string nomeInimigo = "Worm de Teste";
    public float vidaMaxima = 80f;
    public float vidaAtual;
    public float velocidade = 3f;
    public float danoDeContato = 20f; // Dano aplicado no AirGapped por segundo

    private Transform alvoTorre;

    private void Start()
    {
        vidaAtual = vidaMaxima;

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
        // Procura a barreira AirGapped se ainda não tiver um alvo
        if (alvoTorre == null)
        {
            EncontrarAirGappedMaisProximo();
            return;
        }

        // Caminha em direção ao AirGapped
        transform.position = Vector2.MoveTowards(transform.position, alvoTorre.position, velocidade * Time.deltaTime);
    }

    private void EncontrarAirGappedMaisProximo()
    {
        AirGapped[] barreiras = FindObjectsByType<AirGapped>(FindObjectsSortMode.None);

        if (barreiras.Length > 0)
        {
            float menorDistancia = Mathf.Infinity;
            Transform maisProxima = null;

            foreach (var barreira in barreiras)
            {
                float distancia = Vector2.Distance(transform.position, barreira.transform.position);
                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    maisProxima = barreira.transform;
                }
            }

            alvoTorre = maisProxima;
        }
    }

    // Ao encostar no AirGapped, causa dano contínuo
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AirGapped>() != null)
        {
            collision.gameObject.SendMessage("ReceberDano", danoDeContato * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMaxima);

        Debug.Log($"[{nomeInimigo}] HP Atual: {vidaAtual}/{vidaMaxima}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log($"[{nomeInimigo}] Destruído!");
        Destroy(gameObject);
    }
}