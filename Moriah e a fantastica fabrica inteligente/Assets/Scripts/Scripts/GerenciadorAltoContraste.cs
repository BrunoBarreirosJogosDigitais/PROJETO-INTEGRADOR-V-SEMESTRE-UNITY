using UnityEngine;
using UnityEngine.UI;

public class GerenciadorAltoContraste : MonoBehaviour
{
    public static GerenciadorAltoContraste Instancia {  get; private set; }

    [Header("Configuração de Cores")]
    public Color corFundoNormal = new Color(0f, 0f, 0f, 0.4f);
    public Color corFundoAltoContraste = new Color(0f, 0f, 0f, 0.95f);

    [Header("Cores de Texto Opcionais")]
    public Color corTextoNormal = Color.white;
    public Color corTextoAltoContraste = Color.yellow;

    private bool altoContrasteAtivo = false;

    private void Awake()
    {
        if(Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AplicarAltoContraste(altoContrasteAtivo);
    }

    //Metodo chamado pelo botão de acessibilidade no menu de opções
    public void AlternarAltoContraste()
    {
        altoContrasteAtivo = !altoContrasteAtivo;
        AplicarAltoContraste(altoContrasteAtivo);

        Debug.Log($"[Acessibilidade] Modo de Alto Contraste {(altoContrasteAtivo ? "Ativado" : "Desativado")}");
    }

    public void AplicarAltoContraste(bool ativar)
    {
        altoContrasteAtivo = ativar;

        PainelContraste[] paineis = FindObjectsByType<PainelContraste>(FindObjectsSortMode.None);
        foreach(var painel in paineis) 
        {
            painel.AtualizarVisual(altoContrasteAtivo);
        }
    }

    public bool EstaAtivo() => altoContrasteAtivo;
}
