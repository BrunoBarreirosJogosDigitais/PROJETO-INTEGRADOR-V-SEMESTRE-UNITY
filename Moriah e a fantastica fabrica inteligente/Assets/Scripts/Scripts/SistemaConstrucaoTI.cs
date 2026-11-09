using UnityEngine;

public class SistemaConstrucaoTI : MonoBehaviour
{
    [Header("Configuração Local")]
    public Transform pontoDeInstanciacao;
    public GameObject constructoInstalado;

    [Header("Constructo de TI Selecionado")]
    public DadosMelhoriaTI dadosTIParaEsteLocal;
    public GameObject prefabTIParaEsteLocal;

    private void OnMouseDown()
    {
        if (GerenciadorJogo.Instancia == null) return;
        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        if(constructoInstalado != null)
        {
            Debug.Log("Este local do Setor de TI já possui um constructo ativo");
            return;
        }

        if(dadosTIParaEsteLocal == null || prefabTIParaEsteLocal == null)
        {
            Debug.LogWarning("Nenhum constructi de TI doi configurado neste ponto de contrução");
            return;
        }

        //Valida e gasta o dinheiro através do Gerenciador
        if (!GerenciadorJogo.Instancia.GastarDinheiro(dadosTIParaEsteLocal.preco)) return;

        Vector3 posicaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.position : transform.position;
        Quaternion rotacaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.rotation : transform.rotation;

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
