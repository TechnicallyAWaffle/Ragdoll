using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatToy : Item
{
    private Dictionary<GameObject, float> originalSpeeds = new Dictionary<GameObject, float>();
    private Dictionary<GameObject, (float swingDuration, float swingTimer)> originalMaceStates = new Dictionary<GameObject, (float, float)>();

    public CatToy() : base(20, "Freezes all enemies for 10 seconds.", "Freeze", 1f, 10f, 1)
    {
    }

    public override void Use(RagdollMain ragdollMain)
    {
        // Freeze entities with moveSpeed
        FreezeEntitiesWithTag("Scuttler");
        FreezeEntitiesWithTag("Vice");
        // FreezeEntitiesWithTag("Flamespout");

        // Freeze SwingingMaces
        FreezeSwingingMaces();

        // Start the coroutine to unfreeze the entities after 10 seconds
        StartCoroutine(Reset(ragdollMain));
    }

    private void FreezeEntitiesWithTag(string tag)
    {
        var entities = GameObject.FindGameObjectsWithTag(tag);
        foreach (var entity in entities)
        {
            if (entity.TryGetComponent<MonoBehaviour>(out var component) && component.GetType().GetField("moveSpeed") != null)
            {
                float originalSpeed = (float)component.GetType().GetField("moveSpeed").GetValue(component);
                originalSpeeds[entity] = originalSpeed;
                component.GetType().GetField("moveSpeed").SetValue(component, 0f);
            }

            if (entity.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
        }
    }

    private void FreezeSwingingMaces()
    {
        var maces = GameObject.FindGameObjectsWithTag("SwingingMace");
        foreach (var mace in maces)
        {
            if (mace.TryGetComponent<SwingingMace>(out var maceComponent))
            {
                // Store the current state
                originalMaceStates[mace] = (maceComponent.swingDuration, maceComponent.swingTimer);

                // Set swingDuration to a very high number to freeze the mace in place
                maceComponent.swingDuration = float.MaxValue;
                // Stop the swingTimer from updating by effectively pausing it
                maceComponent.enabled = false;
            }
        }
    }

    public override IEnumerator Reset(RagdollMain ragdollMain)
    {
        yield return new WaitForSeconds(duration);

        // Unfreeze entities with moveSpeed
        foreach (var kvp in originalSpeeds)
        {
            if (kvp.Key.TryGetComponent<MonoBehaviour>(out var component) && component.GetType().GetField("moveSpeed") != null)
            {
                component.GetType().GetField("moveSpeed").SetValue(component, kvp.Value);
            }

            if (kvp.Key.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.isKinematic = false;
            }
        }

        // Unfreeze SwingingMaces
        foreach (var kvp in originalMaceStates)
        {
            if (kvp.Key.TryGetComponent<SwingingMace>(out var maceComponent))
            {
                // Restore the swingDuration and swingTimer
                maceComponent.swingDuration = kvp.Value.swingDuration;
                maceComponent.swingTimer = kvp.Value.swingTimer;

                // Re-enable the SwingingMace script to continue swinging
                maceComponent.enabled = true;
            }
        }

        // Clear dictionaries for the next use
        originalSpeeds.Clear();
        originalMaceStates.Clear();
    }
}
