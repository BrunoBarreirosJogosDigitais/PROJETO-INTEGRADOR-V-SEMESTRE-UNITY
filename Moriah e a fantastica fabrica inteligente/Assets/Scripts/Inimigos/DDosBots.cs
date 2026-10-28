using System;
using System.Collections;
using UnityEngine;

public class DDosBots : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 1.5f;
    public float distanciaParaParar = 5f; // 0 = vai até o alvo, 1 = para a 1 unidade de distância do alvo, 2 = para a 2 unidades de distância do alvo, etc.

    [Header("Tiro")]
    public GameObject prefabTiro;
    public Transform pontoDeTiro; // opcional, se não for definido, o tiro será instanciado na posição do inimigo
    public int tirosPorRajada = 3;
    public float intervaloEntreTiros = 0.15f;
    public float intervaloEntreRajadas = 2f;

    private GameObject alvo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alvo = GameObject.Find("PlacaMae");
        StartCoroutine(LoopDeRajadas());
    }

    // Update is called once per frame
    void Update()
    {
        if (alvo == null) return;

        Vector3 direcao = alvo.transform.position - transform.position;
        direcao.z = 0f;

        if (direcao.magnitude > distanciaParaParar)
        {
            transform.Translate(direcao.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    IEnumerator LoopDeRajadas()
    {
        yield return new WaitForSeconds(intervaloEntreRajadas);

        while (true)
        {
            for (int i = 0; i < tirosPorRajada; i++)
            {
                Atirar();
                yield return new WaitForSeconds(intervaloEntreTiros);
            }

            yield return new WaitForSeconds(intervaloEntreRajadas);
        }
    }

    void Atirar()
    {
        if (alvo == null || prefabTiro == null) return;

        Vector3 origem = pontoDeTiro != null ? pontoDeTiro.position : transform.position;
        Vector3 direcao = (alvo.transform.position - origem);
        direcao.z = 0f;
        direcao.Normalize();

        GameObject tiro = Instantiate(prefabTiro, origem, Quaternion.identity);
        tiro.GetComponent<TiroInimigo>().Iniciar(direcao);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("PlacaMae"))
        {
            Destroy(gameObject);
        }
    }
}
