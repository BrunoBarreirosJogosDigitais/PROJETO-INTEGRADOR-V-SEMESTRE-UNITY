using UnityEngine;

public class InimigoCibernetico : MonoBehaviour
{
    public enum TipoInimigo
    {
        Worm,
        Ransomware,
        DDoSBot
    }

    public TipoInimigo tipo = TipoInimigo.Worm;
    public float vida = 50f;
    public float velocidade = 2f;
    public int danoNoNucleo = 10;

    public Transform alvo;

    private void Update()
    {
        if (alvo == null) return;

        Vector3 direcao = (alvo.position - transform.position).normalized;
        transform.position += direcao * velocidade * Time.deltaTime;

        if (Vector3.Distance(transform.position, alvo.position) < 0.6f)
        {
            GerenciadorJogo.Instancia.ReceberDanoPlaca(danoNoNucleo);
            Destroy(gameObject);
        }
    }

    public void ReceberDano(float dano)
    {
        vida -= dano;

        if (vida <= 0f)
            Destroy(gameObject);
    }
}
