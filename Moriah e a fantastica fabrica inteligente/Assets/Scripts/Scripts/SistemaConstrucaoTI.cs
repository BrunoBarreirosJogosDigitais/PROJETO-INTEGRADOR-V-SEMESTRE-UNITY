using UnityEngine;

public class SistemaConstrucaoTI : MonoBehaviour
{
    [Header("Configuração Local")]
    public Transform pontoDeInstanciacao; //Referencia de posição/rotação onde o objeto vai nascer
    public GameObject constructoInstalado; // Armazena a referência da máquina/seervidor criado neste slot

    [Header("Constructo de TI Selecionado")]
    public DadosMelhoriaTI dadosTIParaEsteLocal; //Scriptable contendo preço, bônus e dados da máquina
    public GameObject prefabTIParaEsteLocal; // O prefab 3D físico que será instanciado no mapa

    private void OnMouseDown()
    {
        //Validação 1: Garante que o Gerenciadorea Globais existe na cena
        if (GerenciadorJogo.Instancia == null) return;
        // Validação 2: Impede a construção se o jogador não estiver no modo de gestão 3D
        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        //Validação 3: Verifica se este slot já possui uma máquina construída
        if(constructoInstalado != null)
        {
            Debug.Log("Este local do Setor de TI já possui um constructo ativo");
            return;
        }

        //Validação 4:Assegura que os dados e o prefab da máquina foram configurados no Inspector
        if (dadosTIParaEsteLocal == null || prefabTIParaEsteLocal == null)
        {
            Debug.LogWarning("Nenhum constructi de TI doi configurado neste ponto de contrução");
            return;
        }

        //Valida e gasta o dinheiro através do Gerenciador
        // Validação 5: Valida se o jogador tem dinheiro suficiente e desconta o valor via Gerenciador
        if (!GerenciadorJogo.Instancia.GastarDinheiro(dadosTIParaEsteLocal.preco)) return;

        //Define a posição e rotação exata de spawn com base no ponto de referência ou no próprio objeto
        Vector3 posicaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.position : transform.position;
        Quaternion rotacaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.rotation : transform.rotation;

        //Instancia o prefab 3d da máquina de TI no mundo do jogo
        constructoInstalado = Instantiate(
                prefabTIParaEsteLocal,
                posicaoSpawn,
                rotacaoSpawn
        );

        constructoInstalado.transform.localScale = Vector3.one;

        GerenciadorJogo.Instancia.AdicionarProducao(dadosTIParaEsteLocal.producaoPorSegundo);

        GerenciadorJogo.Instancia.AdicionarGeracaoEnergia(dadosTIParaEsteLocal.geracaoPontosServidor);

        GerenciadorJogo.Instancia.AtivarEfeitoTI(dadosTIParaEsteLocal.tipoBonus, dadosTIParaEsteLocal.valorBonus);

        Debug.Log("Constructo do Setor de TI erguido com sucesso: " + dadosTIParaEsteLocal.nomeConstructo);
    }
}
