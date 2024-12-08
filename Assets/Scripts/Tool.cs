using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Tool")]
public class Tool : ScriptableObject
{
    public string toolName;
    public Sprite toolIcon;
    public string[] breakableResources;
    public float speedMultiplier = 1.0f;
}
