using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorJogo : MonoBehaviour
{
    public static GerenciadorJogo Instancia { get; private set; }

    public enum EstadoJogo { Menu, Tycoon3D, TowerDefense2D, Pausado, GameOver, Vitoria}

    [Header("Estado Atual")]
    public EstadoJogo estadoAtual;

    [Header("Economia")]
    public float dinheiroInicial = 1000f;
    public float dinheiro;
    public float producaoPorSegundo;
    public float manutencaoPorSegundo;
    public float metaFinanceiraVitoria = 3000000f; //Meta para o final do game

    [Header("Seguranca")]
    [Range(0, 100)] public float ameacaCibernetica = 0f;
    public int nivelTI = 1;

    [Header("Tower Defense")]
    public int pontosServidorMaximos = 100;
    public int pontosServidor;
    public int vidaPlacaMaxima = 900000;
    public int vidaPlaca;

    [Header("Energia e Servidores")]
    public float geracaoEnergiaPorSegundo = 0f;
    private float contadorEnergia;

    private float contadorEconomia;
    private float contadorAmeaca;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        if(geracaoEnergiaPorSegundo > 0f && pontosServidor < pontosServidorMaximos)
        {
            contadorEnergia += Time.deltaTime;

            if(contadorEnergia >= 1f)
            {
                contadorEnergia = 0f;
                RecuperarPontosServidor(Mathf.RoundToInt(geracaoEnergiaPorSegundo));
            }
        }

        //Persistencia entre as cenas
        Instancia = this;
        DontDestroyOnLoad(gameObject);


        dinheiro = dinheiroInicial;
        pontosServidor = pontosServidorMaximos;
        vidaPlaca = vidaPlacaMaxima;
        estadoAtual = EstadoJogo.Menu;
    }

    private void Update()
    {
        if (estadoAtual == EstadoJogo.Pausado || estadoAtual == EstadoJogo.GameOver ||
            estadoAtual == EstadoJogo.Vitoria)
            return;

        //O dinheiro passivo do protagonista irá se incrementar gradativamente
        contadorEconomia += Time.deltaTime;


        if (contadorEconomia >= 1f)
        {
            contadorEconomia = 0f;
            float lucro = producaoPorSegundo - manutencaoPorSegundo;
            dinheiro += Mathf.Max(0f, lucro);

            VerificarCondicaoVitoria();
        }

        //Se a produção por segundo for maior que zero o contador de ameaça ira aumentar
        if (producaoPorSegundo > 0f)
        {
            contadorAmeaca += Time.deltaTime;

            //Se o contador de ameaça for maior ou igual a 10 a função AumentarAmeaça sobe para mais 1
            if (contadorAmeaca >= 10f)
            {
                //Zera o contador para iniciar a contagem novamente
                contadorAmeaca = 0f;
                AumentarAmeaca(1f);
            }
        }
    }

    //Função que será chamada em momentos de melhorias das maquinas ou na liberação de outras áreas
    public bool GastarDinheiro(float valor)
    {
        if (valor <= 0f) return true;

        if (dinheiro < valor)
        {
            Debug.Log("Dinheiro insuficiente.");
            return false;
        }

        dinheiro -= valor;
        VerificarCondicaoVitoria();
        return true;
    }

    public void AdicionarGeracaoEnergia(float valor)
    {
        geracaoEnergiaPorSegundo += Mathf.Max(0f, valor);
    }

    public void RemoverGeracaoEnergia(float valor)
    {
        geracaoEnergiaPorSegundo += Mathf.Max(0f, geracaoEnergiaPorSegundo - valor);
    }
  
    public void AdicionarDinheiro(float valor)
    {
        if (valor > 0f) dinheiro += valor;
    }

    //Função usada nas maquinas da fabrica
    public void AdicionarProducao(float valor)
    {
        producaoPorSegundo += Mathf.Max(0f, valor);
    }


    public void AdicionarManutencao(float valor)
    {
        manutencaoPorSegundo += Mathf.Max(0f, valor);
    }

    public void RemoverProducao(float valor)
    {
        producaoPorSegundo = Mathf.Max(0f, producaoPorSegundo - valor);
    }

    public void RemoverManutencao(float valor)
    {
        manutencaoPorSegundo = Mathf.Max(0f, manutencaoPorSegundo - valor);
    }

    //Função usada na para aumentar as ameaças ciberneticas
    public void AumentarAmeaca(float valor)
    {
        ameacaCibernetica = Mathf.Clamp(ameacaCibernetica + valor, 0f, 100f);
    }

    public void ReduzirAmeaca(float valor)
    {
        ameacaCibernetica = Mathf.Clamp(ameacaCibernetica - valor, 0f, 100f);
    }

    public void MelhorarTI()
    {
        int custo = nivelTI * 1500;

        if (!GastarDinheiro(custo)) return;

        nivelTI++;
        pontosServidorMaximos += 25;
        pontosServidor = pontosServidorMaximos;
        vidaPlacaMaxima += 100000;
        vidaPlaca = vidaPlacaMaxima;

        ReduzirAmeaca(5f);
    }

    public bool GastarPontosServidor(int quantidade)
    {
        if (pontosServidor < quantidade) return false;
        pontosServidor -= quantidade;
        return true;
    }

    public void RecuperarPontosServidor(int quantidade)
    {
        pontosServidor = Mathf.Clamp(pontosServidor + quantidade, 0, pontosServidorMaximos);
    }

    public void ReceberDanoPlaca(int dano)
    {
        vidaPlaca = Mathf.Max(0, vidaPlaca - dano);

        if (vidaPlaca <= 0)
        {
            AcionarGameOver();
            Debug.Log("A defesa falhou!");
        }
    }

    public void CurarPlaca(int quantidade)
    {
        vidaPlaca = Mathf.Clamp(vidaPlaca + quantidade, 0, vidaPlacaMaxima);
    }

    private void VerificarCondicaoVitoria()
    {
        if(dinheiro >= metaFinanceiraVitoria && estadoAtual != EstadoJogo.Vitoria)
        {
            AcionarVitoria();
        }
    }

    public void IniciarJogo()
    {
        dinheiro = dinheiroInicial;
        pontosServidor = pontosServidorMaximos;
        vidaPlaca = vidaPlacaMaxima;
        ameacaCibernetica = 0f;

        estadoAtual = EstadoJogo.Tycoon3D;

        SceneManager.LoadScene("Fase");

        Debug.Log("Jogo iniciado! Entrando no modo Tycoon 3D da fábrica... ");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

        // Se estiver testando dentro do Editor da Unity, ele para o modo Play
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Se for o jogo exportado (buildado), ele fecha o aplicativo de verdade
            Application.Quit();
        #endif
    }

    private void AcionarGameOver()
    {
        estadoAtual = EstadoJogo.GameOver;
        Debug.Log("Game Over! O núcleo da Placa-mãe foi destruído");
    }

    private void AcionarVitoria()
    {
        estadoAtual = EstadoJogo.Vitoria;
        Debug.Log("Parabéns! Meta financeira de R$ 3.000.000 atingida!");
    }

    public void AlternaModoJogo(EstadoJogo novoEstado)
    {
        estadoAtual = novoEstado;

        if(estadoAtual == EstadoJogo.TowerDefense2D)
        {
            Debug.Log("Entrando no modo Cibernético (Tower Defense 2D)...");
        }
        else if(estadoAtual == EstadoJogo.Tycoon3D)
        {
            Debug.Log("Retornando ao modo Indústria (Tycoon 3D)...");
        }
    }
}
