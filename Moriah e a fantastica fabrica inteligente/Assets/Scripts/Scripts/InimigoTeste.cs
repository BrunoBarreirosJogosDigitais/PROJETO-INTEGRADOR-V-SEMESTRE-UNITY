using UnityEngine;

public class InimigoTeste : MonoBehaviour
{
    [Header("Atributos de Teste (Geral)")]
    public string nomeInimigo = "Worm de Teste";
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public float velocidade = 2.5f;
    public float danoDeContato = 20f;

    private Transform alvoTorre;
    private Rigidbody2D rb;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();

        // Garante a tag correta
        if (!gameObject.CompareTag("Inimigos"))
        {
            gameObject.tag = "Inimigos";
        }

        // Ignora colisão física com os slots do mapa
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
        // Se não tiver alvo, busca a torre/defesa mais próxima na cena
        if (alvoTorre == null)
        {
            EncontrarTorreMaisProxima();
            return;
        }

        // Caminha em direção à torre encontrada
        Vector3 posicaoAlvo = new Vector3(alvoTorre.position.x, alvoTorre.position.y, transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, posicaoAlvo, velocidade * Time.deltaTime);
    }

    private void EncontrarTorreMaisProxima()
    {
        Antivirus[] antiviruses = FindObjectsByType<Antivirus>(FindObjectsSortMode.None);

        Transform maisProxima = null;
        float menorDistancia = Mathf.Infinity;

        foreach (var torre in antiviruses)
        {
            float distancia = Vector2.Distance(transform.position, torre.transform.position);
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProxima = torre.transform;
            }
        }

        alvoTorre = maisProxima;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.gameObject.SendMessage("ReceberDano", danoDeContato * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMaxima);

        Debug.Log($"[{nomeInimigo}] Sofreu {quantidadeDano} de dano! HP Atual: {vidaAtual}/{vidaMaxima}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log($"[{nomeInimigo}] Neutralizado pelas defesas!");
        Destroy(gameObject);
    }
}