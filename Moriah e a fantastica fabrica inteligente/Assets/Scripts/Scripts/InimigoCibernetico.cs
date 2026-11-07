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
        // Se o alvo estiver vazio por algum motivo, procura a Placa-Mãe na cena na hora
        if (alvo == null)
        {
            GameObject placa = GameObject.Find("PlacaMae");
            if (placa != null)
            {
                alvo = placa.transform;
            }
        }

        // Se encontrou o alvo, move-se em direção a ele
        if (alvo != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, alvo.position, velocidade * Time.deltaTime);

            // Opcional: checa se encostou no centro do círculo para causar dano
            float distancia = Vector3.Distance(transform.position, alvo.position);
            if (distancia <= 0.5f)
            {
                // Lógica de dano ao núcleo ou destruição do inimigo
                Destroy(gameObject);
            }
        }
    }

    public void ReceberDano(float dano)
    {
        vida -= dano;

        if (vida <= 0f)
            Destroy(gameObject);
    }
}
