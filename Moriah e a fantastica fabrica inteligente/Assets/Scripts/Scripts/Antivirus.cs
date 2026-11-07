using UnityEngine;

public class Antivirus : MonoBehaviour
{
    [Header("Objeto Scriptável da Carta")]
    public CartaDefensiva dadosDaCarta;

    [Header("Configuração do Disparo")]
    public GameObject prefabprojetil;
    public Transform pontoDeDisparo;

    [Header("Atributos em Tempo de Execução")]
    public float danoDisparo;
    public float alcance;
    public float tempoRecarga;
    public float vidaAtual;

    private float cronogramDisparo = 0f;

    void Start()
    {
        if (dadosDaCarta != null)
        {
            danoDisparo = dadosDaCarta.dano;
            alcance = dadosDaCarta.alcance;
            tempoRecarga = dadosDaCarta.tempoRecarga;
            vidaAtual = dadosDaCarta.vidaBloqueio;
        }
        else
        {
            Debug.LogWarning("[Antivírus] Nenhuma CartaDefensiva foi associada no Inspector");
            danoDisparo = 25f;
            alcance = 3f;
            tempoRecarga = 1f;
            vidaAtual = 100f;
        }

        if (pontoDeDisparo == null)
        {
            pontoDeDisparo = transform;
        }
    }

    void Update()
    {
        cronogramDisparo += Time.deltaTime;

        if (cronogramDisparo >= tempoRecarga)
        {
            AtacarInimigoNoAlcance();
            cronogramDisparo = 0f;
        }
    }

    void AtacarInimigoNoAlcance()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigos");
        GameObject inimigoMaisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector2.Distance(transform.position, inimigo.transform.position);
            if (distancia <= alcance && distancia < menorDistancia)
            {
                menorDistancia = distancia;
                inimigoMaisProximo = inimigo;
            }
        }

        if (inimigoMaisProximo != null)
        {
            if (prefabprojetil != null)
            {
                GameObject tiroInstanciado = Instantiate(prefabprojetil, pontoDeDisparo.position, Quaternion.identity);

                ProjetilAntivirus projScript = tiroInstanciado.GetComponent<ProjetilAntivirus>();
                if (projScript != null)
                {
                    projScript.ConfigurarAlvo(inimigoMaisProximo.transform, danoDisparo);
                }
            }
            else
            {
                Debug.Log($"[Antivirus] Disparando contra {inimigoMaisProximo.name} causando {danoDisparo} de dano!");
                inimigoMaisProximo.SendMessage("ReceberDano", danoDisparo, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            // Retribui um dano contínuo de contato ao inimigo enquanto estiverem encostados
            collision.gameObject.SendMessage("ReceberDano", danoDisparo * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
        }
    }

    // Método chamado para aplicar dano e diminuir a vida da torre
    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, (dadosDaCarta != null ? dadosDaCarta.vidaBloqueio : 100f));

        Debug.Log($"[Antivírus] Recebendo dano de contato do inimigo! HP Restante: {vidaAtual}");

        if (vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log("[Antivírus] Destruído pelos inimigos!");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float raioVisual = (dadosDaCarta != null) ? dadosDaCarta.alcance : alcance;
        Gizmos.DrawWireSphere(transform.position, raioVisual);
    }
}