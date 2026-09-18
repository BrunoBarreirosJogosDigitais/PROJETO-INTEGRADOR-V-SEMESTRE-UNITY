using UnityEngine;

public class GerenciadorJogo : MonoBehaviour
{
    public static GerenciadorJogo Instancia { get; private set; }

    [Header("Economia")]
    public float dinheiroInicial = 1000f;
    public float dinheiro;
    public float producaoPorSegundo;
    public float manutencaoPorSegundo;

    [Header("Seguranca")]
    [Range(0, 100)] public float ameacaCibernetica = 0f;
    public int nivelTI = 1;

    [Header("Tower Defense")]
    public int pontosServidorMaximos = 100;
    public int pontosServidor;
    public int vidaPlacaMaxima = 500;
    public int vidaPlaca;

    private float contadorEconomia;
    private float contadorAmeaca;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
        dinheiro = dinheiroInicial;
        pontosServidor = pontosServidorMaximos;
        vidaPlaca = vidaPlacaMaxima;
    }

    private void Update()
    {
        contadorEconomia += Time.deltaTime;

        if (contadorEconomia >= 1f)
        {
            contadorEconomia = 0f;
            float lucro = producaoPorSegundo - manutencaoPorSegundo;
            dinheiro += Mathf.Max(0f, lucro);
        }

        if (producaoPorSegundo > 0f)
        {
            contadorAmeaca += Time.deltaTime;

            if (contadorAmeaca >= 10f)
            {
                contadorAmeaca = 0f;
                AumentarAmeaca(1f);
            }
        }
    }

    public bool GastarDinheiro(float valor)
    {
        if (valor <= 0f) return true;

        if (dinheiro < valor)
        {
            Debug.Log("Dinheiro insuficiente.");
            return false;
        }

        dinheiro -= valor;
        return true;
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
        vidaPlacaMaxima += 100;
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
            Debug.Log("A defesa falhou!");
        }
    }

    public void CurarPlaca(int quantidade)
    {
        vidaPlaca = Mathf.Clamp(vidaPlaca + quantidade, 0, vidaPlacaMaxima);
    }
}
