using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class PainelContraste : MonoBehaviour
{
    private Image imageFundo;
    private Text[] textosInternos;


    private void Awake()
    {
        imageFundo = GetComponent<Image>();

        textosInternos = GetComponentsInChildren<Text>();
    }

    void Start()
    {
        if(GerenciadorAltoContraste.Instancia != null)
        {
            AtualizarVisual(GerenciadorAltoContraste.Instancia.EstaAtivo());
        }
    }

    public void AtualizarVisual(bool altoContrasteAtivo)
    {
        if (imageFundo == null) return;

        if (altoContrasteAtivo)
        {
            imageFundo.color = GerenciadorAltoContraste.Instancia.corFundoAltoContraste;

            foreach (var txt in textosInternos)
            {
                txt.color = GerenciadorAltoContraste.Instancia.corTextoAltoContraste;
            }
        }
        else
        {
            imageFundo.color = GerenciadorAltoContraste.Instancia.corFundoNormal;

            foreach (var txt in textosInternos)
            {
                txt.color = GerenciadorAltoContraste.Instancia.corTextoNormal;
            }
        }
    }

}
