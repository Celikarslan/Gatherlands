using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public Inventory inventory;

    public bool CraftItem(string itemName, Dictionary<string, int> requiredResources)
    {
        foreach (var resource in requiredResources)
        {
            if (inventory.GetItemQuantity(resource.Key) < resource.Value)
                return false; // Not enough resources
        }

        foreach (var resource in requiredResources)
        {
            inventory.AddItem(resource.Key, -resource.Value); // Deduct resources
        }

        // Item crafted successfully (you can add it to the player's inventory or spawn it in the game world)
        Debug.Log("Crafted " + itemName);
        return true;
    }
}
