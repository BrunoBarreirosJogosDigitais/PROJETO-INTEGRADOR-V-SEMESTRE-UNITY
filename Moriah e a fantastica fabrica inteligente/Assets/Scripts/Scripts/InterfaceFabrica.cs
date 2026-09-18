using UnityEngine;
using UnityEngine.UI;

public class InterfaceFabrica : MonoBehaviour
{
    public Text textoDinheiro;
    public Text textoProducao;
    public Text textoManutencao;
    public Text textoAmeaca;
    public Text textoTI;

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

        if (textoAmeaca != null)
            textoAmeaca.text = "Ameaça: " + jogo.ameacaCibernetica.ToString("0") + "%";

        if (textoTI != null)
            textoTI.text = "TI: Nível " + jogo.nivelTI;
    }
}
