using UnityEngine;

public class Firewall : MonoBehaviour
{
    [Header("Objeto Scriptavel da Carta")]
    public CartaDefensiva dadosDaCartas;

    [Header("Atributos em Tempo de Execução")]
    public float vidaAtual;
    private float danoPorSegundo;

    void Start()
    {
        if(dadosDaCartas != null)
        {
            vidaAtual = dadosDaCartas.vidaBloqueio;
            danoPorSegundo = dadosDaCartas.dano;
        }
        else
        {
            Debug.LogWarning("[Firewall] Nenhuma CartaDefensiva foi associada no Inspector");
            vidaAtual = 150f;
            danoPorSegundo = dadosDaCartas.dano;
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        vidaAtual -= quantidadeDano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, (dadosDaCartas != null ? dadosDaCartas.vidaBloqueio : 150f));

        Debug.Log($"[Firewall] Barreira atacada! Vida restante: {vidaAtual}");

        if(vidaAtual <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigos"))
        {
            ProcessarDanoContato(collision.gameObject);
        }
    }

    private void ProcessarDanoContato(GameObject objetoInimigo)
    {
 
        objetoInimigo.SendMessage("ReceberDano", danoPorSegundo * Time.deltaTime, SendMessageOptions.DontRequireReceiver);
    }
}
