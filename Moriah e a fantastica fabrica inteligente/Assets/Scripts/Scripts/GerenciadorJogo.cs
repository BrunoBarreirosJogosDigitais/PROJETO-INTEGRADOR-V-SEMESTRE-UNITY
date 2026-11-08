using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorJogo : MonoBehaviour
{
    /// 
    /// [FIXO 1] Singleton e Gestão de Estados Globais
    /// 

    // Instância estática para o padrão Singleton garantindo acesso global único)
    public static GerenciadorJogo Instancia { get; private set; }

    //Enum para controlar os estados globais do jogo na State Machine
    public enum EstadoJogo { Menu, Tycoon3D, TowerDefense2D, Pausado, GameOver, Vitoria }

    [Header("Estado Atual")]
    public EstadoJogo estadoAtual;
    private EstadoJogo estadoAnteriorAntesDoPause;

    [Header("Economia")]
    public float dinheiroInicial = 1000f;
    public float dinheiro;
    public float producaoPorSegundo;
    public float manutencaoPorSegundo;
    public float metaFinanceiraVitoria = 3000000f; // Meta para o final do game

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
    private float contadorEnergia = 5f;

    [Header("Bônus do Setor de TI (Área 2)")]
    public bool criptografiaAtivado = false;
    public bool supercomputadorAtivado = false;

    [Header("Bônus do Setor de RH (Área 3)")]
    public bool simuladorPhishingAtivo = false;
    public bool centralErgonomiaAtiva = false;
    public bool academiaDDoSAtiva = false;

    private float contadorEconomia;
    private float contadorAmeaca;

    [Header("Cenas")]
    public string cenaMenu = "Menu";
    public string cenaVitoria = "Vitoria";
    public string cenaGameOver = "GameOver";

    [Header("Configuração de Hordas Global")]
    public float intervaloChecagemHorda = 5f;
    private float contadorHorda;
    public GameObject[] prefabsInimigos;
    public Transform[] pontosNascimento;
    public Transform nucleo;
    public Vector3 rangeSpawn = new Vector3(2f, 2f, 0f);
    public int numeroOndaAtual = 0;
    public AudioSource audioSourceAlarme;
    public AudioClip somAlerta;

    // Controle de horda ativa para a ameaça
    private bool hordaEmAndamento = false;

    [Header("Teste - Drenar Vida da Placa")]
    public bool drenarVidaTeste = false;
    public int vidaPerdidaPorSegundo = 10000;
    private float contadorDrenoVida;

    private bool vitoriaConquistada;

    //Método Awake executado antes de Start: Configura e SIgleton e persistência
    private void Awake()
    {
        //[FIXO 1.2] Validação de Duplicidade e DontDestroyOnLoad
        // Se já exiiste uma instância ativa e não é esta, destroi o duplicado
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        // Persistencia entre as cenas e define a instancia global impedindo que o objeto seja
        // destruído ao mudar de cena
        Instancia = this;
        DontDestroyOnLoad(gameObject);

        //Inicializa valores padrão de partida
        dinheiro = dinheiroInicial;
        pontosServidor = pontosServidorMaximos;
        vidaPlaca = vidaPlacaMaxima;
        estadoAtual = EstadoJogo.Menu;
    }
    //=========================================


    /// ============================================
    /// BLOCO 2: LOOP ECONÔMICO E DE AMEÇA NO UPDATE
    /// =============================================
    private void Update()
    {
        //Atalho de teclado ESC para pausar/retomar o jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (estadoAtual == EstadoJogo.Tycoon3D || estadoAtual == EstadoJogo.TowerDefense2D)
            {
                PausarJogo();
            }
            else if (estadoAtual == EstadoJogo.Pausado)
            {
                RetomarJogo();
            }
        }

        //Se o jogo estiver pausado ou fora da partida, interrompe o Update
        if (estadoAtual == EstadoJogo.Pausado || estadoAtual == EstadoJogo.GameOver ||
            estadoAtual == EstadoJogo.Vitoria || estadoAtual == EstadoJogo.Menu)
            return;

        // Teste: drena a vida da placa com o tempo.
        if (drenarVidaTeste)
        {
            contadorDrenoVida += Time.deltaTime;

            if (contadorDrenoVida >= 1f)
            {
                contadorDrenoVida = 0f;
                ReceberDanoPlaca(vidaPerdidaPorSegundo);
            }

            if (estadoAtual == EstadoJogo.GameOver) return;
        }

        // Regeneração automatica de pontos de servidor
        if (geracaoEnergiaPorSegundo > 0f && pontosServidor < pontosServidorMaximos)
        {
            contadorEnergia += Time.deltaTime;

            if (contadorEnergia >= 1f)
            {
                contadorEnergia = 0f;
                RecuperarPontosServidor(Mathf.RoundToInt(geracaoEnergiaPorSegundo));
            }
        }

        // O dinheiro passivo do protagonista irá se incrementar gradativamente
        // [FIXO 2] Loop de Economia Passiva com Time.deltime e bônus do RH
        contadorEconomia += Time.deltaTime;

        if (contadorEconomia >= 1f)
        {
            contadorEconomia = 0f;

            float rendaBasePassiva = 100f;
            float producaoEfetiva = producaoPorSegundo;

            //Aplica bônus de 10% da Área 3 (RH - Central de Ergonomia) se ativo
            if (centralErgonomiaAtiva)
            {
                producaoEfetiva *= 1.10f;
            }

            //Calcula o lucro líquido(Produção - Manutenção + Renda Base)
            float lucro = (producaoPorSegundo - manutencaoPorSegundo) + rendaBasePassiva;
            dinheiro += Mathf.Max(0f, lucro);

            //Valida se o jogador atingiu a meta para venceer
            VerificarCondicaoVitoria();
        }

        // Se a horda estiver em andamento, monitora se todos os inimigos foram destruídos para abaixar a ameaça
        if (hordaEmAndamento)
        {
            // Procura por todos os objetos com a tag "Inimigos" na cena ativa
            GameObject[] inimigosRestantes = GameObject.FindGameObjectsWithTag("Inimigos");

            if (inimigosRestantes.Length == 0)
            {
                // A horda terminou! Redução da ameaça
                ReduzirAmeaca(15f);
                hordaEmAndamento = false;
                Debug.Log("<color=green>[FIM DA HORDA]</color> Todos os inimigos foram eliminados. Ameaça reduzida!");
            }
        }
        else
        {
            // Se NÃO houver horda ativa, a ameaça sobe normalmente com a produção
            if (producaoPorSegundo > 0f)
            {
                contadorAmeaca += Time.deltaTime;

                if (contadorAmeaca >= 1f)
                {
                    contadorAmeaca = 0f;

                    float incrementoBase = 0.15f;
                    float fatorProducao = producaoPorSegundo * 0.0002f;
                    float subidaEfetiva = (incrementoBase + fatorProducao);

                    //Mitigação de ameaça se o Simuladdor de Phishing (RH) estiver ativo
                    if (simuladorPhishingAtivo)
                    {
                        subidaEfetiva *= 0.80f;
                    }

                    AumentarAmeaca(subidaEfetiva);
                }
            }

            // Checagem periódica para disparar nova horda
            if (producaoPorSegundo > 0f)
            {
                contadorHorda += Time.deltaTime;

                if (contadorHorda >= intervaloChecagemHorda)
                {
                    contadorHorda = 0f;
                    TentarDispararHordaGlobal();
                }
            }
        }
    }

    /// ==============================
    /// [FIXO 3] Sistema de Sorteio e Disparo de Hordas
    /// ==============================

    public static event System.Action OnAlertaAtaqueDisparado;

    //Sorteia probabilidade for menor ou igual à ameaça, a invasão é deflagrada
    private void TentarDispararHordaGlobal()
    {
        float sorteio = Random.Range(0f, 100f);

        if (sorteio <= ameacaCibernetica)
        {
            numeroOndaAtual++;
            //Calcula a quantidade de inimigos proporcional à ameça
            int quantidadeInimigos = Mathf.Clamp(Mathf.FloorToInt(ameacaCibernetica / 15f), 2, 15);
            Debug.Log($"<color=red>[ALERTA DE ATAQUE]</color> Onda {numeroOndaAtual} deflagrada! Ameaça atual: {ameacaCibernetica:F1}% | Unidades: {quantidadeInimigos}");

            DispararHordaImediata(quantidadeInimigos);

            //Toca o alarme sonoro de invasão
            if (audioSourceAlarme != null && somAlerta != null)
            {
                audioSourceAlarme.PlayOneShot(somAlerta);
            }

            hordaEmAndamento = true; // Trava para indicar que a horda começou
            OnAlertaAtaqueDisparado?.Invoke();
        }
    }

    //Instancia os inimigos dinamicamente nos pontos de spawn designados
    private void DispararHordaImediata(int quantidadeInimigos)
    {
        if (prefabsInimigos == null || prefabsInimigos.Length == 0)
        {
            Debug.LogWarning("[GerenciadorJogo] Prefabs de inimigos não atribuídos no Inspector!");
            return;
        }

        // Busca o núcleo (PlacaMae) nas cenas ativas se estiver nulo
        if (nucleo == null)
        {
            GameObject placaObj = GameObject.Find("PlacaMae");
            if (placaObj != null) nucleo = placaObj.transform;
        }

        // Busca os pontos de spawn pela Tag se necessário
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
        }

        if (nucleo == null || pontosNascimento == null || pontosNascimento.Length == 0)
        {
            Debug.LogWarning("[GerenciadorJogo] Núcleo ou Pontos de Spawn não encontrados na cena ativa para instanciar a horda.");
            return;
        }

        // Loop para instanciar cada malware e configurar seus alvos
        for (int i = 0; i < quantidadeInimigos; i++)
        {
            GameObject prefab = prefabsInimigos[Random.Range(0, prefabsInimigos.Length)];
            Transform pEscolhido = pontosNascimento[Random.Range(0, pontosNascimento.Length)];

            Vector3 posSpawn = pEscolhido != null ? pEscolhido.position : Vector3.zero;
            Vector3 pos = posSpawn + Vector3.Lerp(-rangeSpawn, rangeSpawn, Random.value);
            pos.z = 0f;

            GameObject inimigo = Instantiate(prefab, pos, Quaternion.identity);

            // Atribui o núcleo como alvo principal para os diferentes tipos de malwares
            Inimigo compInimigo = inimigo.GetComponent<Inimigo>();
            if (compInimigo != null) compInimigo.alvo = nucleo.gameObject;

            Trojan compTrojan = inimigo.GetComponent<Trojan>();
            if (compTrojan != null) compTrojan.DefinirAlvo(nucleo);

            DDosBots compDdos = inimigo.GetComponent<DDosBots>();
            if (compDdos != null) compDdos.DefinirAlvo(nucleo);
        }
    }

    /// ===========================
    /// [FIXO 4] Metodos Auxiliares de Gestão, TI, RH e ESTADOS(Inicio)
    /// ===========================
 
    public void PausarJogo()
    {
        if (estadoAtual == EstadoJogo.Pausado || estadoAtual == EstadoJogo.Menu ||
            estadoAtual == EstadoJogo.GameOver || estadoAtual == EstadoJogo.Vitoria)
            return;

        estadoAnteriorAntesDoPause = estadoAtual;
        estadoAtual = EstadoJogo.Pausado;
        Time.timeScale = 0f;
        Debug.Log("[GerenciadorJogo] Jogo Pausado");
    }

    public void RetomarJogo()
    {
        if (estadoAtual != EstadoJogo.Pausado) return;

        estadoAtual = estadoAnteriorAntesDoPause;
        if (estadoAtual == EstadoJogo.Menu || estadoAtual == EstadoJogo.Pausado)
        {
            estadoAtual = EstadoJogo.Tycoon3D; // Fallback de segurança
        }
        Time.timeScale = 1f;
        Debug.Log("[GerenciadorJogo] Jogo Retomado.");
    }

    //Valida e desconta saldo financeiro do jogador ao comprar máquinas
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

    //Ativa bônus passivos e mecânicos adquiridos no Setor de TI
    public void AtivarEfeitoTI(DadosMelhoriaTI.TipoBonusTI tipoBonus, float valorBonus)
    {
        switch (tipoBonus)
        {
            case DadosMelhoriaTI.TipoBonusTI.BlindagemNucleo:
                int hpExtra = Mathf.RoundToInt(valorBonus);
                vidaPlacaMaxima += hpExtra;
                vidaPlaca = Mathf.Min(vidaPlacaMaxima, vidaPlaca + hpExtra);
                Debug.Log($"[TI] Edge Server instalado: +{hpExtra} HP de blindagem aplicada ao núcleo da placa-mãe.");
                break;

            case DadosMelhoriaTI.TipoBonusTI.ReducaoDanoRansomware:
                criptografiaAtivado = true;
                Debug.Log("[TI] Módulo de Criptografia Quântica ativo: Redução de dano contra Ransomwares aplicada.");
                break;

            case DadosMelhoriaTI.TipoBonusTI.EconomiaCartasTD:
                supercomputadorAtivado = true;
                Debug.Log("[TI] Supercomputador Neural ativo: Custo de energia das cartas defensivas reduzido");
                break;
        }
    }

    //Ativa bônus do Setor de Recursos Humanos
    public void AtivarEfeitoRH(DadosMelhoriaRH.TipoBonusRH tipoBonus)
    {
        switch (tipoBonus)
        {
            case DadosMelhoriaRH.TipoBonusRH.MitigacaoEngenhariaSocial:
                simuladorPhishingAtivo = true;
                Debug.Log("Efeito contínuo ativo: Simulador de Phishing mitigando ameaças.");
                break;

            case DadosMelhoriaRH.TipoBonusRH.EficienciaProdutiva:
                centralErgonomiaAtiva = true;
                Debug.Log("Efeito contínuo ativo: Central de Ergonomia aplicando +10% de eficiência global.");
                break;

            case DadosMelhoriaRH.TipoBonusRH.ResistenciaDDoS:
                academiaDDoSAtiva = true;
                AdicionarGeracaoEnergia(2f);
                Debug.Log("Efeito contínuo ativo: Academia Corporativa blindando contra quedas de energia.");
                break;
        }
    }

    //Métodos imline de modificação de recursos produtivos e ameaça
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

    public void AumentarAmeaca(float valor)
    {
        ameacaCibernetica += valor;

        if (ameacaCibernetica >= 100f)
        {
            ameacaCibernetica = 0f;
            Debug.Log("A ameaça cibernética atingiu 100% e foi resetada!");
        }

        ameacaCibernetica = Mathf.Clamp(ameacaCibernetica, 0f, 100f);
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
        if (dinheiro >= metaFinanceiraVitoria && !vitoriaConquistada)
        {
            AcionarVitoria();
        }
    }

    public void IniciarJogo()
    {
        Time.timeScale = 1f;
        dinheiro = dinheiroInicial;
        pontosServidor = pontosServidorMaximos;
        vidaPlaca = vidaPlacaMaxima;
        ameacaCibernetica = 0f;
        vitoriaConquistada = false;
        contadorDrenoVida = 0f;
        hordaEmAndamento = false;

        estadoAtual = EstadoJogo.Tycoon3D;

        SceneManager.LoadScene("Fase");

        Debug.Log("Jogo iniciado! Entrando no modo Tycoon 3D da fábrica... ");
    }

    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void VoltarAoMenu()
    {
        Time.timeScale = 1f;
        estadoAtual = EstadoJogo.Menu;
        SceneManager.LoadScene(cenaMenu);
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void AcionarGameOver()
    {
        if (estadoAtual == EstadoJogo.GameOver) return;

        estadoAtual = EstadoJogo.GameOver;
        Debug.Log("Game Over! O núcleo da Placa-mãe foi destruído");

        SceneManager.LoadScene(cenaGameOver);
    }

    private void AcionarVitoria()
    {
        vitoriaConquistada = true;
        estadoAtual = EstadoJogo.Vitoria;
        Debug.Log("Parabéns! Meta financeira de R$ 3.000.000 atingida!");

        SceneManager.LoadSceneAsync(cenaVitoria, LoadSceneMode.Additive);
    }

    public void VoltarDaVitoria()
    {
        SceneManager.UnloadSceneAsync(cenaVitoria);
        estadoAtual = EstadoJogo.Tycoon3D;
    }

    public void AlternaModoJogo(EstadoJogo novoEstado)
    {
        estadoAtual = novoEstado;

        if (estadoAtual == EstadoJogo.TowerDefense2D)
        {
            Debug.Log("Entrando no modo Cibernético (Tower Defense 2D)...");
        }
        else if (estadoAtual == EstadoJogo.Tycoon3D)
        {
            Debug.Log("Retornando ao modo Indústria (Tycoon 3D)...");
        }
    }
}