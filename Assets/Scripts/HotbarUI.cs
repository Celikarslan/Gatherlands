using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HotbarUI : MonoBehaviour
{
    public GameObject hotbarSlotPrefab; // Prefab for each hotbar slot
    public Transform hotbarContainer; // Parent container for hotbar slots

    private List<GameObject> slots = new List<GameObject>(); // List of all hotbar slots
    private Dictionary<int, string> slotToResourceMap = new Dictionary<int, string>(); // Maps slot index to resource type

    private void Start()
    {
        InitializeHotbar();
    }

    private void OnEnable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnInventoryChanged += OnInventoryChangedHandler;
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnInventoryChanged -= OnInventoryChangedHandler;
    }

    private void OnInventoryChangedHandler(string resourceType, int count)
    {
        // Update only the slot corresponding to the changed resource
        int slotIndex = FindSlotIndexForResource(resourceType);

        if (count > 0) // Resource exists or was updated
        {
            if (slotIndex >= 0)
            {
                // Update the existing slot
                UpdateSlot(slotIndex, resourceType, count);
            }
            else
            {
                // Find the first empty slot and assign the resource
                slotIndex = FindFirstEmptySlot();
                if (slotIndex >= 0)
                {
                    slotToResourceMap[slotIndex] = resourceType;
                    UpdateSlot(slotIndex, resourceType, count);
                }
            }
        }
        else // Resource was removed or depleted
        {
            if (slotIndex >= 0)
            {
                ClearSlot(slotIndex);
            }
        }
    }



    private void InitializeHotbar()
    {
        for (int i = 0; i < 10; i++) // Create 10 slots
        {
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarContainer);
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slotRectTransform.sizeDelta = new Vector2(200, 200); // Set slot size

            HotbarSlot hotbarSlot = slot.GetComponent<HotbarSlot>();
            hotbarSlot.slotIndex = i; // Assign a unique index to each slot
            hotbarSlot.itemIcon = slot.transform.GetChild(0).GetComponent<Image>(); // Assign the icon reference
            hotbarSlot.resourceCount = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>(); // Assign the count reference

            slots.Add(slot);

            // Initialize the slot as empty
            ClearSlot(i);
        }
    }

    public void UpdateHotbar()
    {
        Debug.Log("Updating Hotbar...");
        Dictionary<string, int> inventory = ResourceManager.Instance.GetInventory();

        // Update or add resources to existing slots
        foreach (var resource in inventory)
        {
            int slotIndex = FindSlotIndexForResource(resource.Key);
            if (slotIndex >= 0)
            {
                // Update the existing slot
                UpdateSlot(slotIndex, resource.Key, resource.Value);
            }
            else
            {
                // Find the first empty slot and assign the resource
                slotIndex = FindFirstEmptySlot();
                if (slotIndex >= 0)
                {
                    slotToResourceMap[slotIndex] = resource.Key;
                    UpdateSlot(slotIndex, resource.Key, resource.Value);
                }
            }
        }

        // Clear remaining slots that are unused
        ClearUnusedSlots(inventory);

        DebugSlotMapping(); // Optional: Debug the slot-to-resource mapping
    }


    public void SwapSlots(int indexA, int indexB)
    {
        Debug.Log($"Swapping slots {indexA} and {indexB}");

        // Temporarily store the resource types of both slots
        string resourceA = slotToResourceMap.ContainsKey(indexA) ? slotToResourceMap[indexA] : null;
        string resourceB = slotToResourceMap.ContainsKey(indexB) ? slotToResourceMap[indexB] : null;

        // Perform the swap in the map
        if (resourceA != null)
        {
            slotToResourceMap[indexB] = resourceA;
        }
        else
        {
            slotToResourceMap.Remove(indexB);
        }

        if (resourceB != null)
        {
            slotToResourceMap[indexA] = resourceB;
        }
        else
        {
            slotToResourceMap.Remove(indexA);
        }

        // Refresh the visuals for both slots
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

            hotbarSlot.itemIcon.sprite = GetResourceIcon(resourceType);
            hotbarSlot.resourceCount.text = resourceCount.ToString();
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

        hotbarSlot.itemIcon.sprite = null; // Clear the icon
        hotbarSlot.resourceCount.text = ""; // Clear the count

        // Remove from the resource map
        if (slotToResourceMap.ContainsKey(slotIndex))
        {
            slotToResourceMap.Remove(slotIndex);
        }
    }

    private void ClearUnusedSlots(Dictionary<string, int> inventory)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slotToResourceMap.ContainsKey(i))
            {
                string resourceType = slotToResourceMap[i];
                if (!inventory.ContainsKey(resourceType))
                {
                    ClearSlot(i);
                }
            }
        }
    }

    private int FindSlotIndexForResource(string resourceType)
    {
        foreach (var entry in slotToResourceMap)
        {
            if (entry.Value == resourceType)
                return entry.Key;
        }
        return -1; // Not found
    }

    private int FindFirstEmptySlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slotToResourceMap.ContainsKey(i))
                return i;
        }
        return -1; // No empty slots
    }

    private void UpdateSlot(int slotIndex, string resourceType, int resourceCount)
    {
        GameObject slot = slots[slotIndex];
        HotbarSlot hotbarSlot = slot.GetComponent<HotbarSlot>();

        hotbarSlot.itemIcon.sprite = GetResourceIcon(resourceType);
        hotbarSlot.resourceCount.text = resourceCount.ToString();

        Debug.Log($"Updated slot {slotIndex} with {resourceType} ({resourceCount})");
    }

    private Sprite GetResourceIcon(string resourceType)
    {
        Sprite icon = Resources.Load<Sprite>($"Icons/{resourceType}");
        if (icon == null)
        {
            Debug.LogWarning($"Icon for resource '{resourceType}' not found! Using default icon.");
            icon = Resources.Load<Sprite>("Icons/DefaultIcon"); // Ensure you have a default icon
        }
        return icon;
    }

    private void DebugSlotMapping()
    {
        Debug.Log("Slot-to-Resource Map:");
        foreach (var entry in slotToResourceMap)
        {
            Debug.Log($"Slot {entry.Key}: {entry.Value}");
        }
    }
}
