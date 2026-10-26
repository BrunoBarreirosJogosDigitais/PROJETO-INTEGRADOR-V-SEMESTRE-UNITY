using UnityEngine;

public class ControladorMenuPausa : MonoBehaviour
{
    [Header("Referência ao Painel de Pausa")]
    public GameObject painelPause;

    // Update is called once per frame
    void Update()
    {
        if(GerenciadorJogo.Instancia != null)
        {
            bool estaPausado = (GerenciadorJogo.Instancia.estadoAtual == GerenciadorJogo.EstadoJogo.Pausado);

            if(painelPause != null && painelPause.activeSelf != estaPausado )
            {
                painelPause.SetActive(estaPausado);
            }
        }
    }
}
