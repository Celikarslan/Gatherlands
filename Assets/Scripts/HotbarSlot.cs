using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    public string resourceType; // Resource type stored in this slot

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetResourceType(string type)
    {
        resourceType = type;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = rectTransform.parent;
        rectTransform.SetParent(originalParent.parent); // Temporarily unparent for dragging
        canvasGroup.blocksRaycasts = false; // Prevent interaction while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // Follow the mouse cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.SetParent(originalParent); // Return to original parent
        rectTransform.localPosition = Vector3.zero; // Snap back to slot
        canvasGroup.blocksRaycasts = true; // Re-enable interaction
    }
}
