using UnityEngine;
using UnityEngine.SceneManagement;

public class AlertaCibernetico : MonoBehaviour
{
    public float ameacaParaAtaque = 60f;
    public string nomeCenaDefesa = "TerminalTI";

    private bool ataqueDisparado;

    private void Update()
    {
        if (ataqueDisparado || GerenciadorJogo.Instancia == null) return;

        if (GerenciadorJogo.Instancia.ameacaCibernetica >= ameacaParaAtaque)
        {
            ataqueDisparado = true;
            SceneManager.LoadScene(nomeCenaDefesa);
        }
    }
}
