using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string itemName, int quantity)
    {
    if (items.ContainsKey(itemName))
        items[itemName] += quantity;
    else
        items[itemName] = quantity;

    FindObjectOfType<SimpleInventoryUI>().UpdateInventoryUI(); // Refresh UI
    }



    public int GetItemQuantity(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }
    public Dictionary<string, int> GetAllItems()
    {
    return items;
    }      

}
