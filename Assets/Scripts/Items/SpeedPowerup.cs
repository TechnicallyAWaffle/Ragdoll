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
    }

    private IEnumerator ResetSpeed(RagdollMain ragdollMain)
    {
        yield return new WaitForSeconds(duration);
        ragdollMain.moveSpeed /= multiplier;
    }
}