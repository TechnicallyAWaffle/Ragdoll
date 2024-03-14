using System;
using System.Collections;
using UnityEngine;

public class SpeedPowerup : Item
{
    public SpeedPowerup() : base(10, "Watch as the cat uses a mere fraction of his potential, gaining speed that humans can hardly witness.", "Speed", 2f, 5f)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        ragdollMain.moveSpeed *= multiplier;
        StartCoroutine(ResetSpeed(ragdollMain));

        // Disable the visual representation and collider
        if (TryGetComponent<Renderer>(out var renderer))
        {
            renderer.enabled = false; // Make the object invisible.
        }
        if (TryGetComponent<PolygonCollider2D>(out var collider))
        {
            Debug.Log("PolygonCollider2D disabled");
            collider.enabled = false; // Make the object non-interactable for 2D physics.
        }
    }


    private IEnumerator ResetSpeed(RagdollMain ragdollMain)
    {
        yield return new WaitForSeconds(duration);
        ragdollMain.moveSpeed /= multiplier;
    }
}
