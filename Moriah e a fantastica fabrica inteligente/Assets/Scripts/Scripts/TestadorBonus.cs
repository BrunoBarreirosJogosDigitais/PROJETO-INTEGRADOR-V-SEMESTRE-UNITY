using UnityEngine;

public class TestadorBonus : MonoBehaviour
{
    [Tooltip("Pressione T para imprimir os valores atuais no Console")]

    private void Update()
    {
        // Pressione a tecla 'T' durante o jogo para verificar os status atuais
        if (Input.GetKeyDown(KeyCode.T))
        {
            ImprimirStatusAtual();
        }
    }

    public void ImprimirStatusAtual()
    {
        if (GerenciadorJogo.Instancia == null)
        {
            Debug.LogWarning("[TESTE] GerenciadorJogo não encontrado na cena!");
            return;
        }

        Debug.Log("================== [RELATÓRIO DE TESTE DE BÔNUS] ==================");

        // Economia Geral
        Debug.Log($"[Economia] Dinheiro Atual: R$ {GerenciadorJogo.Instancia.dinheiro}");
        Debug.Log($"[Economia] Produção por Segundo Base: R$ {GerenciadorJogo.Instancia.producaoPorSegundo}/s");
        Debug.Log($"[Economia] Manutenção por Segundo: R$ {GerenciadorJogo.Instancia.manutencaoPorSegundo}/s");

        // Segurança e Ameaça
        Debug.Log($"[Segurança] Nível de TI: {GerenciadorJogo.Instancia.nivelTI}");
        Debug.Log($"[Segurança] Ameaça Cibernética: {GerenciadorJogo.Instancia.ameacaCibernetica}%");

        // Tower Defense / Placa-Mãe
        Debug.Log($"[Tower Defense] Vida da Placa-Mãe: {GerenciadorJogo.Instancia.vidaPlaca} / {GerenciadorJogo.Instancia.vidaPlacaMaxima} HP");
        Debug.Log($"[Tower Defense] Geração de Energia/Servidor: {GerenciadorJogo.Instancia.geracaoEnergiaPorSegundo}/s");

        // Status das Flags de RH (Área 3)
        Debug.Log("--- BÔNUS DO SETOR DE RH (ÁREA 3) ---");
        Debug.Log($"[RH] Simulador de Phishing Ativo? {GerenciadorJogo.Instancia.simuladorPhishingAtivo}");
        Debug.Log($"[RH] Central de Ergonomia Ativa (+10% prod.)? {GerenciadorJogo.Instancia.centralErgonomiaAtiva}");
        Debug.Log($"[RH] Academia Corporativa (DDoS) Ativa? {GerenciadorJogo.Instancia.academiaDDoSAtiva}");

        // Status das Flags de TI (Área 2)
        Debug.Log("--- BÔNUS DO SETOR DE TI (ÁREA 2) ---");
        Debug.Log($"[TI] Criptografia Quântica Ativa? {GerenciadorJogo.Instancia.criptografiaAtivado}");
        Debug.Log($"[TI] Supercomputador Neural Ativo? {GerenciadorJogo.Instancia.supercomputadorAtivado}");

        Debug.Log("==================================================================");
    }
}