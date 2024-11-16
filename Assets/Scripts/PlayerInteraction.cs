using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Inventory inventory;
    public int damage = 1;
    public float interactionRange = 2f;
    public LayerMask resourceLayer;

    private Resource currentHighlightedResource; // Track the currently highlighted resource

    void Update()
    {
        // Get the mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Raycast to detect the resource under the mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, resourceLayer);

        if (hit.collider != null)
        {
            Resource resource = hit.collider.GetComponent<Resource>();

            if (resource != null)
            {
                // Highlight the resource if it's within range
                float distanceToResource = Vector2.Distance(transform.position, resource.transform.position);
                if (distanceToResource <= interactionRange)
                {
                    if (currentHighlightedResource != resource)
                    {
                        // Unhighlight the previously highlighted resource
                        if (currentHighlightedResource != null)
                        {
                            currentHighlightedResource.Highlight(false);
                        }

                        // Highlight the new resource
                        resource.Highlight(true);
                        currentHighlightedResource = resource;
                    }
                }
                else
                {
                    // Unhighlight the resource if it's out of range
                    if (currentHighlightedResource != null)
                    {
                        currentHighlightedResource.Highlight(false);
                        currentHighlightedResource = null;
                    }
                }
            }
        }
        else
        {
            // Unhighlight the resource if no resource is under the mouse
            if (currentHighlightedResource != null)
            {
                currentHighlightedResource.Highlight(false);
                currentHighlightedResource = null;
            }
        }

        // Handle interaction (e.g., clicking to damage resource)
        if (Input.GetMouseButtonDown(0) && currentHighlightedResource != null)
        {
            currentHighlightedResource.TakeDamage(damage);

            if (currentHighlightedResource.health <= 0)
            {
                inventory.AddItem(currentHighlightedResource.resourceType, 1);
                currentHighlightedResource = null; // Clear reference after destroying the resource
            }
        }
    }
}
