using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class SliderEscalaTexto : MonoBehaviour
{
    [Header("ISSO É SÓ PRA DEBUGAR, NÃO É USADO NO JOGO, SÓ PRA TESTAR O SLIDER")]
    public Text textoPercentual;
    public TMP_Text textoPercentualTMP;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = GerenciadorEscalaTexto.EscalaMinima;
        slider.maxValue = GerenciadorEscalaTexto.EscalaMaxima;
        slider.wholeNumbers = false;
    }

    private void OnEnable()
    {
        float escala = GerenciadorEscalaTexto.Instancia != null
            ? GerenciadorEscalaTexto.Instancia.EscalaAtual
            : 1f;

        // Atualiza o slider e o rótulo sem disparar eventos
        slider.SetValueWithoutNotify(escala);
        AtualizarRotulo(escala);

        slider.onValueChanged.AddListener(AoMudarSlider);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(AoMudarSlider);
    }

    private void AoMudarSlider(float valor)
    {
        if (GerenciadorEscalaTexto.Instancia != null)
            GerenciadorEscalaTexto.Instancia.DefinirEscala(valor);

        AtualizarRotulo(valor);
    }

    // Esse "rótulo" é o texto que mostra o valor atual do slider, só pra debug mesmo.
    private void AtualizarRotulo(float valor)
    {
        string texto = Mathf.RoundToInt(valor * 100f) + "%";

        if (textoPercentual != null) textoPercentual.text = texto;
        if (textoPercentualTMP != null) textoPercentualTMP.text = texto;
    }
}
