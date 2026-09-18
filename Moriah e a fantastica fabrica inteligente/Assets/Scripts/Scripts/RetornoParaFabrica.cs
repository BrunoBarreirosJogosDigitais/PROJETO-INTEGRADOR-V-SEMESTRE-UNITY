using UnityEngine;
using UnityEngine.SceneManagement;

public class RetornoParaFabrica : MonoBehaviour
{
    public string cenaFabrica = "Fabrica";

    public void Voltar()
    {
        if (GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.ameacaCibernetica = 0f;
            GerenciadorJogo.Instancia.CurarPlaca(
                GerenciadorJogo.Instancia.vidaPlacaMaxima
            );
            GerenciadorJogo.Instancia.RecuperarPontosServidor(
                GerenciadorJogo.Instancia.pontosServidorMaximos
            );
        }

        SceneManager.LoadScene(cenaFabrica);
    }
}
