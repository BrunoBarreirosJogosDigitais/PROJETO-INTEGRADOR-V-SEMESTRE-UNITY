using UnityEngine;
using UnityEngine.SceneManagement;

public class InteracaoTerminal : MonoBehaviour
{
    [Header("Configuração da Cena de Defesa")]
    public string cenaDefesa = "TerminalTI";

    [Header("Condição Opcional")]
    public bool requerAmeacaMinimaparaEntrar = false;
    public float ameacaMinimaRequerida = 0f;

    [Header("Configuração de Proximidade")]
    public float distanciaInteracao = 2.0f; // Distância em metros para conseguir interagir
    private Transform transformPlayer;
    private bool jogadorNaArea = false;

    private void Start()
    {
        // Encontra o player automaticamente na cena pela Tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            transformPlayer = playerObj.transform;
        }
        else
        {
            Debug.LogError("Nenhum objeto com a tag 'Player' foi encontrado na cena!");
        }
    }

    private void Update()
    {
        if (transformPlayer == null) return;

        // Calcula a distância entre o terminal e o player
        float distancia = Vector3.Distance(transform.position, transformPlayer.position);

        // Verifica se o player entrou ou saiu da área de alcance
        if (distancia <= distanciaInteracao)
        {
            if (!jogadorNaArea)
            {
                jogadorNaArea = true;
                Debug.Log("Pressione 'E' para acessar o terminal de TI.");
            }

            // Se estiver na área e apertar 'E', executa a ação
            if (Input.GetKeyDown(KeyCode.E))
            {
                TentarAcessarTerminal();
            }
        }
        else
        {
            if (jogadorNaArea)
            {
                jogadorNaArea = false;
                Debug.Log("Você se afastou do terminal.");
            }
        }
    }

    private void TentarAcessarTerminal()
    {
        if (GerenciadorJogo.Instancia == null)
        {
            Debug.LogError("GerenciadorJogo não encontrado na cena!");
            return;
        }

        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        if (requerAmeacaMinimaparaEntrar && GerenciadorJogo.Instancia.ameacaCibernetica <= ameacaMinimaRequerida)
        {
            Debug.Log($"Ameaça cibernética baixa ({GerenciadorJogo.Instancia.ameacaCibernetica}%). O terminal está seguro por enquanto.");
            return;
        }

        // Altera o estado global do jogo para o modo Tower Defense 2D
        GerenciadorJogo.Instancia.AlternaModoJogo(GerenciadorJogo.EstadoJogo.TowerDefense2D);

        // Carrega a cena do minigame/defesa
        Debug.Log("Acessando o terminal de defesa cibernética...");
        SceneManager.LoadScene(cenaDefesa);
    }
}