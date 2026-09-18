using UnityEngine;

public class GeradorInimigos : MonoBehaviour
{
    public GameObject[] prefabsInimigos;
    public Transform pontoNascimento;
    public Transform nucleo;

    public float intervalo = 3f;
    public int quantidadePorOnda = 5;

    private float contador;

    private void Update()
    {
        contador += Time.deltaTime;

        if (contador >= intervalo)
        {
            contador = 0f;
            CriarInimigo();
        }
    }

    private void CriarInimigo()
    {
        if (prefabsInimigos == null || prefabsInimigos.Length == 0) return;

        GameObject prefab = prefabsInimigos[
            Random.Range(0, prefabsInimigos.Length)
        ];

        GameObject inimigo = Instantiate(
            prefab,
            pontoNascimento.position,
            Quaternion.identity
        );

        InimigoCibernetico componente = inimigo.GetComponent<InimigoCibernetico>();

        if (componente != null)
            componente.alvo = nucleo;
    }
}
