using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public int damage = 1;
    public float interactionRange = 2f;  // Set the range within which the player can interact
    public LayerMask resourceLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, resourceLayer);

            if (hit.collider != null)
            {
                Resource resource = hit.collider.GetComponent<Resource>();
                if (resource != null)
                {
                    float distanceToResource = Vector2.Distance(transform.position, hit.collider.transform.position);

                    // Check if player is within range
                    if (distanceToResource <= interactionRange)
                    {
                        resource.TakeDamage(damage);
                    }
                    else
                    {
                        Debug.Log("Too far away to interact with resource.");
                    }
                }
            }
        }
    }
}
