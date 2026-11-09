using UnityEngine;

public class AreasDesbloqueaveis : MonoBehaviour
{
    [Header("Desbloqueio de �reas")]
    //private GerenciadorJogo jogo;
    public GameObject porta;
    public float valorParaDesbloqueio;

    [Header("Estado")]
    public bool areaDesbloqueada = false;

    [Header("Painel de Proximidade")]
    public InformacoesObjeto informacoesDaPorta;

    void Start()
    {
        if(porta != null && !areaDesbloqueada)
            porta.SetActive(true);

    }


    private void OnMouseDown()
    {
        if (GerenciadorJogo.Instancia == null) return;
        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        //Se a �rea j� foi desbloqueada, n�o faz nada
        if (areaDesbloqueada) return;

        bool compraRealizada = GerenciadorJogo.Instancia.GastarDinheiro(valorParaDesbloqueio);

        if (compraRealizada)
        {
            areaDesbloqueada = true;

            if (informacoesDaPorta != null)
            {
                if(informacoesDaPorta.gerenciadorPainel != null)
                    informacoesDaPorta.gerenciadorPainel.EsconderInformacoes(informacoesDaPorta);

                informacoesDaPorta.gameObject.SetActive(false);
            }

            if(porta != null)
            {
                porta.SetActive(false); //Libera a passagem
            }

            Debug.Log("�rea desbloqueada com sucesso!");
        }
        else
        {
            Debug.Log("Fundos insuficientes para desbloquear est� �rea da f�brica");
        }
    }

}
