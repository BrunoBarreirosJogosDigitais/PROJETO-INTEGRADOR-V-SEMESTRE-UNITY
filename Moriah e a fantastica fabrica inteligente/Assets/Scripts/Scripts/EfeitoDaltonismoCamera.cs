using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EfeitoDaltonismoCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GerenciadorDaltonismo.Instancia == null) return;

        GerenciadorDaltonismo.Instancia.AoMudarModo += AtualizarEstado;
        AtualizarEstado(GerenciadorDaltonismo.Instancia.ModoAtual);       
    }

    private void OnDestroy()
    {
        if (GerenciadorDaltonismo.Instancia != null)
            GerenciadorDaltonismo.Instancia.AoMudarModo -= AtualizarEstado;
    }

    private void AtualizarEstado(ModoDaltonismo modo)
    {
        enabled = (modo != ModoDaltonismo.Nenhum);    
    }

    private void OnRenderImage(RenderTexture origem, RenderTexture destino)
    {
        var g = GerenciadorDaltonismo.Instancia;

        if (g == null || g.Material == null)
        {
            Graphics.Blit(origem, destino);
            return;
        }

        Graphics.Blit(origem, destino, g.Material);
    }
}
