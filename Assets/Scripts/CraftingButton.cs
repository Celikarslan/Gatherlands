using UnityEngine;
using UnityEngine.UI;

public class CraftingButton : MonoBehaviour
{
    public string itemName; // The name of the item this button crafts
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => CraftItem());
    }

    private void CraftItem()
    {
        FindObjectOfType<CraftingManager>().CraftItem(itemName);
    }
}
