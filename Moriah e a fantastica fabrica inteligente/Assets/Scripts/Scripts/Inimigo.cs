using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Atributos do Inimigo")]
    public float speed;
    public float vidaMaxima = 50f;
    private float vidaAtual;
    public int danoAoNucleo = 15;

    public GameObject alvo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaAtual = vidaMaxima;

        if(alvo == null)
        {
            GameObject placaObj = GameObject.Find("PlacaMae");
            if(placaObj != null)
            {
                alvo = placaObj;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alvo == null) return;

        Vector3 pos = alvo.transform.position - this.transform.position;
        Vector3 velocidade = pos.normalized * speed;
        this.transform.Translate(velocidade * Time.deltaTime, Space.World);
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        Debug.Log($"[Inimigo] Worm Sofreu {quantidadeDano} de dano. Vida restante: {vidaAtual / vidaMaxima}");

        if(vidaAtual <= 0f)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        Debug.Log("[Inimigo] Worm foi destruido");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.StartsWith("PlacaMae"))
        {
            if(GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoAoNucleo);
            }
            Destroy(this.gameObject);
        }
    }
}
