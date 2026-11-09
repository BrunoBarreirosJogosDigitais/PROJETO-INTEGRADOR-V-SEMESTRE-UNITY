
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class GerenciadorPainelInformacoes : MonoBehaviour
{
    [Header("Painel de Informacoes")]
    public GameObject painelInformacoes;

    [Header("Imagem do Objeto")]
    public Image imagemObjeto;

    [Header("Textos do Painel")]
    public Text textoNomeItem;
    public Text textoLocal;
    public Text textoValor;
    public Text textoDescricao;

    private InformacoesObjeto objetoAtual;

    private void Start()
    {
        if (painelInformacoes != null)
        {
            painelInformacoes.SetActive(false);
        }
    }

    public void MostrarInformacoes(
        InformacoesObjeto objeto,
        string nome,
        string local,
        float valor,
        string descricao,
        Sprite imagem)
    {
        if (painelInformacoes == null)
            return;

        objetoAtual = objeto;

        textoNomeItem.text = nome;
        textoLocal.text = "Local: " + local;

        textoValor.text = "Valor: " +
            valor.ToString(
                "C2",
                new CultureInfo("pt-BR")
            );

        textoDescricao.text = descricao;

        if (imagemObjeto != null)
        {
            imagemObjeto.sprite = imagem;
            imagemObjeto.enabled = imagem != null;

            if (imagem != null)
                imagemObjeto.preserveAspect = true;
        }

        painelInformacoes.SetActive(true);
    }

    public void EsconderInformacoes(
        InformacoesObjeto objeto)
    {
        if (objetoAtual != objeto)
            return;

        objetoAtual = null;

        if (painelInformacoes != null)
        {
            painelInformacoes.SetActive(false);
        }
    }
}
