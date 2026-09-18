using UnityEngine;
using TMPro;

public class InterfaceDefesa : MonoBehaviour
{
    public TMP_Text textoServidor;
    public TMP_Text textoVida;
    public TMP_Text textoOnda;
    public GameObject painelDerrota;
    public GameObject painelVitoria;

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

        if (jogo.vidaPlaca <= 0 && painelDerrota != null)
            painelDerrota.SetActive(true);
    }
}
