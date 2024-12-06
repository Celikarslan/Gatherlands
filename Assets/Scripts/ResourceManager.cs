using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    // Event to notify the hotbar of inventory changes
    public event Action<string, int> OnInventoryChanged; // Sends resource type and new count

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(string resourceType)
    {
        if (!inventory.ContainsKey(resourceType))
            inventory[resourceType] = 0;

        inventory[resourceType]++;
        Debug.Log($"Collected {resourceType}. Total: {inventory[resourceType]}");

        // Notify listeners about the inventory change
        OnInventoryChanged?.Invoke(resourceType, inventory[resourceType]);
    }

    public void RemoveResource(string resourceType, int amount)
    {
        if (inventory.ContainsKey(resourceType))
        {
            inventory[resourceType] -= amount;
            if (inventory[resourceType] <= 0)
            {
                inventory.Remove(resourceType); // Remove the resource if count is zero or less
                Debug.Log($"Resource '{resourceType}' depleted and removed from inventory.");
            }

            // Notify listeners about the inventory change
            OnInventoryChanged?.Invoke(resourceType, inventory.ContainsKey(resourceType) ? inventory[resourceType] : 0);
        }
    }

    public int GetResourceCount(string resourceType)
    {
        if (inventory.ContainsKey(resourceType))
            return inventory[resourceType];
        return 0;
    }

    public Dictionary<string, int> GetInventory()
    {
        if (inventory == null)
            inventory = new Dictionary<string, int>();
        return inventory;
    }
}
