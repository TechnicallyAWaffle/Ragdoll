using UnityEngine;

public class Vice : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the Vice
    public float tetherLength = 5f; // Maximum length of the tether
    public float maxSeekRadius = 7.5f; // Maximum radius within which Vice will seek the player

    public LayerMask obstacleLayer; // Layer to check for obstacles/corners
    public bool requireLineOfSight = true; // Flag to require line of sight
    public Vector2 centerPosition; // The center position to return to
    public bool developerMode = true; // Toggle for developer mode

    private Transform player; // Reference to the player's transform
    private Rigidbody2D rb; // Rigidbody component of the Vice
    private Vector2 currentDestination;
    public bool hasHead;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerPosition = transform.position;
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("RagdollHead")) {
          player = GameObject.FindGameObjectWithTag("RagdollHead").transform;
        } else {
          player = GameObject.FindGameObjectWithTag("Ragdoll").transform;
        }

        // Calculate distance to the player
        float distanceToPlayer = Vector2.Distance(player.position, centerPosition);

        // test if the vice has the head still
        hasHead = updateHasHead();

        // Check for line of sight and distance to player
        if (((distanceToPlayer <= maxSeekRadius && IsPlayerInLineOfSight()) || (distanceToPlayer <= maxSeekRadius && !requireLineOfSight)) && !hasHead)
        {
            MoveTowardPlayer();
        }
        else
        {
            returnToCenter();
        }

        Debug.DrawLine((Vector2)transform.position, currentDestination, Color.magenta);
        Debug.DrawLine((Vector2)centerPosition, currentDestination, Color.magenta);
    }

    bool updateHasHead() {
      GameObject ragdollObject = GameObject.FindGameObjectWithTag("Ragdoll");
      RagdollMain ragdollMain = ragdollObject.GetComponent<RagdollMain>();
      return ragdollMain.headCapturedByVice;
    }

    void MoveTowardPlayer()
    {
        float distanceToPlayer = Vector2.Distance(player.position, centerPosition);
        
        // If outside tetherLength, stop moving by setting velocity to zero
        if (distanceToPlayer > tetherLength)
        {
            rb.velocity = Vector2.zero; // Stop the enemy's movement
        }
        else
        {
            // Calculate direction towards the player and move
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * moveSpeed;
            currentDestination = player.position; // Update current destination
        }
    }

    bool IsPlayerInLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, maxSeekRadius, obstacleLayer);
        if (hit.collider != null && hit.transform == player)
        {
            return true; // Player is in line of sight
        }
        return false; // Player is not in line of sight
    }

    void returnToCenter()
    {
        GameObject ragdollHead = GameObject.FindGameObjectWithTag("RagdollHead");
        float distanceToCenter = Vector2.Distance(centerPosition, rb.position);
        
        if (distanceToCenter <= 0.1f) // If within a radius of 0.1, stop moving
        {
            rb.velocity = Vector2.zero; // Stop the movement by setting velocity to zero
            if (hasHead && ragdollHead) {
              Rigidbody2D ragdollHeadRb = ragdollHead.GetComponent<Rigidbody2D>();
              ragdollHeadRb.velocity = Vector2.zero;
            }
        }
        else
        {
            Vector2 directionToCenter = (centerPosition - rb.position).normalized;
            rb.velocity = directionToCenter * (moveSpeed / 2); // Move towards the center at half speed
            if (hasHead && ragdollHead) {
              Rigidbody2D ragdollHeadRb = ragdollHead.GetComponent<Rigidbody2D>();
              ragdollHeadRb.velocity = directionToCenter * (moveSpeed / 2);
            }
        }
        currentDestination = centerPosition;
    }


    private void OnDestroy()
    {
        // Ensure the velocity is reset to zero when the Vice is destroyed to prevent continued movement
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider is your ragdoll head or the Rigidbody
        if (collision.gameObject.CompareTag("RagdollHead") || collision.gameObject.CompareTag("Ragdoll"))
        {
            // get ragdoll head and start returning to center
            Debug.Log("Collision");
            GameObject ragdollObject = GameObject.FindGameObjectWithTag("Ragdoll");
            RagdollMain ragdollMain = ragdollObject.GetComponent<RagdollMain>();
            ragdollMain.TakeDamage(this.gameObject);
        }
    }

    void OnDrawGizmos()
    {
      if (developerMode || !Application.isPlaying)
      {
        if (Application.isPlaying) {
          Gizmos.color = new Color(0, 1, 0, 1);
          Gizmos.DrawWireSphere(centerPosition, maxSeekRadius);
          Gizmos.color = new Color(1, 0, 0, 1);
          Gizmos.DrawWireSphere(centerPosition, tetherLength);
        } else {
          Gizmos.color = new Color(0, 1, 0, 1);
          Gizmos.DrawWireSphere(transform.position, maxSeekRadius);
          Gizmos.color = new Color(1, 0, 0, 1);
          Gizmos.DrawWireSphere(transform.position, tetherLength);
        }
      }
    }
}