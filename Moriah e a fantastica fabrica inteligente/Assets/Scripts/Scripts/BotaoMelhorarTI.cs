using UnityEngine;

public class BotaoMelhorarTI : MonoBehaviour
{
    public void Melhorar()
    {
        if (GerenciadorJogo.Instancia != null)
            GerenciadorJogo.Instancia.MelhorarTI();
    }
}
