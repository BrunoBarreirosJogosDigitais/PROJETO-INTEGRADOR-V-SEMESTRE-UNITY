using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AcaoBotaoGerenciador : MonoBehaviour
{
    public enum AcaoBotao 
    { 
        creditos, VoltarMenu, Jogar, Quit, AlternaAltoContraste, 
        Protanopia, Deuteranopia, Tritanopia, DaltonismoNormal
    }
    public AcaoBotao acaoDesejada;

    void Start()
    {
        Button botao = GetComponent<Button>();

        botao.onClick.AddListener(() =>
        {
            switch (acaoDesejada)
            {
                case AcaoBotao.creditos:
                    if (GerenciadorJogo.Instancia != null)
                        GerenciadorJogo.Instancia.Creditos();
                    break;

                case AcaoBotao.VoltarMenu:
                    if (GerenciadorJogo.Instancia != null)
                        GerenciadorJogo.Instancia.VoltarAoMenu();
                    break;

                case AcaoBotao.Jogar:
                    if (GerenciadorJogo.Instancia != null)
                        GerenciadorJogo.Instancia.IniciarJogo();
                    break;

                case AcaoBotao.Quit:
                    if (GerenciadorJogo.Instancia != null)
                        GerenciadorJogo.Instancia.QuitGame();
                    break;

                case AcaoBotao.AlternaAltoContraste:
                    if (GerenciadorAltoContraste.Instancia != null)
                        GerenciadorAltoContraste.Instancia.AlternarAltoContraste();
                    break;

                case AcaoBotao.Protanopia:
                    if (GerenciadorDaltonismo.Instancia != null)
                        GerenciadorDaltonismo.Instancia.AlternarModo(ModoDaltonismo.Protanopia);
                    break;

                case AcaoBotao.Deuteranopia:
                    if (GerenciadorDaltonismo.Instancia != null)
                        GerenciadorDaltonismo.Instancia.AlternarModo(ModoDaltonismo.Deuteranopia);
                    break;

                case AcaoBotao.Tritanopia:
                    if (GerenciadorDaltonismo.Instancia != null)
                        GerenciadorDaltonismo.Instancia.AlternarModo(ModoDaltonismo.Tritanopia);
                    break;

                case AcaoBotao.DaltonismoNormal:
                    if (GerenciadorDaltonismo.Instancia != null)
                        GerenciadorDaltonismo.Instancia.DefinirModo(ModoDaltonismo.Nenhum);
                    break;
            }
        });
    }

}
