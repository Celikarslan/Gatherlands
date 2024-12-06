using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarUI : MonoBehaviour
{
    public GameObject hotbarSlotPrefab; // Prefab for hotbar slots
    public Transform hotbarContainer; // Parent container for hotbar slots
    private List<GameObject> slots = new List<GameObject>(); // List of hotbar slots

    private Dictionary<int, string> slotToResourceMap = new Dictionary<int, string>(); // Map slot index to resource type
    private int hotbarSize = 10; // Number of slots in the hotbar

    private void Start()
    {
        InitializeHotbar();
    }

    public void AddResource(string resourceType)
    {
        // Find the first empty slot
        for (int i = 0; i < hotbarSize; i++)
        {
            if (!slotToResourceMap.ContainsKey(i))
            {
                slotToResourceMap[i] = resourceType;
                RefreshSlot(i);
                return;
            }
        }

        Debug.LogWarning("No empty slots available in the hotbar!");
    }

    public void AddOrIncrementResource(string resourceType)
    {
        // Check if the resource already exists in the hotbar
        foreach (var entry in slotToResourceMap)
        {
            if (entry.Value == resourceType)
            {
                RefreshSlot(entry.Key); // Refresh the existing slot
                return;
            }
        }

        // If not found, add it to a new slot
        AddResource(resourceType);
    }



    private void InitializeHotbar()
    {
        for (int i = 0; i < hotbarSize; i++) // Adjust for the number of slots
        {
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarContainer);
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();

            if (slotRectTransform != null)
            {
                // Set the slot size to 200x200
                slotRectTransform.sizeDelta = new Vector2(200, 200);
            }

            HotbarSlot hotbarSlot = slot.GetComponent<HotbarSlot>();

            if (hotbarSlot == null)
            {
                Debug.LogError($"HotbarSlot component is missing in prefab for slot {i}!");
                continue;
            }

            hotbarSlot.Initialize(i, this); // Assign index and HotbarUI reference
            slots.Add(slot);

            ClearSlot(i); // Initialize the slot as empty
        }
    }


    public void SwapResources(int indexA, int indexB)
    {
        // Swap resources in the map
        string tempResource = slotToResourceMap.ContainsKey(indexA) ? slotToResourceMap[indexA] : null;

        if (slotToResourceMap.ContainsKey(indexB))
        {
            slotToResourceMap[indexA] = slotToResourceMap[indexB];
        }
        else
        {
            slotToResourceMap.Remove(indexA);
        }

        if (tempResource != null)
        {
            slotToResourceMap[indexB] = tempResource;
        }
        else
        {
            slotToResourceMap.Remove(indexB);
        }

        // Refresh visuals for both slots
        RefreshSlot(indexA);
        RefreshSlot(indexB);
    }

    private void RefreshSlot(int slotIndex)
    {
        GameObject slot = slots[slotIndex];
        HotbarSlot hotbarSlot = slot.GetComponent<HotbarSlot>();

        if (slotToResourceMap.ContainsKey(slotIndex))
        {
            string resourceType = slotToResourceMap[slotIndex];
            int resourceCount = ResourceManager.Instance.GetResourceCount(resourceType);

            hotbarSlot.UpdateSlot(GetResourceIcon(resourceType), resourceCount.ToString());
        }
        else
        {
            ClearSlot(slotIndex);
        }
    }

    private void ClearSlot(int slotIndex)
    {
        GameObject slot = slots[slotIndex];
        HotbarSlot hotbarSlot = slot.GetComponent<HotbarSlot>();

        if (hotbarSlot == null)
        {
            Debug.LogError($"HotbarSlot component is missing from slot {slotIndex}!");
            return;
        }

        hotbarSlot.UpdateSlot(null, ""); // Clear the slot
    }


    private Sprite GetResourceIcon(string resourceType)
    {
        // Dynamically load resource icons from the Resources folder
        return Resources.Load<Sprite>($"Icons/{resourceType}");
    }

    public void AddResourceToSlot(int slotIndex, string resourceType)
    {
        slotToResourceMap[slotIndex] = resourceType;
        RefreshSlot(slotIndex);
    }
}
