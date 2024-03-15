using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // Member variables
    public Guid uniqueID = Guid.NewGuid();
    public int price;
    public int grade;
    public string description;
    public string type;
    public float multiplier;
    public float duration;
    public Coroutine resetCoroutine;

    // Constructor to set basic item properties
    protected Item(int price, string description, string type, float multiplier, float duration, int grade)
    {
        this.price = price;
        this.description = description;
        this.type = type;
        this.multiplier = multiplier;
        this.duration = duration;
        this.grade = grade;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.CompareTag("Ragdoll") || collider.gameObject.CompareTag("RagdollHead")) && this.type != "Catnip")
        {
            // Try adding the item to the ItemManager
            if (ItemManager.Instance.AddPowerup(this))
            {
                // If successful, disable the GameObject here or within ItemManager
                // gameObject.SetActive(false); // Disable this item's GameObject
            }
        }
        else if (this.type == "Catnip")
        {
            GameObject ragdollObject = GameObject.FindGameObjectWithTag("Ragdoll");
            if (ragdollObject != null)
            {
                RagdollMain ragdollMain = ragdollObject.GetComponent<RagdollMain>();
                if (ragdollMain != null)
                {
                    this.Use(ragdollMain); // Use the Catnip item instantly
                }
            }
        }
    }


    // Abstract function to be implemented by subclasses
    public abstract void Use(RagdollMain ragdollMain);

    public abstract IEnumerator Reset(RagdollMain ragdollMain);

    // Getter functions
    public Guid GetId() => uniqueID;
    public int GetPrice() => price;
    public string GetDescription() => description;
    new public string GetType() => type;
    public float GetMultiplier() => multiplier;
    public float GetDuration() => duration;
}