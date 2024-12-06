using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public string resourceType; // Type of the resource (e.g., "Tree", "Stone")
    public int maxHealth = 3; // Maximum health of the resource
    private int currentHealth;


    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthBarSlider;
    private Image healthBarFill;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnMouseEnter()
    {
        // Highlight the resource when moused over
        spriteRenderer.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        // Reset the highlight when the mouse leaves
        spriteRenderer.color = originalColor;
    }

    public void HitResource()
    {
        if (healthBarInstance == null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBarInstance.transform.SetParent(transform);

            // Position the health bar dynamically
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if (collider != null)
            {
                healthBarInstance.transform.position = new Vector3(
                    collider.bounds.center.x,
                    collider.bounds.max.y + 0.1f,
                    transform.position.z
                );
            }

            healthBarSlider = healthBarInstance.GetComponentInChildren<Slider>();
            healthBarFill = healthBarSlider.fillRect.GetComponent<Image>(); // Reference the Fill image
        }

        if (healthBarFill != null)
        {
            UpdateHealthBarColor();
        }

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

        currentHealth--;

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Destroy(healthBarInstance);
            Destroy(gameObject);
            ResourceManager.Instance.AddResource(resourceType);
        }
    }

    private void UpdateHealthBarColor()
    {
        // Change the color based on health percentage
        float healthPercentage = (float)currentHealth / maxHealth;

        if (healthPercentage > 0.5f)
        {
            healthBarFill.color = Color.green; // Full health
        }
        else if (healthPercentage > 0.2f)
        {
            healthBarFill.color = Color.yellow; // Medium health
        }
        else
        {
            healthBarFill.color = Color.red; // Low health
        }
    }



    private IEnumerator SmoothHealthChange(float targetValue)
    {
        float currentValue = healthBarSlider.value;

        float elapsed = 0f;
        float duration = 0.3f; // Smooth animation duration

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            healthBarSlider.value = Mathf.Lerp(currentValue, targetValue, elapsed / duration);
            yield return null;
        }

        healthBarSlider.value = targetValue;
    }

}
