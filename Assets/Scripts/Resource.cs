using UnityEngine;

public class Resource : MonoBehaviour
{
    public int health = 3;
    public string resourceType;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private Color originalColor;          // Store the original color of the resource

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color
    }

    public void Highlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            spriteRenderer.color = Color.yellow; // Highlight color
        }
        else
        {
            spriteRenderer.color = originalColor; // Reset to original color
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
