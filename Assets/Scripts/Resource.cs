using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public string resourceType; // Type of the resource (e.g., "Tree", "Stone")
    public int maxHealth = 3; // Maximum health of the resource
    private int currentHealth;

    public string requiredTool; // The name of the tool required to break this resource

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Slider healthBarSlider;
    private Image healthBarFill;

    private Coroutine healthBarFadeCoroutine;

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

    public void HitResource(Tool playerTool)
    {
        // Check if the player has the correct tool equipped
        if (!CanBreakWithTool(playerTool))
        {
            Debug.Log($"Cannot break {resourceType}. Requires {requiredTool}.");
            return;
        }

        if (healthBarInstance == null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBarInstance.transform.SetParent(transform);

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
        }

        if (healthBarFadeCoroutine != null)
        {
            StopCoroutine(healthBarFadeCoroutine);
        }

        healthBarInstance.SetActive(true);
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;

        currentHealth--;

        if (healthBarSlider != null)
        {
            StartCoroutine(SmoothHealthChange(currentHealth));
        }

        if (currentHealth <= 0)
        {
            Destroy(healthBarInstance);
            Destroy(gameObject);
            ResourceManager.Instance.AddResource(resourceType);
        }
        else
        {
            healthBarFadeCoroutine = StartCoroutine(FadeOutHealthBar());
        }
    }

    private bool CanBreakWithTool(Tool playerTool)
    {
        // If no tool is required, allow breaking
        if (string.IsNullOrEmpty(requiredTool)) return true;

        // If the player doesn't have a tool, they can't break this resource
        if (playerTool == null) return false;

        // Check if the player's tool matches the required tool
        foreach (string toolType in playerTool.breakableResources)
        {
            if (toolType == resourceType) return true;
        }

        return false;
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

    private IEnumerator FadeOutHealthBar()
    {
        yield return new WaitForSeconds(1.5f);
        if (healthBarInstance != null)
        {
            healthBarInstance.SetActive(false);
        }
    }
}
