using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        HotbarUI hotbarUI = FindObjectOfType<HotbarUI>();
        hotbarUI.AddOrIncrementResource(resourceType);
    }


    public int GetResourceCount(string resourceType)
    {
        return inventory.ContainsKey(resourceType) ? inventory[resourceType] : 0;
    }

    public bool ConsumeResource(string resourceType, int amount)
    {
        if (inventory.ContainsKey(resourceType) && inventory[resourceType] >= amount)
        {
            inventory[resourceType] -= amount;

            // Update the HotbarUI to reflect the new resource count
            HotbarUI hotbarUI = FindObjectOfType<HotbarUI>();
            if (hotbarUI != null)
            {
                hotbarUI.UpdateHotbar();
            }

            return true; // Successfully consumed the resource
        }
        else
        {
            Debug.LogWarning($"Not enough {resourceType} to consume! Required: {amount}, Available: {GetResourceCount(resourceType)}");
            return false; // Not enough resources
        }
    }
}
