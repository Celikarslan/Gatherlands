using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRange = 2.0f;

    public Tool currentTool; // Currently equipped tool
    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
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
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleResourceInteraction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            Resource resource = hit.collider.GetComponent<Resource>();
            if (resource != null && Vector2.Distance(transform.position, resource.transform.position) <= interactRange)
            {
                resource.HitResource(currentTool); // Pass the player's current tool
            }
        }
    }


    public void EquipTool(Tool tool)
    {
        currentTool = tool;
        Debug.Log($"Equipped {tool.toolName}!");
    }

    private bool CanBreakResource(string resourceType)
    {
        if (currentTool == null) return false;
        foreach (string breakableResource in currentTool.breakableResources)
        {
            if (breakableResource == resourceType) return true;
        }
        return false;
    }
}