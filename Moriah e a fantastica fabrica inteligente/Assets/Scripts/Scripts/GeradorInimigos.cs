using UnityEngine;
using UnityEngine.SceneManagement;

public class GeradorInimigos : MonoBehaviour
{
    [Header("Configuração de Spawns")]
    public GameObject[] prefabsInimigos;
    public Transform pontoNascimento;
    public Transform nucleo;
    public Vector3 range;

    [Header("Escalabilidade Base")]
    public float intervalo = 3f;
    public float intervaloChecagem = 5f;
    public int quantidadePorOnda = 5;

    private float contador;

    private int inimigosVivosNaHorda = 0;
    private bool hordaEmAndamento = false;

    private void Start()
    {
        if(nucleo == null)
        {
            GameObject placaObj = GameObject.Find("PlacaMae");
            if (placaObj != null) nucleo = placaObj.transform;
        }
    }

    private void Update()
    {
        if (GerenciadorJogo.Instancia == null || GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.TowerDefense2D)
            return;

        
        if(!hordaEmAndamento)
        {
            contador += Time.deltaTime;

            if (contador >= intervaloChecagem)
            {
                contador = 0f;
                TentarDispararHorda();
            }
        }
    }

    private void TentarDispararHorda()
    {
        if(prefabsInimigos == null || prefabsInimigos.Length == 0) return;

        float ameacaAtual = GerenciadorJogo.Instancia.ameacaCibernetica;

        float sorteio = Random.Range(0f, 100f);

        if(sorteio <= ameacaAtual)
        {
            DispararHordaPorNivelDeAmeaca(ameacaAtual);
        }
        else
        {
            Debug.Log($"[GeradorInimigos] Checagem de segurança realizada. Ameaça em {ameacaAtual:F1}%. Nenhum ataque nesta janela.");
        }
    }

    private void DispararHordaPorNivelDeAmeaca(float ameaca)
    {
        int quantidadeInimigos = Mathf.Clamp(Mathf.FloorToInt(ameaca / 15f), 2, 15);

        Debug.Log($"<color=red>[ALEERTA DE ATAQUE]</color> Horda cibernética deflagrada!" +
            $"Ameaça atual: {ameaca:F1}% | Unidades na horda: {quantidadeInimigos}");

        hordaEmAndamento = true;
        inimigosVivosNaHorda = quantidadeInimigos;

        for(int i = 0; i < quantidadeInimigos; i++)
        {
            CriarInimigo();
        }
        
    }

    private void CriarInimigo()
    {
        // No futuro, podemos filtrar quais inimigos aparecem dependendo de quão alta está a ameaça (ex: Trojans só em ameaças altas)
        GameObject prefab = prefabsInimigos[Random.Range(0, prefabsInimigos.Length)];

        Vector3 posSpawn = pontoNascimento != null ? pontoNascimento.position : transform.position;
        Vector3 pos = posSpawn + Vector3.Lerp(-range, range, Random.value);
        pos.z = 0f;

        GameObject inimigo = Instantiate(prefab, pos, Quaternion.identity);

        InimigoCibernetico componente = inimigo.GetComponent<InimigoCibernetico>();
        if (componente != null && nucleo != null)
            componente.alvo = nucleo;

        RastreadorInimigo rastreador = inimigo.AddComponent<RastreadorInimigo>();
        rastreador.Inicializar(this);
    }

    public void RegistrarInimigoDestruido()
    {
        if(!hordaEmAndamento) return;

        inimigosVivosNaHorda--;
        Debug.Log($"[GeradorInimigo] Inimigo neutralizado. Restam {inimigosVivosNaHorda} na horda.");

        if(inimigosVivosNaHorda <= 0)
        {
            ConcluirHorda();
        }
    }

    private void ConcluirHorda()
    {
        hordaEmAndamento = false;
        inimigosVivosNaHorda = 0;

        if(GerenciadorJogo.Instancia != null)
        {
            float reducaoPosHorda = 5f;
            GerenciadorJogo.Instancia.ReduzirAmeaca(reducaoPosHorda);
            Debug.Log($"<color=green>[HORDA CONCLUÍDA]</color> Todos os inimigos foram eliminados! Ameaça aliviada em -{reducaoPosHorda}%.");
        }
    }
}

public class RastreadorInimigo : MonoBehaviour
{
    private GeradorInimigos gerador;
    private bool computado = false;

    public void Inicializar(GeradorInimigos geradorRef)
    {
        gerador = geradorRef;   
    }

    private void OnDestroy()
    {
        computado = false;
        gerador.RegistrarInimigoDestruido();
    }
}