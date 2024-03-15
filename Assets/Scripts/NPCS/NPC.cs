using UnityEngine;

[System.Serializable]
public class NPC : MonoBehaviour
{
    public string name;
    public Animator npcAnimator;

    protected virtual void Awake()
    {
        // Initialize NPC properties if needed
    }

    public void destroySelf() { 
        // Make the object disappear without destroying it
        gameObject.SetActive(false);
    }

    public virtual void Interact()
    {
        // Default interact implementation
    }
}
