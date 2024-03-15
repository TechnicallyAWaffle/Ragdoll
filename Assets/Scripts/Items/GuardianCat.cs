using System.Collections;
using UnityEngine;

public class GuardianCat : Item
{
    private int originalMaxHealth;

    public GuardianCat() : base(20, "Temporarily increases RagdollMain's max health to 9.", "GuardianCat", 9f, 15f, 1)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        // Save the original max health before changing
        originalMaxHealth = ragdollMain.healthManager.getMaxHealth();

        // Set the new max health to 9
        ragdollMain.healthManager.setMaxHealth((int)multiplier);

        // Start the coroutine to reset the max health after 15 seconds
        StartCoroutine(Reset(ragdollMain));
    }

    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        // Wait for the duration of the powerup effect
        yield return new WaitForSeconds(duration);

        // Reset the max health to its original value
        ragdollMain.healthManager.setMaxHealth(originalMaxHealth);
    }
}
