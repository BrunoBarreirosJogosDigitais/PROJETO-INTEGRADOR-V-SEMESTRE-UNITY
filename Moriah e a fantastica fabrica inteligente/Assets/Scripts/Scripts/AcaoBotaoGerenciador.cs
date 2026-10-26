using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AcaoBotaoGerenciador : MonoBehaviour
{
    public enum AcaoBotao { creditos, VoltarMenu, Jogar, Quit, AlternaAltoContraste}
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
            }
        });
    }

}
