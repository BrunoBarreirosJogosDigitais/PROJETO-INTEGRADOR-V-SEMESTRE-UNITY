using UnityEngine;
using UnityEngine.UI;

public class PlacaMae : MonoBehaviour
{
    [Header("Configuração de Impacto")]
    public string tagInimigos = "Inimigos";

    [Header("Interface Visual (UI)")]
    public Text textoVidaPlaca;

    void Start()
    {
        if (textoVidaPlaca != null)
        {
            textoVidaPlaca.text = "";
        }
    }

    void Update()
    {
        AtualizarInterfaceVisual();
    }

    void AtualizarInterfaceVisual()
    {
        if (textoVidaPlaca != null && GerenciadorJogo.Instancia != null)
        {
            int vidaAtual = GerenciadorJogo.Instancia.vidaPlaca;
            int vidaMax = GerenciadorJogo.Instancia.vidaPlacaMaxima;

            textoVidaPlaca.text = $"{vidaAtual} / {vidaMax}";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagInimigos))
        {
            int danoDoInimigo = 0;
            string nomeUnidade = collision.gameObject.name;

            // Verifica se o objeto que bateu é um Projétil/Tiro do DDoS Bot
            TiroInimigo tiroScript = collision.gameObject.GetComponent<TiroInimigo>();
            if (tiroScript != null)
            {
                danoDoInimigo = tiroScript.danoDoTiro; // Pega os 25 de dano configurados no tiro!
            }
            else
            {
                // Se for um inimigo correndo (Worm, Trojan, DDoS Bot corpo a corpo)
                danoDoInimigo = ObterDanoDoInimigo(collision.gameObject);
            }

            if (GerenciadorJogo.Instancia != null)
            {
                GerenciadorJogo.Instancia.ReceberDanoPlaca(danoDoInimigo);
                Debug.Log($"<color=cyan>[PLACA-MÃE]</color> Impacto de: <b>{nomeUnidade}</b> | Dano aplicado ao núcleo: <color=yellow>{danoDoInimigo}</color> HP | Vida restante da Placa: {GerenciadorJogo.Instancia.vidaPlaca}");
            }

            // Destrói o projétil ou inimigo após o impacto
            Destroy(collision.gameObject);
        }
    }

    private int ObterDanoDoInimigo(GameObject inimigoObj)
    {
        foreach (var comp in inimigoObj.GetComponents<MonoBehaviour>())
        {
            var tipo = comp.GetType();
            var campo = tipo.GetField("danoAoNucleo");
            if (campo != null)
            {
                object valor = campo.GetValue(comp);
                if (valor is int i) return i;
                if (valor is float f) return Mathf.RoundToInt(f);
            }
        }
        return 15;
    }
}