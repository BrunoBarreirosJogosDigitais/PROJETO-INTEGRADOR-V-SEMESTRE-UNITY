using UnityEngine;

public class SistemaConstrucao : MonoBehaviour
{
    [Header("Referencias")]
    public Camera cameraPrincipal;
    public LayerMask camadaChao;
    public Transform pastaMaquinas;

    [Header("Maquina selecionada")]
    public DadosMaquina maquinaSelecionada;
    public GameObject prefabMaquina;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConstruirNoLocalDoMouse();
        }
    }
    public void SelecionarMaquina(DadosMaquina dados)
    {
        maquinaSelecionada = dados;
    }

    public void SelecionarPrefab(GameObject prefab)
    {
        prefabMaquina = prefab;
    }

    private void ConstruirNoLocalDoMouse()
    {
        if (maquinaSelecionada == null || prefabMaquina == null) return;

        Ray raio = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(raio, out RaycastHit impacto, 500f, camadaChao))
            return;

        if (!GerenciadorJogo.Instancia.GastarDinheiro(maquinaSelecionada.preco))
            return;

        Transform pasta = pastaMaquinas != null ? pastaMaquinas : transform;

        GameObject nova = Instantiate(
            prefabMaquina,
            impacto.point,
            Quaternion.identity,
            pasta
        );

        MaquinaConstruida maquina = nova.GetComponent<MaquinaConstruida>();

        if (maquina != null)
        {
            maquina.Inicializar(maquinaSelecionada);
        }
    }
}
