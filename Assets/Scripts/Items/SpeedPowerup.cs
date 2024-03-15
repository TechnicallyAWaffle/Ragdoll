using System;
using System.Collections;
using UnityEngine;

public class SpeedPowerup : Item
{
    public SpeedPowerup() : base(10, "Watch as the cat uses a mere fraction of his potential, gaining speed that humans can hardly witness.", "Speed", 2f, 5f, 1)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        if (!IsActive) {
            IsActive = true;
            ragdollMain.moveSpeed *= multiplier;
            StartCoroutine(Reset(ragdollMain));
        }
    }

    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        yield return new WaitForSeconds(duration);
        ragdollMain.moveSpeed /= multiplier;
        ItemManager.Instance.Deactivate(this);
    }
}
