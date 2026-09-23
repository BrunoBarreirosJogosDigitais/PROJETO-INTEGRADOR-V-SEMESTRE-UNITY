using Unity.VisualScripting;
using UnityEngine;

public class SistemaConstrucao : MonoBehaviour
{
    
    //[Header("Referencias")]
    //public Camera cameraPrincipal;
    //public LayerMask camadaChao;
    //public Transform pastaMaquinas;

    [Header("Configuração do Local")]
    public Transform pontoDeInstanciacao;
    public GameObject maquinaInstalada;

    [Header("Maquina selecionada")]
    //public DadosMaquina maquinaSelecionada;
    //public GameObject prefabMaquina;
    public DadosMaquina dadosMaquinaParaEsteLocal;
    public GameObject prefabMaquinaParaEsteLocal;

    private void OnMouseDown()
    {

        if (GerenciadorJogo.Instancia == null) return;
        if(GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        if(maquinaInstalada != null)
        {
            Debug.Log("Esta local já possui um constructo ativo");
            return;
        }

        if(dadosMaquinaParaEsteLocal == null || prefabMaquinaParaEsteLocal == null)
        {
            Debug.LogWarning("Nenhuma máquina foi configurada neste ponto de construção.");
            return;
        }

        if (!GerenciadorJogo.Instancia.GastarDinheiro(dadosMaquinaParaEsteLocal.preco)) return;

        Vector3 posicaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.position : transform.position;
        Quaternion rotacaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.rotation : transform.rotation;

        maquinaInstalada = Instantiate(
                prefabMaquinaParaEsteLocal,
                posicaoSpawn, rotacaoSpawn,
                transform
        );

        GerenciadorJogo.Instancia.AdicionarProducao(dadosMaquinaParaEsteLocal.producaoPorSegundo);
        GerenciadorJogo.Instancia.AdicionarManutencao(dadosMaquinaParaEsteLocal.manutencaoPorSegundo);

        MaquinaConstruida maquina = maquinaInstalada.GetComponent<MaquinaConstruida>();
        if(maquina != null)
        {
            maquina.Inicializar(dadosMaquinaParaEsteLocal);
        }

        Debug.Log("Constructo erguido com sucesso neste local");
    }

}
