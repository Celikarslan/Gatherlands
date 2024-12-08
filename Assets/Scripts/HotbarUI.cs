using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    public GameObject hotbarSlotPrefab; // Prefab for hotbar slots
    public Transform hotbarContainer; // Parent container for hotbar slots
    private List<GameObject> slots = new List<GameObject>(); // List of hotbar slots

    private Dictionary<int, string> slotToResourceMap = new Dictionary<int, string>(); // Map slot index to resource type
    private int hotbarSize = 10; // Number of slots in the hotbar

    public int HotbarSize => hotbarSize; // Public getter for hotbar size
    public Dictionary<int, string> SlotToResourceMap => slotToResourceMap; // Public getter for slot map


    private void Start()
    {
        InitializeHotbar();
    }

    public void UpdateHotbar()
    {
        // Iterate through all hotbar slots and refresh their content
        for (int i = 0; i < hotbarSize; i++)
        {
            if (slotToResourceMap.ContainsKey(i))
            {
                string resourceType = slotToResourceMap[i];
                int resourceCount = ResourceManager.Instance.GetResourceCount(resourceType);

                if (resourceCount > 0)
                {
                    RefreshSlot(i);
                }
                else
                {
                    // If the resource count is 0, clear the slot
                    slotToResourceMap.Remove(i);
                    ClearSlot(i);
                }
            }
        }
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
        AddResourceToEmptySlot(resourceType);
    }

    private void AddResourceToEmptySlot(string resourceType)
    {
        for (int i = 0; i < hotbarSize; i++)
        {
            if (!slotToResourceMap.ContainsKey(i)) // Ensure the slot is empty
            {
                slotToResourceMap[i] = resourceType;
                RefreshSlot(i);
                Debug.Log($"Resource '{resourceType}' added to hotbar in slot {i}.");
                return;
            }
        }

        Debug.LogWarning("No empty slots available in the hotbar! Resource cannot be added.");
    }

    public bool HasEmptySlot()
    {
        for (int i = 0; i < hotbarSize; i++)
        {
            if (!slotToResourceMap.ContainsKey(i)) // Check if the slot is empty
            {
                return true;
            }
        }
        return false; // No empty slots
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

    public void RefreshSlot(int slotIndex)
    {
        GameObject slot = slots[slotIndex];
        HotbarSlot hotbarSlot = slot.GetComponent<HotbarSlot>();

        if (slotToResourceMap.ContainsKey(slotIndex))
        {
            string resourceType = slotToResourceMap[slotIndex];
            int resourceCount = ResourceManager.Instance.GetResourceCount(resourceType);

            // Only show the count if it's greater than 1
            string countText = resourceCount > 1 ? resourceCount.ToString() : "";

            hotbarSlot.UpdateSlot(GetResourceIcon(resourceType), countText);
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
}
