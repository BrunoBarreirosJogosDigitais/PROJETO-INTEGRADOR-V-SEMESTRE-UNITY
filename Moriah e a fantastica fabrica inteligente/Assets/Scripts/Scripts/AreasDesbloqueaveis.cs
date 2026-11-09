using UnityEngine;

public class AreasDesbloqueaveis : MonoBehaviour
{
    [Header("Desbloqueio de Áreas")]
    //private GerenciadorJogo jogo;
    public GameObject porta;
    public float valorParaDesbloqueio;

    [Header("Estado")]
    public bool areaDesbloqueada = false;

    void Start()
    {
        if(porta != null && !areaDesbloqueada)
            porta.SetActive(true);

    }


    private void OnMouseDown()
    {
        if (GerenciadorJogo.Instancia == null) return;
        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        //Se a área já foi desbloqueada, não faz nada
        if (areaDesbloqueada) return;

        bool compraRealizada = GerenciadorJogo.Instancia.GastarDinheiro(valorParaDesbloqueio);

        if (compraRealizada)
        {
            areaDesbloqueada = true;

            if(porta != null)
            {
                porta.SetActive(false); //Libera a passagem
            }

            Debug.Log("Área desbloqueada com sucesso!");
        }
        else
        {
            Debug.Log("Fundos insuficientes para desbloquear está área da fábrica");
        }
    }

}
