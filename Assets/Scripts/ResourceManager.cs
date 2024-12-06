using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddResource(string resourceType)
    {
        if (!inventory.ContainsKey(resourceType))
            inventory[resourceType] = 0;

        inventory[resourceType]++;
        Debug.Log($"Collected {resourceType}. Total: {inventory[resourceType]}");
    }

    public int GetResourceCount(string resourceType)
    {
        if (inventory.ContainsKey(resourceType))
            return inventory[resourceType];
        return 0;
    }
}
