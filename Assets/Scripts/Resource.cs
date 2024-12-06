using UnityEngine;

public class Resource : MonoBehaviour
{
    public int clicksToBreak = 3; // Number of clicks to break this resource
    public string resourceType; // Type of resource (e.g., "Tree", "Stone")

    private int currentClicks = 0; // Tracks clicks received

    public void Initialize(string type)
    {
        resourceType = type;
    }

    void OnMouseDown()
    {
        // Increment click count
        currentClicks++;

        // Check if the resource should break
        if (currentClicks >= clicksToBreak)
        {
            CollectResource();
        }
    }

    void CollectResource()
    {
        // Add resource to the player's inventory
        ResourceManager.Instance.AddResource(resourceType);

        // Destroy the resource object
        Destroy(gameObject);
    }
}
