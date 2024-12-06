using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Reduce visibility and allow dropping
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // No movement of the slot itself
        // Remove this if you want visual movement: rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Reset visibility and block raycasts
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
