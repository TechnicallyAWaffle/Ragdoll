using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour
{
    public string npcName;
    public string npcIdentifier; // Unique identifier for each NPC type
    public Sprite npcSprite;
    public Animator npcAnimator;

    protected virtual void Awake()
    {
        // Initialize NPC properties if needed
    }

    public virtual void Interact()
    {
        // Default interact implementation
    }
}
