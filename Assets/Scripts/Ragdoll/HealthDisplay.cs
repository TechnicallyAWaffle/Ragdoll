using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public HealthSystem health;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] heartArray;


    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Ragdoll").GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < heartArray.Length; i++)
        {
            if (i < health.getCurrentHealth())
            {
                heartArray[i].sprite = fullHeart;
            }
            else
            {
                heartArray[i].sprite = emptyHeart;
            }
            if (i < health.getMaxHealth())
            {
                heartArray[i].enabled = true;
            }
            else
            {
                heartArray[i].enabled=false;
            }
        }
    }
}
