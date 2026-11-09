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

    [Header("Configuração de Cenas")]
    public string nomeCanvasTycoon = "CanvasTycoon";
    public string nomeCenaTerminal = "TerminalTI";

    private void Start()
    {
        // Esconde o canvas do Tycoon em todas as cenas carregadas assim que o terminal abre
        AlternarVisibilidadeTycoonEmTodasAsCenas(false);

        if (GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.AlternaModoJogo(GerenciadorJogo.EstadoJogo.TowerDefense2D);
        }
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

        if(textoOnda != null)
        {
            textoOnda.text = "Onda: " + jogo.numeroOndaAtual;
        }

        if (jogo.vidaPlaca <= 0 && painelDerrota != null && !painelDerrota.activeSelf)
            painelDerrota.SetActive(true);
    }

    public void VoltarParaFabrica()
    {
        // Restaura a visibilidade do Tycoon nas cenas ativas
        AlternarVisibilidadeTycoonEmTodasAsCenas(true);

        if (GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.AlternaModoJogo(GerenciadorJogo.EstadoJogo.Tycoon3D);
        }

        if (SceneManager.GetSceneByName(nomeCenaTerminal).isLoaded)
        {
            SceneManager.UnloadSceneAsync(nomeCenaTerminal);
        }
    }

    private void AlternarVisibilidadeTycoonEmTodasAsCenas(bool ativar)
    {
        // Varre todas as cenas abertas no modo Additive para achar e desligar o canvas da fábrica
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene cena = SceneManager.GetSceneAt(i);
            if (cena.IsValid() && cena.isLoaded)
            {
                foreach (GameObject obj in cena.GetRootGameObjects())
                {
                    if (obj.name == nomeCanvasTycoon)
                    {
                        obj.SetActive(ativar);
                        Debug.Log("Canvas do Tycoon (" + obj.name + ") na cena " + cena.name + " alterado para: " + ativar);
                    }
                }
            }
        }
    }
}