using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public int slotIndex; // The index of this slot in the hotbar
    public Image itemIcon; // Reference to the Image component for the resource icon
    public TextMeshProUGUI resourceCount; // Reference to the TextMeshProUGUI component for the count

    private Transform originalParent;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Store the original parent
        transform.SetParent(originalParent.parent); // Temporarily unparent the slot for dragging
        canvasGroup.blocksRaycasts = false; // Disable raycasts so other slots can detect the drop
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition; // Follow the mouse cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // Return to the original parent
        transform.localPosition = Vector3.zero; // Snap back to the original position
        canvasGroup.blocksRaycasts = true; // Re-enable raycasts
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Get the slot being dragged
        HotbarSlot draggedSlot = eventData.pointerDrag.GetComponent<HotbarSlot>();
        if (draggedSlot != null && draggedSlot != this)
        {
            // Find the HotbarUI instance
            HotbarUI hotbarUI = FindObjectOfType<HotbarUI>();
            if (hotbarUI != null)
            {
                // Swap the two slots
                hotbarUI.SwapSlots(draggedSlot.slotIndex, this.slotIndex);
            }
        }
    }
}
