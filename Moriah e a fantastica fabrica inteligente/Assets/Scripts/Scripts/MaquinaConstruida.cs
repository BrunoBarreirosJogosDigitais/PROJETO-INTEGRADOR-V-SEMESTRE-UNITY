using UnityEngine;

public class MaquinaConstruida : MonoBehaviour
{
    public DadosMaquina dados;
    private bool registrada;

    public void Inicializar(DadosMaquina novosDados)
    {
        dados = novosDados;

        if (dados != null && GerenciadorJogo.Instancia != null)
        {
            GerenciadorJogo.Instancia.AdicionarProducao(dados.producaoPorSegundo);
            GerenciadorJogo.Instancia.AdicionarManutencao(dados.manutencaoPorSegundo);
            GerenciadorJogo.Instancia.AumentarAmeaca(dados.aumentoAmeaca);
            registrada = true;
        }
    }
    private void OnDestroy()
    {
        if (!registrada || dados == null || GerenciadorJogo.Instancia == null) return;

        GerenciadorJogo.Instancia.RemoverProducao(dados.producaoPorSegundo);
        GerenciadorJogo.Instancia.RemoverManutencao(dados.manutencaoPorSegundo);
    }
}
