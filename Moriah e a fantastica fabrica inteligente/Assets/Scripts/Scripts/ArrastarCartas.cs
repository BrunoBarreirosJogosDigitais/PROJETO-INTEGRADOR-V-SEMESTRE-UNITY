using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrastarCartas : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Refer�ncia da Carta")]
    public CartaDefensiva dadosDaCartas;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 posicaoInicial;
    private Transform painelPaiOriginal;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        //painelPaiOriginal = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        painelPaiOriginal = transform.parent;
        posicaoInicial = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);

        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out localPoint
        );
        rectTransform.localPosition = localPoint;    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        TryInstantiateTower(eventData.position);

        RetornarCartaAoCorredor();
    }

    private void TryInstantiateTower(Vector2 screenPosition)
    {
        if (dadosDaCartas == null || dadosDaCartas.prefabTorreParaInstanciar == null)
        {
            Debug.LogWarning("Dados da carta ou Prefab da Torre estão nulos!");
            return;
        }

        
        if(GerenciadorJogo.Instancia != null)
        {
            if (GerenciadorJogo.Instancia.pontosServidor < dadosDaCartas.custoServidor)
            {
                Debug.Log("Pontos de Servidor insuficientes para invocar: " + dadosDaCartas.nomeCarta);
                return;
            }
        }

        float distanciaAteZZero = -canvas.worldCamera.transform.position.z;

        Vector3 worldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distanciaAteZZero));
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y), Vector2.zero);
        
        if(hit.collider != null)
        {
            Debug.Log("Raycast 2D atingiu o objeto: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("SlotDefesa"))
            {
                Vector3 posicaoInstanciacao = new Vector3(worldPos.x, worldPos.y, 0f);

                Instantiate(dadosDaCartas.prefabTorreParaInstanciar, posicaoInstanciacao, Quaternion.identity);
                Debug.Log("Torre " + dadosDaCartas.nomeCarta + "constru�da com sucesso!");

                if(GerenciadorJogo.Instancia != null)
                {
                    GerenciadorJogo.Instancia.GastarPontosServidor(dadosDaCartas.custoServidor);
                }

                Debug.Log("Torre " + dadosDaCartas.nomeCarta + "construída com sucesso!");
            } 
            else
            {
                Debug.Log("Objeto atingido: " + hit.collider.gameObject.name + " (Falta a tag 'SlotDefesa').");
            }     
        }
        else
        {
            Debug.Log("O Raycast não atingiu nenhum colisor 2D");
        }
    }

    private void RetornarCartaAoCorredor()
    {
        if(painelPaiOriginal != null)
        {
            transform.SetParent(painelPaiOriginal, false);
            rectTransform.anchoredPosition = posicaoInicial;
            rectTransform.localScale = Vector3.one;
        }
    }
}
