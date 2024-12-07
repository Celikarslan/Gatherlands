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

        // Check if player has enough resources
        if (!HasRequiredResources(recipe))
        {
            Debug.LogWarning($"Not enough resources to craft {itemName}!");
            return;
        }

        // Check for inventory space
        if (!HasInventorySpace())
        {
            Debug.LogWarning("No available inventory space to craft this item!");
            return;
        }

        // Consume resources
        foreach (var resource in recipe.requiredResources)
        {
            ResourceManager.Instance.ConsumeResource(resource.resourceType, resource.amount);
        }

        // Craft the item
        if (recipe.craftedTool != null)
        {
            AddToInventory(recipe.craftedTool);
            Debug.Log($"{itemName} crafted and added to inventory!");
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

    private bool HasInventorySpace()
    {
        // Check if there is an open slot in the hotbar
        HotbarUI hotbarUI = FindObjectOfType<HotbarUI>();
        return hotbarUI.HasEmptySlot();
    }

    private void AddToInventory(Tool craftedTool)
    {
        // Add the crafted tool to the first empty slot in the hotbar
        HotbarUI hotbarUI = FindObjectOfType<HotbarUI>();
        hotbarUI.AddCraftedToolToEmptySlot(craftedTool);

        // Equip the crafted tool
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.EquipTool(craftedTool);
        }

        // Spawn the tool next to the player
        SpawnToolInWorld(craftedTool);
    }


    private void SpawnToolInWorld(Tool craftedTool)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Vector3 spawnPosition = player.transform.position + new Vector3(1f, 0, 0); // Spawn next to player
        GameObject toolObject = new GameObject(craftedTool.toolName); // Placeholder GameObject
        toolObject.transform.position = spawnPosition;

        Debug.Log($"{craftedTool.toolName} spawned next to the player!");
    }
}
