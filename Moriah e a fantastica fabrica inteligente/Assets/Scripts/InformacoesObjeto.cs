using UnityEngine;

public class InformacoesObjeto : MonoBehaviour
{
    [Header("Opções de ScriptableObject (Preencha apenas UM)")]
    public DadosMaquina dadosMaquina;
    public DadosMelhoriaRH dadosRH;
    public DadosMelhoriaTI dadosTI;

    [Header("Informações Manuais de Apoio")]
    public string local = "Setor de Produção";
    public Sprite imagemItem;

    [Header("Gerenciador do Painel")]
    public GerenciadorPainelInformacoes gerenciadorPainel;

    private void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Player"))
        {
            if (gerenciadorPainel == null) return;

            string nome = "";
            float valor = 0f;
            string descricao = "";

            // Verifica qual ScriptableObject foi atribuído ao objeto
            if (dadosMaquina != null)
            {
                nome = dadosMaquina.nomeMaquina;
                valor = dadosMaquina.preco;
                descricao = dadosMaquina.descricao;
            }
            else if (dadosRH != null)
            {
                nome = dadosRH.nomeMelhoria;
                valor = dadosRH.preco;
                descricao = dadosRH.descricao;
            }
            else if (dadosTI != null)
            {
                nome = dadosTI.nomeConstructo;
                valor = dadosTI.preco;
                descricao = dadosTI.descricao;
            }
            else
            {
                Debug.LogWarning($"[InformacoesObjeto] Nenhum ScriptableObject atribuído em {gameObject.name}!");
                return;
            }

            // Envia os dados para o painel de UI exibir
            gerenciadorPainel.MostrarInformacoes(
                this,
                nome,
                local,
                valor,
                descricao,
                imagemItem
            );
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