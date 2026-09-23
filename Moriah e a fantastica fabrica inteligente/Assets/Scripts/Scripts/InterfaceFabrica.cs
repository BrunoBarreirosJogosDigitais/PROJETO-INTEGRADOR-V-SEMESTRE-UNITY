using UnityEngine;
using UnityEngine.UI;

public class InterfaceFabrica : MonoBehaviour
{
    [Header("HUD - Tycoon")]
    public Text textoDinheiro;
    public Text textoProducao;
    public Text textoManutencao;
    public Slider sliderAmeaca;
    public Slider sliderTI;
    public int nivelTIMaximoExemplo = 10;

    [Header("HUD - Tower Defense")]
    public Text textoPontosServidor;
    public Text textoVidaPlaca;

    private void Update()
    {
        if (GerenciadorJogo.Instancia == null) return;

        var jogo = GerenciadorJogo.Instancia;

        if (textoDinheiro != null)
            textoDinheiro.text = "Dinheiro: R$ " + jogo.dinheiro.ToString("N0");

        if (textoProducao != null)
            textoProducao.text = "Produção: " + jogo.producaoPorSegundo.ToString("0") + "/s";

        if (textoManutencao != null)
            textoManutencao.text = "Manutenção: R$ " + jogo.manutencaoPorSegundo.ToString("0") + "/s";

        if (textoPontosServidor != null)
            textoPontosServidor.text = "Energia: " + jogo.pontosServidor + " / " + jogo.pontosServidorMaximos;

        if (textoVidaPlaca != null)
            textoVidaPlaca.text = "Placa-Mãe: " + jogo.vidaPlaca + " / " + jogo.vidaPlacaMaxima + "PV";

        if(sliderAmeaca != null)
        {
            sliderAmeaca.maxValue = 100f;
            sliderAmeaca.value = jogo.ameacaCibernetica;
        }

        if(sliderTI != null)
        {
            sliderTI.maxValue = nivelTIMaximoExemplo;
            sliderTI.value = jogo.nivelTI;
        }
    }
}
