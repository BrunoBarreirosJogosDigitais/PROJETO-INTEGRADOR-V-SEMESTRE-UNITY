using UnityEngine;

public class Antivirus : MonoBehaviour
{
    [Header("Objeto Scriptável da Carta")]
    public CartaDefensiva dadosDaCarta;

    [Header("Atributos em Tempo de Execução")]
    public float danoDisparo;
    public float alcance;
    public float tempoRecarga;

    private float cronogramDisparo = 0f;

    void Start()
    {
        if (dadosDaCarta != null) 
        {
            danoDisparo = dadosDaCarta.dano;
            alcance = dadosDaCarta.alcance;
            tempoRecarga = dadosDaCarta.tempoRecarga;
        }
        else
        {
            Debug.LogWarning("[Antivírus] Nenhum CartaDefensiva foi associada no Inspector");
            danoDisparo = 25f;
            alcance = 3f;
            tempoRecarga = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        cronogramDisparo += Time.deltaTime;

        if(cronogramDisparo >= tempoRecarga)
        {
            AtacarInimigoNoAlcance();
            cronogramDisparo = 0f;
        }
    }

    void AtacarInimigoNoAlcance()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigos");
        GameObject inimigoMaisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector2.Distance(transform.position, inimigo.transform.position);
            if (distancia <= alcance && distancia < menorDistancia)
            {
                menorDistancia = distancia;
                inimigoMaisProximo = inimigo;
            }
        }

        if (inimigoMaisProximo != null)
        {
            Debug.Log($"[Antivirus] Disparando contra {inimigoMaisProximo.name} causando {danoDisparo} de dano!");

            inimigoMaisProximo.SendMessage("ReceberDano", danoDisparo, SendMessageOptions.DontRequireReceiver);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float raioVisual = (dadosDaCarta != null) ? dadosDaCarta.alcance : alcance;
        Gizmos.DrawWireSphere(transform.position, raioVisual);  
    }
}
