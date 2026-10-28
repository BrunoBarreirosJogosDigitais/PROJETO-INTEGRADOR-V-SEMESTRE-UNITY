using UnityEngine;

public class TiroInimigo : MonoBehaviour
{
    public float velocidade = 6f;
    public float tempoDeVida = 5f;
    public int danoDoTiro = 5;

    private Vector3 direcao;

    public void Iniciar(Vector3 dir)
    {
        direcao = dir;

        // Isso gira o sprite para apontar na direção do movimento. Totalmente opcional, mas dá um efeito legal.
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo);

        Destroy(gameObject, tempoDeVida); // Se tiver bala perdida, com o tempo ela é destruída pra não ficar ocupando memória.
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direcao * velocidade * Time.deltaTime, Space.World);    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.StartsWith("PlacaMae"))
        {
            if(GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoDoTiro);
            }

            Destroy(this.gameObject);
        }
    }
}
