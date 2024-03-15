using System.Collections;
using UnityEngine;

public class TreatCrumbs : Item
{
    public TreatCrumbs() : base(5, "Heals RagdollMain by a magical amount.", "TreatCrumbs", 1f, 0f, 1)
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
        // Since duration is 0, there's nothing to reset after a delay.
        ItemManager.Instance.Deactivate(this);
        yield break; // Alternatively, you could implement logic here if needed.
    }
}
