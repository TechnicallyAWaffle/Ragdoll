using System.Collections;
using UnityEngine;

public class BagOfTreats : Item
{
    public BagOfTreats() : base(15, "Restores a large amount of health to RagdollMain.", "BagOfTreats", 3f, 0f, 1)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        // Assuming ragdollMain has a method to heal
        ragdollMain.healthManager.heal((int)multiplier);
    }

    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        // No reset logic required due to instant effect
        yield break;
    }
}
