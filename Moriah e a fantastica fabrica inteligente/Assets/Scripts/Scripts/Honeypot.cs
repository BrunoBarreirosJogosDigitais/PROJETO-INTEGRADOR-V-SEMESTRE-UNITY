using UnityEngine;

public class Honeypot : MonoBehaviour
{
    [Header("Objeto Scriptável da Carta")]
    public CartaDefensiva dadosDaCarta;

    [Header("Atributos em Tempo de Execução")]
    public float vidaAtual;
    public float danoExplosao = 70f;
    public float raioExplosao = 3.5f;
    public float fatorLentisao = 0.5f;
    public float duracaoLentidao = 3f;

    private bool jaExplodiu = false;

    void Start()
    {
        if(dadosDaCarta != null)
        {
            vidaAtual = dadosDaCarta.vidaBloqueio;
            danoExplosao = dadosDaCarta.dano;
        }
        else
        {
            Debug.LogWarning("[Honeypot] Nenhuma CartaDefensiva associada no Inspector");
            vidaAtual = 50f;
            danoExplosao = 70f;
        }
    }

    public void ReceberDano(float quantidadeDano)
    {
        if (jaExplodiu) return;

        vidaAtual -= quantidadeDano;
        float vidaMax = (dadosDaCarta != null) ? dadosDaCarta.vidaBloqueio : 50f;
        vidaAtual = Mathf.Clamp(vidaAtual, 0f, vidaMax);

        Debug.Log($"[Honeypot] Armadilha atacada! HP Restante: {vidaAtual}/{vidaMax}");

        if(vidaAtual <= 0f)
        {
            ExplodirEAplicarSlow();
        }
    }

    private void ExplodirEAplicarSlow()
    {
        jaExplodiu = true;
        Debug.Log("[Honetpot] ARMADILHA DETONADA! Aplicando dano em área e lentidão!");

        Collider2D[] alvosAtingidos = Physics2D.OverlapCircleAll(transform.position, raioExplosao);

        foreach(Collider2D col in alvosAtingidos)
        {
            if (col.CompareTag("Inimigos"))
            {
                col.SendMessage("ReceberDano", danoExplosao, SendMessageOptions.DontRequireReceiver);

                InimigoTeste inimigo = col.GetComponent<InimigoTeste>();
                if(inimigo != null)
                {
                    inimigo.AplicarLentidao(fatorLentisao, duracaoLentidao);
                }
            }
        }

        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioExplosao);
    }

}
