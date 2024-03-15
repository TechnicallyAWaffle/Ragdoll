using UnityEngine;

public class SwingingMace : MonoBehaviour
{
    public float swingDuration = 2f; // Duration of one swing from side to side
    private float swingTimer = 0f; // Timer to track the progress of the swing
    private bool swingingRight = true; // Direction of the swing
    private float offsetFromCenter; // Dynamically calculated based on the object's dimensions

    private void Start()
    {
        CalculateOffsetFromCenter();
    }
    private void Update()
    {
        Swing();
    }
private void Swing()
{
    // Increment or decrement the timer based on the swing direction
    swingTimer += (swingingRight ? 1 : -1) * Time.deltaTime;

    // When the swing reaches its duration, reverse the direction
    if (swingTimer > swingDuration)
    {
        swingingRight = false;
        swingTimer = swingDuration;
    }
    else if (swingTimer < 0)
    {
        swingingRight = true;
        swingTimer = 0;
    }

    // Calculate the normalized time of the swing
    float normalizedTime = swingTimer / swingDuration;
    // Apply cubic easing in and out
    float angle = SwingCubicTween(normalizedTime) * 180f - 90f; // Adjust range to [-90, 90] degrees

    // Define the pivot point closer to the top of the mace
    Vector3 pivotPoint = transform.position + transform.up * offsetFromCenter; // 'offsetFromCenter' should be defined based on your object's size

    // Rotate around the new pivot point
    transform.RotateAround(pivotPoint, Vector3.forward, angle - transform.eulerAngles.z);
}


    private float SwingCubicTween(float t)
    {
        // Cubic easing in and out
        return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
    }

    private void CalculateOffsetFromCenter()
    {
        // Assuming the sprite renderer component is attached to the same GameObject
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Calculate offset to make pivot equidistant from sides and top
            // Use the smaller of the height or width to ensure the pivot is within the object's bounds
            float height = spriteRenderer.bounds.size.y;
            float width = spriteRenderer.bounds.size.x;

            if (height > width) {
              offsetFromCenter = height/2 - width / 2;
            } else {
              offsetFromCenter = width/2 - height / 2;
            }
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found. Ensure it's attached to the GameObject.");
        }
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
                  // Pass the collision point as well
                  Vector2 collisionPoint = collision.GetContact(0).point;
                  ragdollMain.TakeDamage(this.gameObject);
              }
          }
      }
  }


    void OnDrawGizmos()
    {
        // Set the color of the gizmo
        Gizmos.color = Color.blue;
        
        // Calculate the pivot point based on the current offsetFromCenter
        Vector3 pivotPoint = transform.position + transform.up * offsetFromCenter;

        // Draw a small sphere at the pivot point
        Gizmos.DrawSphere(pivotPoint, 0.1f); // Adjust the 0.1f value to change the size of the sphere if needed
    }
}
