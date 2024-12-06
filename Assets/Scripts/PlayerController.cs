using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRange = 2.0f; // Range within which the player can interact with resources

    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure your camera has the 'MainCamera' tag.");
        }
    }

    void Update()
    {
        // Get input for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Handle interaction
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            HandleResourceInteraction();
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleResourceInteraction()
    {
        // Cast a ray from the camera to detect the clicked object
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            Debug.Log($"Hit detected: {hit.collider.name}"); // Log the name of the hit object

            Resource resource = hit.collider.GetComponent<Resource>();
            if (resource != null)
            {
                // Check if the resource is within interaction range
                if (Vector2.Distance(transform.position, resource.transform.position) <= interactRange)
                {
                    resource.HitResource(); // Interact with the resource
                }
                else
                {
                    Debug.Log("Resource is too far away!");
                }
            }
        }
        else
        {
            Debug.Log("No object hit by raycast.");
        }
    }

}
