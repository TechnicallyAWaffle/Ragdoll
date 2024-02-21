using UnityEngine;
using System.Collections;

public class Scuttler : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private bool movingLeft = false;
    private Rigidbody2D rb;


    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    void Update()
    {
        rb.velocity = new Vector2(movingLeft ? -moveSpeed : moveSpeed, rb.velocity.y);

        // Determine the direction of the raycast based on the movement direction
        Vector2 rayDirection = movingLeft ? new Vector2(-1, -5) : new Vector2(1, -5);
        rayDirection.Normalize(); // Normalize the vector to ensure it has a consistent magnitude

        // Calculate the rightmost x and y positions based on the object's bounds
        Bounds bounds = spriteRenderer.bounds; // Assuming the object has a SpriteRenderer
        float rightmostX = bounds.max.x;
        float middleY = (bounds.min.y + bounds.max.y) / 2f;

        // Adjust the startPosition based on the rightmost x and y, but consider the direction
        Vector2 startPosition = new Vector2(rightmostX, middleY);

        // If moving left, you might want to adjust the x to the leftmost side
        if (movingLeft)
        {
            startPosition.x = bounds.min.x; // Use the leftmost x position when moving left
        }

        // Cast a ray at a 45-degree angle downwards in the direction of movement to check for ground
        RaycastHit2D groundInfo = Physics2D.Raycast(startPosition, rayDirection, 2f, groundLayer);
        // Cast a ray forward to check for a wall
        RaycastHit2D wallInfo = Physics2D.Raycast(startPosition, movingLeft ? Vector2.left : Vector2.right, 0.25f, wallLayer);

        // Debug.Log(startPosition);
        Debug.DrawLine(startPosition, startPosition + rayDirection * 2f, Color.red);
        Debug.DrawLine(startPosition, startPosition + (movingLeft ? Vector2.left : Vector2.right) * 0.25f, Color.blue);

        // Check if there is no ground ahead in the direction of movement or there is a wall ahead
        if (!groundInfo.collider || wallInfo.collider)
        {
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        movingLeft = !movingLeft;

        // Change to the turning sprite before flipping direction

        // Use a coroutine to wait for 2 seconds before flipping the sprite back
        StartCoroutine(ChangeDirectionAfterDelay(2f));
    }

    IEnumerator ChangeDirectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // After waiting, set the sprite based on the new direction

        // This also handles the rotation if you're using sprites to indicate direction instead of rotating the GameObject
        transform.eulerAngles = new Vector3(0, movingLeft ? 180 : 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider is your ragdoll head or the Rigidbody
        if (collision.gameObject.CompareTag("RagdollHead") || collision.gameObject.CompareTag("Ragdoll"))
        {
            // get ragdoll game object
            Debug.Log("Collision");
            GameObject ragdollObject = GameObject.FindGameObjectWithTag("Ragdoll");
            RagdollMain ragdollMain = ragdollObject.GetComponent<RagdollMain>();
            ragdollMain.TakeDamage(this.gameObject);
        }
    }
}
