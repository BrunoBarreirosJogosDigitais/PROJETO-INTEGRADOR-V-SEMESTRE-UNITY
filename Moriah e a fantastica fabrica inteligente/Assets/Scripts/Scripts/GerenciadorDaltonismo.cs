using System;
using UnityEngine;

public enum ModoDaltonismo
{
    Nenhum = 0,
    Protanopia = 1,
    Deuteranopia = 2,
    Tritanopia = 3
}

public class GerenciadorDaltonismo : MonoBehaviour
{
    public static GerenciadorDaltonismo Instancia { get; private set;}

    public event Action<ModoDaltonismo> AoMudarModo;

    [Header("Configuracao")]
    [SerializeField] private Shader shaderDaltonismo;
    [Range(0f, 1f)] public float intensidade = 1f;

    public ModoDaltonismo ModoAtual { get; private set; }
    public Material Material { get; private set; }

    private const string ChavePrefs = "ModoDaltonismo";

    // Matrizes de simulação de daltonismo
    private static readonly Matrix4x4 SimProtanopia = Matriz3x3(
        0.152286f, 1.052583f, -0.204868f,
        0.114503f, 0.786281f, 0.099216f,
       -0.003882f, -0.048116f, 1.051998f);

    private static readonly Matrix4x4 SimDeuteranopia = Matriz3x3(
        0.367322f, 0.860646f, -0.227968f,
        0.280085f, 0.672501f, 0.047413f,
       -0.011820f, 0.042940f, 0.968881f);

    private static readonly Matrix4x4 SimTritanopia = Matriz3x3(
        1.255528f, -0.076749f, -0.178779f,
        -0.078411f, 0.930809f, 0.147602f,
        0.004733f, 0.691367f, 0.303900f);

    // Matrizes de correção de daltonismo
    private static readonly Matrix4x4 CorrProtanopia = Matriz3x3(
        0f, 0f, 0f,
        0.7f, 1f, 0f,
        0.7f, 0f, 1f);

    private static readonly Matrix4x4 CorrDeuteranopia = Matriz3x3(
        1f, 0.7f, 0f,
        0f, 0f, 0f,
        0f, 0.7f, 1f);

    private static readonly Matrix4x4 CorrTritanopia = Matriz3x3(
        1f, 0f, 0.7f,
        0f, 1f, 0.7f,
        0f, 0f, 0f);

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        if (shaderDaltonismo != null)
            Material = new Material(shaderDaltonismo);
        else
            Debug.LogError("GerenciadorDaltonismo: arraste o shader Hidden/Daltonismo no Inspector.");

        ModoAtual = (ModoDaltonismo)PlayerPrefs.GetInt(ChavePrefs, 0);
        AplicarParametros();
    }

    /// <summary>
    /// Ativa o modo de daltonismo especificado e salva a configuração.
    /// </summary>
    public void AlternarModo(ModoDaltonismo modo)
    {
        DefinirModo(ModoAtual == modo ? ModoDaltonismo.Nenhum : modo);
    }

    public void DefinirModo(ModoDaltonismo modo)
    {
        ModoAtual = modo;
        PlayerPrefs.SetInt(ChavePrefs, (int)modo);
        PlayerPrefs.Save();

        AplicarParametros();
        AoMudarModo?.Invoke(modo);
    }

    private void AplicarParametros()
    {
        if (Material == null) return;

        switch (ModoAtual)
        {
            case ModoDaltonismo.Protanopia:
                Material.SetMatrix("_MatrizSimulacao", SimProtanopia);
                Material.SetMatrix("_MatrizCorrecao", CorrProtanopia);
                break;
            case ModoDaltonismo.Deuteranopia:
                Material.SetMatrix("_MatrizSimulacao", SimDeuteranopia);
                Material.SetMatrix("_MatrizCorrecao", CorrDeuteranopia);
                break;
            case ModoDaltonismo.Tritanopia:
                Material.SetMatrix("_MatrizSimulacao", SimTritanopia);
                Material.SetMatrix("_MatrizCorrecao", CorrTritanopia);
                break;
        }

        Material.SetFloat("_Intensidade", intensidade);
    }

    // Isso aqui monta uma matriz 3x3 a partir de 9 valores, linha por linha
    private static Matrix4x4 Matriz3x3(float a, float b, float c,
                                       float d, float e, float f,
                                       float g, float h, float i)
    {
        Matrix4x4 m = Matrix4x4.identity;
        m.SetRow(0, new Vector4(a, b, c, 0f));
        m.SetRow(1, new Vector4(d, e, f, 0f));
        m.SetRow(2, new Vector4(g, h, i, 0f));
        return m;
    }
}
