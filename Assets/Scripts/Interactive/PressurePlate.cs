using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : BaseInteractive
{
    public enum PlateType { Heavy, Light }
    public PlateType plateType;

    private void Awake()
    {
        // Get the BoxCollider2D component and set it as a trigger
        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive && 
            ((plateType == PlateType.Light && (collision.CompareTag("Ragdoll") || collision.CompareTag("RagdollHead") || collision.CompareTag("Scuttler")))
            || (plateType == PlateType.Heavy && collision.CompareTag("Ragdoll"))))
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Deactivate();
    }
}
