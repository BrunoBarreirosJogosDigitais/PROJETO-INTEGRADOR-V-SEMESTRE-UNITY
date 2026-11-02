using UnityEngine;

public class InicializadorTerminal : MonoBehaviour
{
    [Header("Elementos Visuais para Esconder no Início")]
    public GameObject[] paineisVisuais; // Arraste aqui o Canvas e os elementos visuais do terminal

    private void Start()
    {
        // Garante que o jogo começa com a interface do terminal escondida
        EsconderInterfaceTerminal();
    }

    public void EsconderInterfaceTerminal()
    {
        foreach (GameObject painel in paineisVisuais)
        {
            if (painel != null) painel.SetActive(false);
        }
        Debug.Log("[TerminalTI] Cena carregada em segundo plano, interface ocultada com sucesso.");
    }

    public void MostrarInterfaceTerminal()
    {
        foreach (GameObject painel in paineisVisuais)
        {
            if (painel != null) painel.SetActive(true);
        }
    }
}