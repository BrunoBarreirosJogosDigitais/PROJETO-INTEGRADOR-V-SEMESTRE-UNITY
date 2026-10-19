using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CenaVitoria : MonoBehaviour
{
    [Header("Elementos de Texto (UI)")]
    public Text textoVitoria1;
    public Text textoVitoria2;

    [Header("Configuração de Cenas")]
    public string nomeCenaFabrica = "Fase";
    public string nomeCanvasTycoon = "CanvasTycoon";
    public string nomeCenaVitoria = "Vitoria";

    void Start()
    {
        // Esconde o canvas da fábrica enquanto a tela de vitória está aberta
        AlternarVisibilidadeTycoon(false);
    }

    public void VoltarParaFabrica()
    {
        AlternarVisibilidadeTycoon(true);

        if (GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.AlternaModoJogo(GerenciadorJogo.EstadoJogo.Tycoon3D);
        }

        SceneManager.UnloadSceneAsync(nomeCenaVitoria);
    }

    private void AlternarVisibilidadeTycoon(bool ativar)
    {
        Scene cenaFabrica = SceneManager.GetSceneByName(nomeCenaFabrica);
        if (cenaFabrica.IsValid() && cenaFabrica.isLoaded)
        {
            foreach (GameObject obj in cenaFabrica.GetRootGameObjects())
            {
                if (obj.name == nomeCanvasTycoon)
                {
                    obj.SetActive(ativar);
                    Debug.Log("Canvas do Tycoon (" + obj.name + ") foi alterado para: " + ativar);
                    return;
                }
            }
        }
    }
}