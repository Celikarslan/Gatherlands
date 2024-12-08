using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    private int index;
    private HotbarUI hotbarUI;
    private Image icon;
    private bool isSelected; // Tracks if this slot is selected
    public Image selectionHighlight; // Optional: Highlight for selected slot
    public Text countText; // Optional: For displaying resource count or extra info

    private void Awake()
    {
        icon = GetComponent<Image>();
        if (selectionHighlight != null)
        {
            selectionHighlight.enabled = false; // Disable highlight by default
        }
        if (countText != null)
        {
            countText.text = ""; // Clear count text by default
        }
    }

    public void Initialize(int slotIndex, HotbarUI parentUI)
    {
        index = slotIndex;
        hotbarUI = parentUI;
        ClearSlot();
    }

    public void UpdateSlot(Sprite toolIcon, string additionalText = "")
    {
        if (toolIcon != null)
        {
            icon.enabled = true;
            icon.sprite = toolIcon;

            // Update count text if provided
            if (countText != null)
            {
                countText.text = additionalText;
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        icon.enabled = false;
        icon.sprite = null;

        if (selectionHighlight != null)
        {
            selectionHighlight.enabled = false;
        }
        if (countText != null)
        {
            countText.text = "";
        }

        isSelected = false;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (selectionHighlight != null)
        {
            selectionHighlight.enabled = selected; // Enable/disable highlight
        }
    }

    public int GetSlotIndex()
    {
        return index;
    }
}
