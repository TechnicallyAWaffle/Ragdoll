using System;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ragdoll") || collision.gameObject.CompareTag("RagdollHead"))
        {
            GameObject ragdollObject = GameObject.FindGameObjectWithTag("Ragdoll");
            if (ragdollObject != null)
            {
                RagdollMain ragdollMain = ragdollObject.GetComponent<RagdollMain>();
                if (ragdollMain != null)
                {
                    this.Use(ragdollMain);
                }
            }
        }
    }

    // Abstract function to be implemented by subclasses
    public abstract void Use(RagdollMain ragdollMain);

    // Getter functions
    public Guid GetId() => uniqueID;
    public int GetPrice() => price;
    public string GetDescription() => description;
    public string GetType() => type;
    public float GetMultiplier() => multiplier;
    public float GetDuration() => duration;
}