using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRange = 2.0f;
    public Tool currentTool;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (Input.GetMouseButton(0))
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            Resource resource = hit.collider.GetComponent<Resource>();
            if (resource != null)
            {
                resource.HitResource(currentTool);
            }
        }
    }

    public void EquipToolFromHotbar(string toolName)
    {
        Tool toolToEquip = Resources.Load<Tool>($"Tools/{toolName}");
        currentTool = toolToEquip;
        Debug.Log($"Equipped tool: {toolToEquip?.toolName ?? "None"}");
    }
}
