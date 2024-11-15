using UnityEngine;

public class Resource : MonoBehaviour
{
    public int health = 3;
    public string resourceType; // Add this field to specify the type of resource

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
