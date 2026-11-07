using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CartaDefensiva : MonoBehaviour, IPointerClickHandler
{
    [Header("Dados da Carta")]
    public CartaDefensiva cartaDados;

    [Header("Refer�ncias de Texto da HUD")]
    public Text textoNomeCarta;
    public Text textoDescricao;
    public Text textoCusto;
    public Text textoDano;
    public Text textoVida;
    public Text textoAlcance;

    [Header("Componentes Visuais")]
    public Image imagemIcone;
    public GameObject painelDetalhes;

    [Header("Configura��o de Duplo Clique")]
    private float tempoUltimoClique = 0f;
    private float limiteDuploClique = 0.3f;
    private bool cartaExpandida = false;

    private void Start()
    {
        if (GerenciadorJogo.Instancia != null && GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.TowerDefense2D)
        {
            // Opcional: desativa o componente se estiver rodando fora da hora
            // gameObject.SetActive(false); 
        }

        ConfigurarVisualCarta();
        AlternaModoVisualizacao(false);
    }

    public void ConfigurarVisualCarta()
    {
        if (cartaDados == null) return;

        if (textoNomeCarta != null) textoNomeCarta.text = cartaDados.nomeCarta;
        if (textoDescricao != null) textoDescricao.text = cartaDados.descricao;
        if (textoCusto != null) textoCusto.text = "Custo: " + cartaDados.custoServidor;
        if (textoDano != null) textoDano.text = "Dano: " + cartaDados.dano;
        if (textoVida != null) textoVida.text = "Bloqueio: " + cartaDados.vidaBloqueio;
        if (textoAlcance != null) textoAlcance.text = "Alcance: " + cartaDados.alcance;
        if (imagemIcone != null && cartaDados.imagemCarta != null)
        {
            imagemIcone.sprite = cartaDados.imagemCarta;
            imagemIcone.preserveAspect = true;
        }
        else
        {
            Debug.LogWarning("Imagem da carta n�o atribu�da para: " + cartaDados.nomeCarta);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            float tempoAtual = Time.unscaledTime;

            if(tempoAtual - tempoUltimoClique <= limiteDuploClique)
            {
                AlternarExpansaoCarta();
                tempoUltimoClique = 0f;
            }
            else
            {
                tempoUltimoClique = tempoAtual;
            }
        }
    }

    private void AlternarExpansaoCarta()
    {
        cartaExpandida = !cartaExpandida;
        AlternaModoVisualizacao(cartaExpandida);
    }

    private void AlternaModoVisualizacao(bool expandido)
    {
        if(painelDetalhes != null)
        {
            painelDetalhes.SetActive(expandido);
        }
    }
}
