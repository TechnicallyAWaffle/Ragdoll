
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private Item[] storedPowerups = new Item[3]; // Array to hold up to 3 items

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddPowerup(Item powerup)
    {
        for (int i = 0; i < storedPowerups.Length; i++)
        {
            if (storedPowerups[i] == null)
            {
                storedPowerups[i] = powerup;
                Debug.Log("Powerup added: " + powerup.type);

                // Disable the sprite renderer to not display the sprite
                SpriteRenderer spriteRenderer = powerup.gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = false;
                }

                // Disable any colliders to prevent collision detection
                Collider2D collider = powerup.gameObject.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                return true; // Successfully added powerup
            }
        }
        Debug.Log("Inventory full, cannot add more powerups.");
        return false; // Inventory full
    }


    public void ActivatePowerup(int index)
    {
        if (index >= 0 && index < storedPowerups.Length && storedPowerups[index] != null)
        {
            Item powerup = storedPowerups[index];
            Debug.Log("Activating powerup: " + powerup.type);

            powerup.Use(GameManager.Instance.ragdollMain); // Activate the powerup
        }
        else
        {
            Debug.Log("Invalid slot index or no powerup in this slot.");
        }
    }

    public void Deactivate(Item powerup)
    {
        // Wait for the reset logic to complete
        Debug.Log("Deactivating powerup: " + powerup.type);

        RemovePowerup(powerup);

        // Ensure safe deactivation
        if (powerup != null && powerup.gameObject != null)
        {
            Destroy(powerup.gameObject); // Now safe to destroy
        }
    }

    public void RemovePowerup(Item powerup)
    {
        for (int i = 0; i < storedPowerups.Length; i++)
        {
            if (storedPowerups[i] == powerup)
            {
                storedPowerups[i] = null; // Remove the reference from the array
                Debug.Log("Powerup removed: " + powerup.type);
                break; // Exit the loop once the item is found and removed
            }
        }
    }
}