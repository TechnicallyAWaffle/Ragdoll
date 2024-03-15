using UnityEngine;

public class CageControl : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Check if the colliding object is the RagdollHead
            if (collision.gameObject.CompareTag("RagdollHead"))
            {
                Debug.Log("Collided with RagdollHead");
                // Destroy this obstacle object
                Destroy(gameObject);
            }
        }
    }
}
