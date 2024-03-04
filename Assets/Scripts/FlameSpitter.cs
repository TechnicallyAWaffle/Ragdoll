using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpitter : MonoBehaviour
{

    public SpriteRenderer flameSprite;
    public CapsuleCollider2D flameHitbox;

    private float timerPause;
    public float pauseDuration;
    private float timerFlame;
    public float flameDuration;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        flameSprite.enabled = false;
        flameHitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            timerPause += Time.deltaTime;
        }

        if (isPaused)
        {
            timerFlame += Time.deltaTime;
        }


        if (timerPause > pauseDuration)
        {
            flameHitbox.enabled = true;
            flameSprite.enabled = true;

            Debug.Log("LappedFlame");

            timerPause = 0f;

            isPaused = true;
        }

        if (timerFlame > flameDuration)
        {
            flameSprite.enabled = false;
            flameHitbox.enabled = false;

            Debug.Log("LappedPause");

            timerFlame = 0f;

            isPaused = false;
        }
    }
}
