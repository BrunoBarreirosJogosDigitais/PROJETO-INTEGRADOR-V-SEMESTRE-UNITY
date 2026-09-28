using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrastarCartas : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Referência da Carta")]
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

        painelPaiOriginal = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {


        posicaoInicial = rectTransform.anchoredPosition;

        transform.SetParent(canvas.transform, true);

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        TryInstantiateTower(eventData.position);

        transform.SetParent(canvas.transform, true);
        rectTransform.anchoredPosition = posicaoInicial;
    }

    private void TryInstantiateTower(Vector2 screenPosition)
    {
        if (dadosDaCartas == null || dadosDaCartas.prefabTorreParaInstanciar == null) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f))
        {

            if (hit.collider.CompareTag("SlotDefesa"))
            {
                Instantiate(dadosDaCartas.prefabTorreParaInstanciar, hit.point, Quaternion.identity);

                Debug.Log("Torre " + dadosDaCartas.nomeCarta + "construída com sucesso!");
            }      
        }
    }

}
