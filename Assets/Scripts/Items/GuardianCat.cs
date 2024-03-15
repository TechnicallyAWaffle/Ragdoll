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
        originalMaxHealth = ragdollMain.healthManager.getMaxHealth();
        ragdollMain.healthManager.setMaxHealth((int)multiplier);

        // Safe to start the coroutine here and keep a reference to it in the base class
        StartCoroutine(Reset(ragdollMain));
    }

    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        yield return new WaitForSeconds(duration);
        ragdollMain.healthManager.setMaxHealth(originalMaxHealth);
        ItemManager.Instance.Deactivate(this);
    }
}
