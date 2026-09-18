using UnityEngine;

[System.Serializable]
public class DadosSalvos
{
    public float dinheiro;
    public float producao;
    public float manutencao;
    public float ameaca;
    public int nivelTI;
}

public class SalvamentoJogo : MonoBehaviour
{
    private const string Chave = "MORIAH_SALVAMENTO";

    public void Salvar()
    {
        if (GerenciadorJogo.Instancia == null) return;

        var jogo = GerenciadorJogo.Instancia;

        DadosSalvos dados = new DadosSalvos
        {
            dinheiro = jogo.dinheiro,
            producao = jogo.producaoPorSegundo,
            manutencao = jogo.manutencaoPorSegundo,
            ameaca = jogo.ameacaCibernetica,
            nivelTI = jogo.nivelTI
        };

        string json = JsonUtility.ToJson(dados);
        PlayerPrefs.SetString(Chave, json);
        PlayerPrefs.Save();
    }

    public void Carregar()
    {
        if (!PlayerPrefs.HasKey(Chave) || GerenciadorJogo.Instancia == null)
            return;

        string json = PlayerPrefs.GetString(Chave);
        DadosSalvos dados = JsonUtility.FromJson<DadosSalvos>(json);

        var jogo = GerenciadorJogo.Instancia;

        jogo.dinheiro = dados.dinheiro;
        jogo.producaoPorSegundo = dados.producao;
        jogo.manutencaoPorSegundo = dados.manutencao;
        jogo.ameacaCibernetica = dados.ameaca;
        jogo.nivelTI = dados.nivelTI;
    }
}
