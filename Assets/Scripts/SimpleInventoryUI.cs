using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InventorySprite
{
    public string itemName;   // Name of the item (e.g., "Wood", "Stone")
    public Sprite icon;       // The icon to use for this item
}

public class SimpleInventoryUI : MonoBehaviour
{
    public Inventory inventory;                    // Reference to the Inventory
    public List<Image> slotIcons;                  // List of Image components for each slot icon
    public List<TextMeshProUGUI> slotQuantities;   // List of Text components for quantities
    public List<InventorySprite> itemIcons;        // List of item names and their associated sprites

    void Start()
    {
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        // Clear all quantities and icons initially
        for (int i = 0; i < slotIcons.Count; i++)
        {
            slotIcons[i].sprite = null;           // Remove any existing icon
            slotIcons[i].enabled = false;         // Hide icon if there's no item
            slotQuantities[i].text = "";          // Set quantity to empty
        }

        // Populate icons and quantities based on items in inventory
        int index = 0;
        foreach (KeyValuePair<string, int> item in inventory.GetAllItems())
        {
            if (index < slotIcons.Count && item.Value > 0) // Only update if quantity is more than 0
            {
                slotIcons[index].sprite = GetItemSprite(item.Key); // Set the icon
                slotIcons[index].enabled = true;                   // Show icon
                slotQuantities[index].text = item.Value.ToString(); // Display item quantity
                index++;
            }
        }
    }

    private Sprite GetItemSprite(string itemName)
    {
        // Search for the sprite associated with the itemName
        foreach (var inventorySprite in itemIcons)
        {
            if (inventorySprite.itemName == itemName)
            {
                return inventorySprite.icon;
            }
        }
        return null; // Return null if no matching itemName is found
    }
}
