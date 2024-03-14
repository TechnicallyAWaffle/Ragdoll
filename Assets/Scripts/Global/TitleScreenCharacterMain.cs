using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCharacterMain : MonoBehaviour
{
    public void Configure(Vector3 course, Sprite sprite)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        rb.velocity = course;
        rb.AddTorque(Random.Range(-1, 1), ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 50 || Mathf.Abs(transform.position.y) > 50)
            Destroy(gameObject);
    }
}
