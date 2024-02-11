using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class ButtonSwitch : BaseInteractive
{
    private void Awake()
    {
        // Get the PolygonCollider2D component and set it as a trigger
        var collider = GetComponent<PolygonCollider2D>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive)
            Activate();
        else
            Deactivate();
    }
}