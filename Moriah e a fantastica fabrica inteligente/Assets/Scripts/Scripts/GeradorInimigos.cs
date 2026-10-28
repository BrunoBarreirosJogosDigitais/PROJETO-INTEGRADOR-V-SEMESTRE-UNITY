using UnityEngine;

public class GeradorInimigos : MonoBehaviour
{
    [Header("Configuração de Inimigos")]
    public GameObject[] prefabsInimigos;
    public Transform pontoNascimento;
    public Transform nucleo;
    public Vector3 range;

    [Header("Escalonamento Dinâmico")]
    public float intervalo = 3f;
    public int quantidadePorOnda = 5;
    public float intervaloBase = 5f;
    public float intervaloMinimo = 1f;

    private float contador;

    private void Start()
    {
        // InvokeRepeating("CriarInimigo", 0, intervalo);
    }

    private void Update()
    {
        if (GerenciadorJogo.Instancia == null) return;

        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.TowerDefense2D &&
            GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D)
            return;

        contador += Time.deltaTime;

        float producaoAtual = GerenciadorJogo.Instancia.producaoPorSegundo;

        float intervaloDinamico = Mathf.Max(intervaloMinimo, intervaloBase / Mathf.Max(1f, producaoAtual * 0.05f));

        if (contador >= intervalo)
        {
            contador = 0f;
            CriarInimigo();
        }
    }

    private void CriarInimigo()
    {
        if (prefabsInimigos == null || prefabsInimigos.Length == 0) return;

        GameObject prefab = prefabsInimigos[
            Random.Range(0, prefabsInimigos.Length)
        ];

        Vector3 pos = this.transform.position + Vector3.Lerp(-range, range, Random.value);

        GameObject inimigo = Instantiate(
            prefab,
            pos,
            Quaternion.identity
        );

        if (nucleo != null)
        {
            // Verifica se o script de componente compatível está presente e atribui o alvo
            // (Compatível com os scripts que criamos: Inimigo, DDosBots, Trojan)
            Transform alvoTransform = nucleo;

            // Tentativa genérica de injetar o alvo dependendodependendo do tipo de script do inimigo
            var inimigoSimples = inimigo.GetComponent<Inimigo>();
            if (inimigoSimples != null && alvoTransform != null)
            {
                inimigoSimples.alvo = alvoTransform.gameObject;
            }
        }
    }
}
