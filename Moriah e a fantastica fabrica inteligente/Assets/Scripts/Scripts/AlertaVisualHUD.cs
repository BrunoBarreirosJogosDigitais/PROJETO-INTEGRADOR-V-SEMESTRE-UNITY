using System.Collections;
using UnityEngine;

public class AlertaVisualHUD : MonoBehaviour
{
    [Header("Configuração do Alerta Visual")]
    public GameObject painelAlertaVisual;
    public float duracaoPiscar = 2f;


    private void OnEnable()
    {
        //GerenciadorJogo.OnAlertaAtaqueDisparado += AcionarAlertaVisual;
    }

    private void OnDisable()
    {
        //GerenciadorJogo.OnAlertaAtaqueDisparado -= AcionarAlertaVisual;
    }

    void Start()
    {
        if(painelAlertaVisual != null)
        {
            painelAlertaVisual.SetActive(false);
        }
    }

    void AcionarAlertaVisual()
    {
        if(painelAlertaVisual != null)
        {
            StopAllCoroutines();
            StartCoroutine(PiscarAlertaRoutine());
        }
    }

    IEnumerator PiscarAlertaRoutine()
    {
        float tempoRestante = duracaoPiscar;
        bool estadoLigado = true;

        while(tempoRestante > 0)
        {
            estadoLigado = !estadoLigado;
            painelAlertaVisual.SetActive(estadoLigado);
            yield return new WaitForSeconds(0.25f);
            tempoRestante -= 0.25f;
        }

        painelAlertaVisual.SetActive(false);
    }
}
