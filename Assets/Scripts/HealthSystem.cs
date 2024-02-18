using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private int currentHealth = 3;
    private int maxHealth = 3;
    [SerializeField] GameObject ragdoll;
    public SpriteRenderer playerBody;
    public SpriteRenderer playerHead;
    public SpriteRenderer playerBowLeft;
    public SpriteRenderer playerBowRight;
    public RagdollMain playerMovement;

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void checkHealth()
    {
        if (currentHealth <= 0)
        {
            // call fuction to reset room
            Debug.Log("Dead");
            currentHealth = 0;
            playerBody.enabled = false;
            playerHead.enabled = false;
            playerBowLeft.enabled = false;
            playerBowRight.enabled = false;
            playerMovement.enabled = false;
            //Destroy(ragdoll);
            //ragdoll.SetActive(false);
            // Debug.Log("I LIVE");
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // prevents health from exceeding max
        }
    }

    public int getCurrentHealth() { return currentHealth; }
    public int getMaxHealth() { return maxHealth; }

    public void hurt(int damage)
    {
        currentHealth -= damage;
        checkHealth();
        Debug.Log("Ow");
    }

    public void heal(int recover)
    {
        currentHealth += recover;
        checkHealth();
    }

    public void setMaxHealth(int max)
    {
        currentHealth = max;
        checkHealth();
    }
}
