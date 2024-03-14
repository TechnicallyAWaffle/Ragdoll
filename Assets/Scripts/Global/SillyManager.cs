using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SillyManager : MonoBehaviour
{
    [SerializeField] List<Sprite> titleScreenCharacters = new List<Sprite>();
    [SerializeField] GameObject titleScreenCharacterTemplate;
    [SerializeField] private float characterSpeed;

    private float maxtime = 5f;
    private float currentTime = 0f;
    private int currentIndex = 6;
    

    void Update()
    {
        currentTime -= Time.deltaTime;
        
        if (currentTime <= 0.0f)
        {
            Quaternion randRotation;
            Vector3 randHeading;
            Vector3 randPosition = GetRandomPosOffScreen();
            randRotation = Quaternion.Euler(0,0,Random.Range(-180,180));
            randHeading = Vector3.Normalize((new Vector3(Random.Range(-7, 7), Random.Range(-3, 3), -1) - randPosition)) * characterSpeed;
            GameObject character = Instantiate(titleScreenCharacterTemplate, randPosition, randRotation);
            character.GetComponent<TitleScreenCharacterMain>().Configure(randHeading, titleScreenCharacters[currentIndex]);
            if (currentIndex < titleScreenCharacters.Count - 1)
                currentIndex = 6;
            else
                currentIndex = 6;
            currentTime = maxtime;
        }
    }

    private Vector3 GetRandomPosOffScreen()
    {
        float x = Random.Range(-0.1f, 0.1f);
        float y = Random.Range(-0.1f, 0.1f);
        if(Mathf.Sign(x) > 0)
            x += Mathf.Sign(x);
        if (Mathf.Sign(y) > 0)
            y += Mathf.Sign(y);
        Vector3 randomPoint = new(x, y);

        randomPoint.z = 9f; 
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(randomPoint);

        return worldPoint;
    }
}
