using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AplicadorEscalaTexto : MonoBehaviour
{
    // Guarda o tamanho original de cada texto 
    private readonly Dictionary<Text, float> tamanhosLegacy = new Dictionary<Text, float>();

    // Isso é só pro TMP se tiver
    private readonly Dictionary<TMP_Text, float> tamanhosTMP = new Dictionary<TMP_Text, float>();

    private void Start()
    {
        Registrar();

        if (GerenciadorEscalaTexto.Instancia != null)
        {
            GerenciadorEscalaTexto.Instancia.AoMudarEscala += Aplicar;
            Aplicar(GerenciadorEscalaTexto.Instancia.EscalaAtual);
        }
    }

    private void OnDestroy()
    {
        if (GerenciadorEscalaTexto.Instancia != null)
            GerenciadorEscalaTexto.Instancia.AoMudarEscala -= Aplicar;
    }

    // Vai procurar todos os textos do canvas do terminal, inclusive os desativados
    // Como aquele painel de cartas.
    private void Registrar()
    {
        foreach (Text t in GetComponentsInChildren<Text>(true))
        {
            if (!tamanhosLegacy.ContainsKey(t))
                tamanhosLegacy.Add(t, t.fontSize);
        }

        foreach (TMP_Text t in GetComponentsInChildren<TMP_Text>(true))
        {
            if (!tamanhosTMP.ContainsKey(t))
                tamanhosTMP.Add(t, t.fontSize);
        }
    }

    private void Aplicar(float escala)
    {
        foreach (var par in tamanhosLegacy)
        {
            if (par.Key == null) continue;
            par.Key.fontSize = Mathf.RoundToInt(par.Value * escala);
        }

        foreach (var par in tamanhosTMP)
        {
            if (par.Key == null) continue;
            par.Key.fontSize = par.Value * escala;
        }
    }

    // Isso aqui é só pra forçar a atualização do tamanho dos textos, caso algum texto seja adicionado depois do Start.
    public void Atualizar()
    {
        Registrar();

        float escala = GerenciadorEscalaTexto.Instancia != null
            ? GerenciadorEscalaTexto.Instancia.EscalaAtual
            : 1f;

        Aplicar(escala);
    }
}
