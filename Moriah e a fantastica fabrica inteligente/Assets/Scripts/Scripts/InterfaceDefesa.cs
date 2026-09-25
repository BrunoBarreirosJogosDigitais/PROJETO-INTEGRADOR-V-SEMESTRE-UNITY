using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InterfaceDefesa : MonoBehaviour
{

    [Header("Elementos de Texto (UI)")]
    public Text textoServidor;
    public Text textoVida;
    public Text textoOnda;

    [Header("Painéis de Fim de Jogo")]
    public GameObject painelDerrota;
    //public GameObject painelVitoria;

    [Header("Configuração de Cenas")]
    public string nomeCenaFabrica = "Fase";

    private void Update()
    {
        if (GerenciadorJogo.Instancia == null) return;

        var jogo = GerenciadorJogo.Instancia;

        if (textoServidor != null)
            textoServidor.text = "Servidor: " +
                jogo.pontosServidor + "/" +
                jogo.pontosServidorMaximos;

        if (textoVida != null)
            textoVida.text = "Placa-mãe: " +
                jogo.vidaPlaca + "/" +
                jogo.vidaPlacaMaxima;

        if (jogo.vidaPlaca <= 0 && painelDerrota != null && !painelDerrota.activeSelf)
            painelDerrota.SetActive(true);

    }

    public void VoltarParaFabrica()
    {
        if(GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.AlternaModoJogo(GerenciadorJogo.EstadoJogo.Tycoon3D);

            //GerenciadorJogo.Instancia.ReduzirAmeaca(15f);
        }

        SceneManager.LoadScene(nomeCenaFabrica);
    }
}
