using UnityEngine;
using UnityEngine.SceneManagement;

public class InteracaoTerminal : MonoBehaviour
{
    public string cenaDefesa = "TerminalTI";

    private void OnMouseDown()
    {
        SceneManager.LoadScene(cenaDefesa);
    }
}
