using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrastarCartas : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    /// <summary>
    /// [FIXO 1.0] Variáveis e configurações de UI
    /// </summary>
    [Header("Refer�ncia da Carta")]
    public CartaDefensiva dadosDaCartas; //ScriptableObject com as estatísticas e prefab da torre

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 posicaoInicial;
    private Transform painelPaiOriginal;

    //Método Awake executado ao inicializar: configura componentes de UI e Rigidbody da HUD
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

    //Disparado automaticamente assim quando o jogador começar a arrastar a carta
    public void OnBeginDrag(PointerEventData eventData)
    {
        painelPaiOriginal = transform.parent;
        posicaoInicial = rectTransform.anchoredPosition;

        //Move a carta para o Canvas principal para garantir que ela fique por cima de todos os elementos visuais
        transform.SetParent(canvas.transform, true);

        //Deixa a carta semi-transparente e desativa os raycasts para o mouse interagir com o mundo abaixo
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    //Disparado continuamente enquanto o jogador arrasta a carta pela tela
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

    //Disparado quando o jogador solta o botão do mouse ao terminar de arrastar
    public void OnEndDrag(PointerEventData eventData)
    {
        //Restaura a opacidade normal e reativa os raycast de UI
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //Tenta validar o espaço e instanciar a torre no map 2D
        TryInstantiateTower(eventData.position);

        //Devolve a carta visualmente para o seu painel/corredor original
        RetornarCartaAoCorredor();
    }

    //Verifica se o jogador possui pontos de servidor suficientes na HUD
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

        //Calcula a distância da câmera até z = 0 para converter a coordenada de tela corretamente para o mundo 2D
        float distanciaAteZZero = -canvas.worldCamera.transform.position.z;

        Vector3 worldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distanciaAteZZero));

        //Dispara um RayCast 2D a partir da posição do mouse no mundo
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(worldPos.x, worldPos.y), Vector2.zero);
        
        if(hit.collider != null)
        {
            //Verifica se o objetico atingindo possui a tag específica de construção "SlotDefesa"
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

    //Reseta a posição da carta devolta para o layout original da HUD caso o jogador solte a carta
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
