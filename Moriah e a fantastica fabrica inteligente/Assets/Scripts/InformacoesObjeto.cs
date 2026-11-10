using UnityEngine;

public class InformacoesObjeto : MonoBehaviour
{
    [Header("Fonte de Dados (ScriptableObject)")]
    [Tooltip("Arraste aqui um ScriptableObject de Máquina, Melhoria RH ou TI.")]
    [SerializeField] private ScriptableObject dadosObjeto;

    [Header("Imagem Padrão (Caso não venha no SO)")]
    public Sprite imagemItemPadrao;

    [Header("Gerenciador do Painel")]
    public GerenciadorPainelInformacoes gerenciadorPainel;

    private void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Player"))
        {
            if (gerenciadorPainel != null && dadosObjeto != null)
            {
                ExtrairEEnviarInformacoes();
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

    private void ExtrairEEnviarInformacoes()
    {
        string nome = "Desconhecido";
        string local = "Setor Industrial";
        float valor = 0f;
        string descricao = "";
        Sprite imagem = imagemItemPadrao;

        // Verifica qual é o tipo de ScriptableObject atribuído e extrai os dados correspondentes
        if (dadosObjeto is DadosMaquina maquina)
        {
            nome = maquina.nomeMaquina;
            descricao = maquina.descricao;
            valor = maquina.preco;
            local = "Setor de Produção"; // Ou adicione um campo de local no seu SO se preferir
        }
        else if (dadosObjeto is DadosMelhoriaRH melhoriaRH)
        {
            nome = melhoriaRH.nomeMelhoria;
            descricao = melhoriaRH.descricao;
            valor = melhoriaRH.preco;
            local = "Recursos Humanos";
        }
        else if (dadosObjeto is DadosMelhoriaTI melhoriaTI)
        {
            nome = melhoriaTI.nomeConstructo;
            descricao = melhoriaTI.descricao;
            valor = melhoriaTI.preco;
            local = "Área de TI";
        }

        // Envia os dados extraídos para o gerenciador do painel
        gerenciadorPainel.MostrarInformacoes(
            this,
            nome,
            local,
            valor,
            descricao,
            imagem
        );
    }
}