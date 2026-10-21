using UnityEngine;

public class ProjetilAntivirus : MonoBehaviour
{
    public float velocidadeProjetil = 8f;
    private Transform alvo;
    private float dano;


    public void ConfigurarAlvo(Transform novoAlvo, float danoRecebido)
    {
        alvo = novoAlvo;
        dano = danoRecebido;
    }

    // Update is called once per frame
    void Update()
    {
        if(alvo == null)
        {
            Destroy(alvo);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, alvo.position, velocidadeProjetil * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigos"))
        {
            collision.SendMessage("ReceberDano", dano, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
