using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    // Event to notify the hotbar of inventory changes
    public event Action OnInventoryChanged;

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
        {
            inventory[resourceType] = 0;
        }

        inventory[resourceType]++;
        Debug.Log($"Collected {resourceType}. Total: {inventory[resourceType]}");

        // Notify listeners that the inventory has changed
        OnInventoryChanged?.Invoke();
    }

    public Dictionary<string, int> GetInventory()
    {
        if (inventory == null)
            inventory = new Dictionary<string, int>();
        return inventory;
    }
}
