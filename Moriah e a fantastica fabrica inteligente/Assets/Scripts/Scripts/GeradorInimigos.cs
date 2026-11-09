using UnityEngine;
using UnityEngine.SceneManagement;

public class GeradorInimigos : MonoBehaviour
{
    [Header("Configura��o de Spawns M�ltiplos")]
    public GameObject[] prefabsInimigos;
    public Transform[] pontosNascimento;
    public Transform nucleo;
    public Vector3 range = new Vector3(2f, 2f, 0f);

    [Header("Escalabilidade Base")]
    public float intervaloChecagem = 5f;
    private float contador;

    private void Update()
    {
        if (GerenciadorJogo.Instancia == null)
            return;

        // Se o jogo estiver pausado ou no menu, n�o conta o tempo da horda
        if (GerenciadorJogo.Instancia.estadoAtual == GerenciadorJogo.EstadoJogo.Pausado ||
            GerenciadorJogo.Instancia.estadoAtual == GerenciadorJogo.EstadoJogo.GameOver ||
            GerenciadorJogo.Instancia.estadoAtual == GerenciadorJogo.EstadoJogo.Vitoria ||
            GerenciadorJogo.Instancia.estadoAtual == GerenciadorJogo.EstadoJogo.Menu)
            return;

        // O tempo roda constantemente em segundo plano, independentemente de estar no 3D ou no terminal
        if (GerenciadorJogo.Instancia.producaoPorSegundo > 0f)
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
        if (prefabsInimigos == null || prefabsInimigos.Length == 0) return;

        float ameacaAtual = GerenciadorJogo.Instancia.ameacaCibernetica;
        float sorteio = Random.Range(0f, 100f);

        if (sorteio <= ameacaAtual)
        {
            DispararHordaPorNivelDeAmeaca(ameacaAtual);
        }
    }

    private void DispararHordaPorNivelDeAmeaca(float ameaca)
    {
        int quantidadeInimigos = Mathf.Clamp(Mathf.FloorToInt(ameaca / 15f), 2, 15);

        Debug.Log($"<color=red>[ALERTA DE ATAQUE]</color> Horda cibern�tica deflagrada imediatamente! Amea�a atual: {ameaca:F1}% | Unidades: {quantidadeInimigos}");

        // Busca o n�cleo (PlacaMae) nas cenas ativas
        if (nucleo == null)
        {
            GameObject placaObj = GameObject.Find("PlacaMae");
            if (placaObj != null) nucleo = placaObj.transform;
        }

        // Busca os pontos de spawn pela Tag
        if (pontosNascimento == null || pontosNascimento.Length == 0)
        {
            GameObject[] objsTag = GameObject.FindGameObjectsWithTag("PontoSpawn");
            if (objsTag.Length > 0)
            {
                pontosNascimento = new Transform[objsTag.Length];
                for (int i = 0; i < objsTag.Length; i++)
                {
                    pontosNascimento[i] = objsTag[i].transform;
                }
            }
            else
            {
                pontosNascimento = new Transform[] { transform };
            }
        }

        if (nucleo == null || pontosNascimento.Length == 0)
        {
            Debug.LogWarning("[GeradorInimigos] N�cleo ou Pontos de Spawn n�o encontrados na cena ativa.");
            return;
        }

        // Instancia os inimigos imediatamente na grade de defesa
        for (int i = 0; i < quantidadeInimigos; i++)
        {
            CriarInimigo();
        }

        // Reduz a amea�a ap�s o ataque ser gerado
        GerenciadorJogo.Instancia.ReduzirAmeaca(15f);
    }

    private void CriarInimigo()
    {
        if (prefabsInimigos.Length == 0) return;

        GameObject prefab = prefabsInimigos[Random.Range(0, prefabsInimigos.Length)];

        Transform pEscolhido = transform;
        if (pontosNascimento != null && pontosNascimento.Length > 0)
        {
            Transform pTemp = pontosNascimento[Random.Range(0, pontosNascimento.Length)];
            if (pTemp != null) pEscolhido = pTemp;
        }

        Vector3 posSpawn = pEscolhido != null ? pEscolhido.position : transform.position;
        Vector3 pos = posSpawn + Vector3.Lerp(-range, range, Random.value);
        pos.z = 0f;

        GameObject inimigo = Instantiate(prefab, pos, Quaternion.identity);

        if (nucleo != null)
        {
            Inimigo compInimigo = inimigo.GetComponent<Inimigo>();
            if (compInimigo != null) compInimigo.alvo = nucleo.gameObject;

            Trojan compTrojan = inimigo.GetComponent<Trojan>();
            if (compTrojan != null) compTrojan.DefinirAlvo(nucleo);

            DDosBots compDdos = inimigo.GetComponent<DDosBots>();
            if (compDdos != null) compDdos.DefinirAlvo(nucleo);
        }
    }
}