using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Tool")]
public class Tool : ScriptableObject
{
    public string toolName; // Name of the tool (e.g., "Pickaxe")
    public Sprite toolIcon; // Icon for the tool
    public string[] breakableResources; // Resources this tool can break
}
