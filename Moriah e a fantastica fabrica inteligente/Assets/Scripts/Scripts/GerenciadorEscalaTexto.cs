using System;
using UnityEngine;

public class GerenciadorEscalaTexto : MonoBehaviour
{
    public static GerenciadorEscalaTexto Instancia { get; private set; }

    public const float EscalaMinima = 1f;
    public const float EscalaMaxima = 1.3f;

    public event Action<float> AoMudarEscala;

    public float EscalaAtual { get; private set; } = 1f;

    private const string ChavePrefs = "EscalaTexto";

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        EscalaAtual = Mathf.Clamp(
            PlayerPrefs.GetFloat(ChavePrefs, 1f),
            EscalaMinima, EscalaMaxima);
    }

    public void DefinirEscala(float escala)
    {
        EscalaAtual = Mathf.Clamp(escala, EscalaMinima, EscalaMaxima);

        PlayerPrefs.SetFloat(ChavePrefs, EscalaAtual);
        PlayerPrefs.Save();

        AoMudarEscala?.Invoke(EscalaAtual);
    }

    public void Restaurar()
    {
        DefinirEscala(1f);
    }
}
