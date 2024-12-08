using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [System.Serializable]
    public class SerializableResourceRequirement
    {
        public string resourceType; // Resource name (e.g., "Wood", "Stone")
        public int amount; // Amount required
    }

    [System.Serializable]
    public class Recipe
    {
        public string itemName; // Name of the crafted item (e.g., "Pickaxe")
        public Sprite itemIcon; // Icon for the crafted item
        public Tool craftedTool; // Tool ScriptableObject for tools
        public List<SerializableResourceRequirement> requiredResources = new List<SerializableResourceRequirement>();
    }

    public List<Recipe> recipes = new List<Recipe>(); // List of all recipes
    public GameObject craftingMenu; // Reference to the Crafting Menu canvas

    private bool isCraftingMenuVisible = false; // Tracks the visibility state

    private void Update()
    {
        // Toggle the crafting menu visibility when Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCraftingMenu();
        }
    }

    private void ToggleCraftingMenu()
    {
        isCraftingMenuVisible = !isCraftingMenuVisible;
        craftingMenu.SetActive(isCraftingMenuVisible);
    }

    public void CraftItem(string itemName)
    {
        Recipe recipe = GetRecipeByName(itemName);

        if (recipe == null)
        {
            Debug.LogWarning($"Recipe for {itemName} not found!");
            return;
        }

        // Check if the player has enough resources
        if (!HasRequiredResources(recipe))
        {
            Debug.LogWarning($"Not enough resources to craft {itemName}!");
            return;
        }

        // Consume the resources
        foreach (var resource in recipe.requiredResources)
        {
            ResourceManager.Instance.ConsumeResource(resource.resourceType, resource.amount);
        }

        // Add the crafted tool to the hotbar
        if (recipe.craftedTool != null)
        {
            AddToHotbar(recipe.craftedTool);
            Debug.Log($"{itemName} crafted and added to hotbar!");
        }
    }

    private Recipe GetRecipeByName(string itemName)
    {
        return recipes.Find(recipe => recipe.itemName == itemName);
    }

    private bool HasRequiredResources(Recipe recipe)
    {
        foreach (var resource in recipe.requiredResources)
        {
            int playerResourceCount = ResourceManager.Instance.GetResourceCount(resource.resourceType);
            if (playerResourceCount < resource.amount)
            {
                return false; // Not enough resources
            }
        }
        return true; // All resources are available
    }

    private void AddToHotbar(Tool craftedTool)
    {
        HotbarUI hotbarUI = FindObjectOfType<HotbarUI>();
        if (hotbarUI != null)
        {
            // Find the first available empty slot
            for (int i = 0; i < hotbarUI.HotbarSize; i++)
            {
                if (!hotbarUI.SlotToResourceMap.ContainsKey(i)) // Empty slot
                {
                    hotbarUI.SlotToResourceMap[i] = craftedTool.toolName;
                    hotbarUI.RefreshSlot(i); // Update the hotbar visuals
                    Debug.Log($"Tool '{craftedTool.toolName}' added to hotbar in slot {i}.");
                    return;
                }
            }

            Debug.LogWarning("No empty slots available in the hotbar!");
        }
        else
        {
            Debug.LogWarning("HotbarUI not found. Tool could not be added to the hotbar.");
        }
    }
}
