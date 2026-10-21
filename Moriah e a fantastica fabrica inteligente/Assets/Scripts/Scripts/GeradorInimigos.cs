using UnityEngine;

public class GeradorInimigos : MonoBehaviour
{
    public GameObject[] prefabsInimigos;
    public Transform pontoNascimento;
    public Transform nucleo;
    public Vector3 range;

    public float intervalo = 3f;
    public int quantidadePorOnda = 5;

    private float contador;

    private void Start()
    {
        // InvokeRepeating("CriarInimigo", 0, intervalo);
    }

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

        Vector3 pos = this.transform.position + Vector3.Lerp(-range, range, Random.value);

        GameObject inimigo = Instantiate(
            prefab,
            pos,
            Quaternion.identity
        );

        InimigoCibernetico componente = inimigo.GetComponent<InimigoCibernetico>();

        if (componente != null)
            componente.alvo = nucleo;
    }
}
