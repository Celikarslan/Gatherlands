using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    public GameObject hotbarSlotPrefab; // Prefab for each hotbar slot
    public Transform hotbarContainer; // Parent container for hotbar slots

    private Dictionary<string, GameObject> slots = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        // Subscribe to the ResourceManager's inventory change event
        ResourceManager.Instance.OnInventoryChanged += UpdateHotbar;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to avoid memory leaks
        ResourceManager.Instance.OnInventoryChanged -= UpdateHotbar;
    }

    public void UpdateHotbar()
    {
        // Clear the current slots
        foreach (Transform child in hotbarContainer)
        {
            Destroy(child.gameObject);
        }

        slots.Clear();

        // Populate the hotbar with updated inventory
        foreach (var resource in ResourceManager.Instance.GetInventory())
        {
            string resourceType = resource.Key;
            int resourceCount = resource.Value;

            // Create a new slot for the resource
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarContainer);
            slots[resourceType] = slot;

            // Adjust the size of the slot
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slotRectTransform.sizeDelta = new Vector2(200, 200); // Set width and height to 200x200

            // Access the children of the slot
            Transform itemSlot = slot.transform.GetChild(0); // The 'ItemSlot' child
            Transform resourceCountText = slot.transform.GetChild(1); // The 'ResourceCount' child

            // Set the icon and count
            itemSlot.GetComponent<Image>().sprite = GetResourceIcon(resourceType); // Resource Icon
            resourceCountText.GetComponent<TextMeshProUGUI>().text = resourceCount.ToString(); // Resource Count
        }
    }


    private Sprite GetResourceIcon(string resourceType)
{
    // Load the sprite dynamically from the Resources folder
    Sprite icon = Resources.Load<Sprite>($"Icons/{resourceType}");

    if (icon == null)
    {
        Debug.LogError($"Icon for resource '{resourceType}' not found!");
    }

    return icon;
}
}
