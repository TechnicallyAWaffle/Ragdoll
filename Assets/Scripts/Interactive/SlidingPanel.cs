using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SlidingPanel : MonoBehaviour
{
    public Vector3 movementDelta; // Delta to apply when activating
    private Vector3 initialPosition; // Store the initial position of the panel
    public float speed = 1.0f; // Speed of movement

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Coroutine moveCoroutine;

    private void Start()
    {
        initialPosition = transform.position; // Store the initial position at start

        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    // Method to "activate" the panel, e.g., slide out by the delta from the initial position
    public void Activate()
    {
        targetPosition = initialPosition + movementDelta; // Set target to initial position + delta
        StartMovement();
    }

    // Method to "deactivate" the panel, e.g., return to initial position
    public void Deactivate()
    {
        targetPosition = initialPosition; // Set target back to the initial position
        StartMovement();
    }

    private void StartMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // Stop any ongoing movement coroutine
        }
        moveCoroutine = StartCoroutine(MovePanel());
    }

    IEnumerator MovePanel()
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        transform.position = targetPosition; // Ensure the panel is exactly at the target position
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Ragdoll") || collision.gameObject.CompareTag("RagdollHead")) && isMoving)
        {
            StopCoroutine(moveCoroutine); // Stop moving
            StartCoroutine(PauseMovement());
        }
    }

    IEnumerator PauseMovement()
    {
        isMoving = false;
        yield return new WaitForSeconds(3); // Wait for 3 seconds
        StartMovement(); // Resume movement
    }
}
