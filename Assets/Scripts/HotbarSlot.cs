using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HotbarSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image itemIcon; // Icon for the resource
    public TextMeshProUGUI resourceCount; // Text for the resource count
    private int slotIndex; // Index of this slot in the hotbar
    private HotbarUI hotbarUI; // Reference to the HotbarUI

    private GameObject dragIcon; // Temporary icon during drag
    private Canvas dragCanvas;

    public void Initialize(int index, HotbarUI ui)
    {
        slotIndex = index;
        hotbarUI = ui;

        // Find or create a drag canvas for animations
        dragCanvas = FindObjectOfType<Canvas>();
    }

    public void UpdateSlot(Sprite icon, string count)
    {
        itemIcon.sprite = icon;
        itemIcon.color = icon == null ? new Color(1, 1, 1, 0) : Color.white; // Make icon transparent if null
        resourceCount.text = count;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        

        if (itemIcon.sprite == null) return;

        // Create a temporary icon for dragging
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(dragCanvas.transform, false);
        dragIcon.transform.SetAsLastSibling();

        Image icon = dragIcon.AddComponent<Image>();
        icon.sprite = itemIcon.sprite;
        icon.raycastTarget = false; // Prevent raycast blocking

        RectTransform rt = dragIcon.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(100, 100); // Adjust size of the dragging icon
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon == null) return;

        // Move the drag icon with the cursor
        dragIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Destroy the drag icon when dragging ends
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        HotbarSlot draggedSlot = eventData.pointerDrag?.GetComponent<HotbarSlot>();

        if (draggedSlot != null && draggedSlot != this)
        {
            hotbarUI.SwapResources(draggedSlot.slotIndex, slotIndex);
        }
    }
}