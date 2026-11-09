
using UnityEngine;

public class InformacoesObjeto : MonoBehaviour
{
    [Header("Informacoes do Objeto")]
    public string nomeItem = "Braço Robótico";

    public string local = "Setor de Produção";

    public float valor = 5000f;

    [TextArea(3, 6)]
    public string descricao =
        "Máquina utilizada para automatizar a produção.";

    [Header("Imagem do Objeto")]
    public Sprite imagemItem;

    [Header("Gerenciador do Painel")]
    public GerenciadorPainelInformacoes gerenciadorPainel;

    private void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Player"))
        {
            if (gerenciadorPainel != null)
            {
                gerenciadorPainel.MostrarInformacoes(
                    this,
                    nomeItem,
                    local,
                    valor,
                    descricao,
                    imagemItem
                );
            }
        }
    }

    private void OnTriggerExit(Collider outro)
    {
        if (outro.CompareTag("Player"))
        {
            if (gerenciadorPainel != null)
            {
                gerenciadorPainel.EsconderInformacoes(this);
            }
        }
    }
}
