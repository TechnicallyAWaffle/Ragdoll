using System.Collections;
using UnityEngine;

public class FishTreat : Item
{
    public FishTreat() : base(10, "Restores a moderate amount of health to RagdollMain.", "FishTreat", 2f, 0f, 1)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        if (!IsActive) {
            IsActive = true;
            // Assuming ragdollMain has a method to heal
            ragdollMain.healthManager.heal((int)multiplier);
            StartCoroutine(Reset(ragdollMain));
        }
    }

    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        // No reset logic required due to instant effect
        ItemManager.Instance.Deactivate(this);
        yield break;
    }
}
