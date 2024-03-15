using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private int numPowerups;
    private int maxPowerupsPerType;
    private List<List<Item>> storedPowerups;
    private List<Item> activePowerups;
    private RagdollMain ragdollMain;

    void Awake()
    {
        numPowerups = 7;
        maxPowerupsPerType = 3;
        storedPowerups = new List<List<Item>>(numPowerups);
        for (int i = 0; i < numPowerups; i++)
        {
            storedPowerups.Add(new List<Item>(maxPowerupsPerType)); // Initialize with inner lists
        }
        activePowerups = new List<Item>(numPowerups);
        for (int i = 0; i < numPowerups; i++)
        {
            activePowerups.Add(null); // Initialize all to null
        }
        GameObject ragdollObject = GameObject.FindGameObjectWithTag("Ragdoll");
        if (ragdollObject != null)
        {
            ragdollMain = ragdollObject.GetComponent<RagdollMain>();
        }
    }

    public void AddPowerup(Item powerup)
    {
        int index = GetPowerupIndex(powerup);
        Debug.Log("Adding powerup: " + powerup.type);
        if (index != -1 && storedPowerups[index].Count < maxPowerupsPerType)
        {
            storedPowerups[index].Add(powerup); // Add to the appropriate sublist
        } else if (index != -1) {
            Debug.Log("Invalid index");
        }
    }

    public void ActivatePowerup(int index)
    {
        if (ragdollMain == null || index < 0 || index >= storedPowerups.Count || storedPowerups[index] == null || storedPowerups[index].Count == 0)
        {
            Debug.Log ("Invalid index: " + index);
            return; // Invalid index or no powerups of this type
        }

        Item powerup = storedPowerups[index][0];
        Debug.Log("Activating powerup: " + powerup.type);

        storedPowerups[index].RemoveAt(0); // This will affect the index of remaining powerups
        activePowerups[index] = powerup; // Mark the powerup as active

        ragdollMain.ActivatePowerup(powerup);
        StartCoroutine(DeactivateAfterDelay(powerup, index));
    }

    private IEnumerator DeactivateAfterDelay(Item powerup, int index)
    {
        yield return new WaitForSeconds(powerup.duration);
        Debug.Log("Deactivating powerup: " + powerup.type);
        ragdollMain.DeactivatePowerup(powerup);
        activePowerups[index] = null;
        Destroy(powerup);
    }

    private int GetPowerupIndex(Item powerup)
    {
        switch (powerup.type)
        {
            case "TreatCrumbs": return 0;
            case "FishTreat": return 1;
            case "BagOfTreats": return 2;
            case "GuardianCat": return 3;
            case "CatToy": return 4;
            case "Catnip": return 5;
            case "Speed": return 6;
            default: return -1;
        }
    }
}
