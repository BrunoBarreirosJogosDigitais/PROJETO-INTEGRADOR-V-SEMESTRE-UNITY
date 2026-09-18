using UnityEngine;

public class TorreDefensiva : MonoBehaviour
{
    public CartaDefensiva carta;
    private float contador;

    public void Inicializar(CartaDefensiva novaCarta)
    {
        carta = novaCarta;
    }

    private void Update()
    {
        if (carta == null) return;

        contador += Time.deltaTime;

        if (contador < carta.tempoRecarga) return;

        contador = 0f;

        InimigoCibernetico alvo = EncontrarAlvo();

        if (alvo != null && carta.dano > 0f)
            alvo.ReceberDano(carta.dano);
    }

    private InimigoCibernetico EncontrarAlvo()
    {
        InimigoCibernetico[] inimigos = FindObjectsOfType<InimigoCibernetico>();

        InimigoCibernetico maisProximo = null;
        float menorDistancia = carta.alcance;

        foreach (var inimigo in inimigos)
        {
            float distancia = Vector3.Distance(
                transform.position,
                inimigo.transform.position
            );

            if (distancia <= menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = inimigo;
            }
        }

        return maisProximo;
    }
}
