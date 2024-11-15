using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Inventory inventory;
    public int damage = 1;
    public float interactionRange = 2f;
    public LayerMask resourceLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Convert the mouse position to world space
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Raycast at the mouse position to detect the clicked object
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, resourceLayer);

            if (hit.collider != null)
            {
                Resource resource = hit.collider.GetComponent<Resource>();

                if (resource != null)
                {
                    // Calculate the distance from the player to the resource
                    float distanceToResource = Vector2.Distance(transform.position, hit.collider.transform.position);

                    // Check if the resource is within interaction range
                    if (distanceToResource <= interactionRange)
                    {
                        resource.TakeDamage(damage);

                        if (resource.health <= 0)  // Assuming TakeDamage reduces health to 0 when destroyed
                        {
                            inventory.AddItem(resource.resourceType, 1); // Add resource to inventory
                        }
                    }
                    
                }
            }
        }
    }
}
