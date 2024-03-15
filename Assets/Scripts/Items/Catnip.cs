using System.Collections;
using UnityEngine;

public class Catnip : Item
{
    public Catnip() : base(10, "Completely recharges the head meter instantly.", "Catnip", 1f, 0f, 1)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        if (!IsActive) {
            IsActive = true;
        // Recharge the head meter to its maximum value
            Debug.Log ("Previous head meter: " + ragdollMain.headMeter);
            ragdollMain.headMeter = ragdollMain.maxHeadMeter;
            Debug.Log ("Post head meter: " + ragdollMain.headMeter);
            StartCoroutine(Reset(ragdollMain));
        }
    }

    // Since the effect is instant and does not have a duration, we do not need to implement Reset
    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        // ItemManager.Instance.RemovePowerup(this);
        // Destroy (gameObject);
        // No reset logic is required for an instant effect
        ItemManager.Instance.Deactivate(this);
        yield break;
    }
}
