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

        //Retorna obrigatoriamente a torre usando física 2D
        if(painelPaiOriginal != null)
        {
            transform.SetParent(canvas.transform, false);
            rectTransform.anchoredPosition = posicaoInicial;
        }     
    }

    private void TryInstantiateTower(Vector2 screenPosition)
    {
        if (dadosDaCartas == null || dadosDaCartas.prefabTorreParaInstanciar == null)
        {
            Debug.LogWarning("Dados da carta ou Prefab da Torre estão nulos!");
            return;
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -Camera.main.transform.position.z));
        Vector2 worldPosition2D = new Vector2(worldPosition.x, worldPosition.y);


        RaycastHit2D hit = Physics2D.Raycast(worldPosition2D, Vector2.zero);
        
        if(hit.collider != null)
        {
            Debug.Log("Raycast 2D atingiu o objeto: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("SlotDefesa"))
            {
                Vector3 posicaoInstanciacao = new Vector3(hit.point.x, hit.point.y, 0f);
                Instantiate(dadosDaCartas.prefabTorreParaInstanciar, hit.point, Quaternion.identity);

                Debug.Log("Torre " + dadosDaCartas.nomeCarta + "constru�da com sucesso!");
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

}
