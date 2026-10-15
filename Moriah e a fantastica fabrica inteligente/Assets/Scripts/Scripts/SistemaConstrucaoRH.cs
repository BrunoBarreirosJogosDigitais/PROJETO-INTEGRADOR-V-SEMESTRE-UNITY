using UnityEngine;

public class SistemaConstrucaoRH : MonoBehaviour
{
    [Header("Configuração do Local")]
    public Transform pontoDeInstanciacao;
    public GameObject melhoriaInstalada;

    [Header("Melhoria de RH Selecionada")]
    public DadosMelhoriaRH dadosMelhoriaParaEsteLocal;
    public GameObject prefabMelhoriaParaEsteLocal;

    private void OnMouseDown()
    {

        if (GerenciadorJogo.Instancia == null) return;
        if (GerenciadorJogo.Instancia.estadoAtual != GerenciadorJogo.EstadoJogo.Tycoon3D) return;

        if(melhoriaInstalada != null)
        {
            Debug.Log("Este local do Setor de RH já possui melhoria ativa");
            return;
        }

        if(dadosMelhoriaParaEsteLocal == null || prefabMelhoriaParaEsteLocal == null)
        {
            Debug.LogWarning("Nenhuma melhoria de RH foi configurada neste ponto de construção.");
            return;
        }

        if (!GerenciadorJogo.Instancia.GastarDinheiro(dadosMelhoriaParaEsteLocal.preco)) return;

        Vector3 posicaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.position : transform.position;
        Quaternion rotacaoSpawn = pontoDeInstanciacao != null ? pontoDeInstanciacao.rotation : transform.rotation;

        melhoriaInstalada = Instantiate(
            prefabMelhoriaParaEsteLocal,
            posicaoSpawn, rotacaoSpawn,
            transform
        );

        GerenciadorJogo.Instancia.AdicionarProducao(dadosMelhoriaParaEsteLocal.producaoPorSegundo);

        GerenciadorJogo.Instancia.AtivarEfeitoRH(dadosMelhoriaParaEsteLocal.tipoBonus);

        Debug.Log("Melhoria do Setor de RH erguida com sucesso: " + dadosMelhoriaParaEsteLocal.nomeMelhoria);
    }

}
