using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

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
    public string nomeCanvasTycoon = "CanvasTycoon";
    public string nomeCenaTerminal = "TerminalTI";

    private void Start()
    {
        AlternarVisibilidaeTycoon(false);
    }

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
        AlternarVisibilidaeTycoon(true);

        if (GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.AlternaModoJogo(GerenciadorJogo.EstadoJogo.Tycoon3D);

            //GerenciadorJogo.Instancia.ReduzirAmeaca(15f);
        }

        SceneManager.UnloadSceneAsync(nomeCenaTerminal);
    }

    private void AlternarVisibilidaeTycoon(bool ativar)
    {
        Scene cenaFabrica = SceneManager.GetSceneByName(nomeCenaFabrica);
        if(cenaFabrica.IsValid() && cenaFabrica.isLoaded)
        {
            foreach(GameObject obj in cenaFabrica.GetRootGameObjects())
            {
                if(obj.name == nomeCanvasTycoon)
                {
                    obj.SetActive(ativar);
                    Debug.Log("Canvas do Tycoon (" + obj.name + ") foi alterado para: " + ativar);
                    return;
                }
            }
        }
    }
}
