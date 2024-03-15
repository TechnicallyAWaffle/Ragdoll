using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public HealthSystem health;
    public GameObject heartContainer; // The UI container for the hearts
    public GameObject heartPrefab; // The prefab for the heart UI element
    public Sprite emptyHeart;
    public Sprite fullHeart;

    private List<GameObject> heartList = new List<GameObject>(); // A list to keep track of the instantiated heart UI elements

    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Ragdoll").GetComponent<HealthSystem>();
        UpdateHeartDisplay(); // Initial update on start
    }

    void Update()
    {
        UpdateHeartDisplay(); // Keep the display updated in case of changes
    }

    public void UpdateHeartDisplay()
    {
        int maxHealth = health.getMaxHealth();
        int currentHealth = health.getCurrentHealth();
        int spacing = 90; // Set the spacing between hearts

        // Check and instantiate heart UI elements if needed
        while (heartList.Count < maxHealth)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer.transform);
            // Calculate the position for the new heart based on the index
            newHeart.GetComponent<RectTransform>().anchoredPosition = new Vector2(-900 + heartList.Count * spacing, 487);
            heartList.Add(newHeart);
        }

        // Update existing hearts (deactivate excess and update the sprites)
        for (int i = 0; i < heartList.Count; i++)
        {
            bool isActive = i < maxHealth;
            heartList[i].SetActive(isActive);

            if (isActive)
            {
                // Update the position of the heart in case max health has decreased
                heartList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-900 + i * spacing, 487);

                // Update the sprite based on current health
                Image heartImage = heartList[i].GetComponent<Image>();
                heartImage.sprite = (i < currentHealth) ? fullHeart : emptyHeart;
            }
        }
    }
}
