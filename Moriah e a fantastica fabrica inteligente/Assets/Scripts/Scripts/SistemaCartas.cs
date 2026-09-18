using UnityEngine;

public class SistemaCartas : MonoBehaviour
{
    public Camera cameraPrincipal;
    public LayerMask camadaChao;

    public CartaDefensiva cartaSelecionada;
    public GameObject prefabCarta;

    public void SelecionarCarta(CartaDefensiva carta)
    {
        cartaSelecionada = carta;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ColocarCarta();
    }

    private void ColocarCarta()
    {
        if (cartaSelecionada == null || prefabCarta == null) return;

        Ray raio = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(raio, out RaycastHit impacto, 500f, camadaChao))
            return;

        if (!GerenciadorJogo.Instancia.GastarPontosServidor(
            cartaSelecionada.custoServidor))
            return;

        GameObject defesa = Instantiate(
            prefabCarta,
            impacto.point,
            Quaternion.identity
        );

        TorreDefensiva torre = defesa.GetComponent<TorreDefensiva>();

        if (torre != null)
            torre.Inicializar(cartaSelecionada);
    }
}
