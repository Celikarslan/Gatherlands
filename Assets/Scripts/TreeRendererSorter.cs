using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TreeRendererSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
    }

    void UpdateSortingOrder()
    {
        // Adjust sorting order based on Y position
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        // Ensure tree always renders above the tilemap
        spriteRenderer.sortingLayerName = "Default"; // Ensure it's on the Default layer
    }

    void OnValidate()
    {
        // Ensure this runs during prefab edits as well
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
    }
}
