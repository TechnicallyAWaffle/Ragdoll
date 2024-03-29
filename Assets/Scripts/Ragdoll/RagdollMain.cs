using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RagdollMain : MonoBehaviour, IManageable
{

    //Initialize
    public Animator animator;
    public Rigidbody2D rbHead;
    public SpriteRenderer spriteRenderer;
    public Vector3 headVelocity;
    private Vector3 headPositionLastFrame;
    private bool isDamaged = false;
    public float flashDuration = 0.2f;
    public float numFlashes = 3;
    public float pushForce = 5f;
    public bool flipped = false;
    public Sprite ragDollWholeSprite;
    private Rigidbody2D rb;
    private GroundChecker groundChecker;
    private InteractionManager interactionManager;
    private HealthSystem healthManager;


    public GameObject ragdollHead;
    public GameObject ragdollBody;
    public GameObject ragdollTieLeft;
    public GameObject ragdollTieRight;


    //Private Runtime Variables
    private bool isSeparated = false;
    private bool isLaunching = false;
    private bool headGrabbed = false;
    public bool headCapturedByVice = false;
    //private bool flipped = false;
    private float health;
    private float currentHeadClamp;
    private GameManager gameManager;

    //Public Runtime Variables 
    public Vector3 respawnPoint;


    //Serialize
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float headClamp;
    [SerializeField] private float headMeter;
    [SerializeField] private float headGravity;
    [SerializeField] private float headYOffset;
    [SerializeField] private float maxHeadMeter;
    [SerializeField] private float slingshotForceMultiplier;
    [SerializeField] private Sprite ragDollBodySprite;
    [SerializeField] private Sprite ragDollHeadSprite;
    [SerializeField] public Vector2 movementInput = Vector2.zero;
    [SerializeField] private GameObject headAnchor;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float minimumVisibleTetherRadius;
    [SerializeField] private float maximumTetherRadius;
    [SerializeField] private float tetherForce;

    public struct playerActions
    {
        public InputActions inputActions;
        public InputAction movement; // Default: WASD
        public InputAction reattach; // Default: Right Mouse Button
        public InputAction launchHead; 
        public InputAction grabHead; //Default: Left Mouse Button
        public InputAction jump; // Default: Space
        public InputAction interact; //Default: E
    }

    public playerActions playerControls;
    private void OnEnable()
    {
        playerControls.movement = playerControls.inputActions.Player.Move;
        playerControls.movement.Enable();

        playerControls.jump = playerControls.inputActions.Player.Jump;
        playerControls.jump.Enable();
        playerControls.jump.performed += Jump;

        playerControls.grabHead = playerControls.inputActions.Player.GrabHead;
        playerControls.grabHead.Enable();
        playerControls.grabHead.started += GrabHead;
        playerControls.grabHead.canceled += ReleaseHead;

        playerControls.reattach = playerControls.inputActions.Player.Reattach;
        playerControls.reattach.Enable();
        playerControls.reattach.started += ReattachHead;

        playerControls.interact = playerControls.inputActions.Player.Interact;
        playerControls.interact.Enable();
        playerControls.interact.performed += interactionManager.Interact;
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = ragdollBody.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundChecker = transform.Find("GroundChecker").GetComponent<GroundChecker>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        interactionManager = transform.Find("InteractionManager").GetComponent<InteractionManager>();
        healthManager = gameObject.GetComponent<HealthSystem>();

        playerControls.inputActions = new InputActions();
        playerControls.inputActions.Enable();

        headMeter = maxHeadMeter;
        currentHeadClamp = headClamp;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = playerControls.movement.ReadValue<Vector2>();
        
        if (movementInput.x != 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if (groundChecker.isGrounded == true) rb.velocity = new Vector2(movementInput.x * moveSpeed, rb.velocity.y);
        else rb.velocity = new Vector2(movementInput.x * (moveSpeed / 1.25f), rb.velocity.y);

        if (movementInput.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (movementInput.x < 0) transform.localScale = new Vector3(1, 1, 1);
        UpdateTether();
    }

    private void FixedUpdate()
    {
        if (headGrabbed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,15);
            Vector3 headAnchor = transform.position + new Vector3(0, headYOffset, 0);
            Vector3 mouseVector = (mousePosition - headAnchor);
            //Debug.Log("Mouse: " + mousePosition);
            //Debug.Log("Head: " + ragdollHead.transform.position);
            //Debug.DrawLine(headAnchor + new Vector3(0, headYOffset,0), mousePosition, Color.green);

            RaycastHit2D hit = Physics2D.Raycast(headAnchor, mouseVector.normalized, currentHeadClamp, 64);
            if (mouseVector.magnitude <= currentHeadClamp)
                ragdollHead.transform.position = mousePosition;
            else if (hit)
                ragdollHead.transform.position = hit.point;
            else
                ragdollHead.transform.position = headAnchor + mouseVector.normalized * currentHeadClamp;

            if (isSeparated)
            {
                headMeter -= (ragdollHead.transform.position - headPositionLastFrame).magnitude;
                headVelocity = (ragdollHead.transform.position - headPositionLastFrame) / Time.deltaTime;
                headPositionLastFrame = ragdollHead.transform.position;
                if (headMeter <= 0)
                {
                    headGrabbed = false;
                    rbHead.gravityScale = headGravity;
                }
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && groundChecker.isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    public void GrabHead(InputAction.CallbackContext context)
    {
        if (context.started && !headCapturedByVice)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit)
            {
                if (hit.transform.gameObject.tag == "Ragdoll" || hit.transform.gameObject.tag == "RagdollHead")
                {
                    if (isSeparated) //Pick up head after slingshot
                    {
                        headGrabbed = true;
                        rbHead.gravityScale = 0;
                    }
                    else //Slingshot 
                    {
                        ragdollHead.transform.parent = null;
                        headGrabbed = true;
                        isLaunching = true;
                        ragdollHead.SetActive(true);
                        //spriteRenderer.sprite = ragDollBodySprite;
                        headPositionLastFrame = ragdollHead.transform.position;
                        rbHead.bodyType = RigidbodyType2D.Dynamic;
                        rbHead.drag = 1;
                        rbHead.gravityScale = 0;
                    }
                }
            }
        }
    }

    public void ReleaseHead(InputAction.CallbackContext context)
    {
        if (context.canceled && !headCapturedByVice)
        {
            if (isSeparated && headMeter > 0) //Release head onto ground
            {
                headGrabbed = false;
                rbHead.gravityScale = headGravity;
                rbHead.velocity = headVelocity;
            }
            else if(isLaunching) //Release slingshot
            {
                isSeparated = true;
                currentHeadClamp = maximumTetherRadius;
                headGrabbed = false;
                isLaunching = false;
                rbHead.constraints = RigidbodyConstraints2D.None;
                rbHead.gravityScale = headGravity;
                rbHead.AddForce((headAnchor.transform.position - ragdollHead.transform.position) * ((headAnchor.transform.position - ragdollHead.transform.position).magnitude * slingshotForceMultiplier), ForceMode2D.Impulse);
                Debug.Log(headAnchor.transform.position * (headAnchor.transform.position - ragdollHead.transform.position).magnitude);
            }
        }
    }

    public void ReattachHead(InputAction.CallbackContext context)
    {
        ragdollHead.transform.parent = transform;
        isSeparated = false;
        headGrabbed = false;
        currentHeadClamp = headClamp;
        ragdollHead.transform.localPosition = new Vector3(-0.035f, 0.575f, 0); //Hardcoded offset since the sprite center is different from the imported sprite :(
        rbHead.bodyType = RigidbodyType2D.Kinematic;
        rbHead.constraints = RigidbodyConstraints2D.FreezeRotation;
        ragdollHead.transform.rotation = Quaternion.identity;
        rbHead.velocity = Vector3.zero;
        rbHead.gravityScale = 0;
        headMeter = maxHeadMeter;
        lineRenderer.enabled = false;
        headCapturedByVice = false;
    }

    private void UpdateTether()
    {
        Vector3 headAnchorPosition = headAnchor.transform.position;
        Vector3 headPosition = ragdollHead.transform.position;
        float distanceToBody = Vector3.Magnitude(headAnchorPosition - headPosition);
        rbHead.drag = 1;
        if (isSeparated)
        {
            if (distanceToBody >= minimumVisibleTetherRadius)
            {
                lineRenderer.enabled = true;
                Vector3[] positions = { headAnchorPosition, headPosition };
                lineRenderer.SetPositions(positions);
                
            }
            else lineRenderer.enabled = false;
        }
        if (!headGrabbed && distanceToBody >= maximumTetherRadius)
        {
            rbHead.AddForce((headAnchorPosition - headPosition).normalized * ((tetherForce * distanceToBody)), ForceMode2D.Force);
            rbHead.drag = 5;
        }
            
    }

    public void TakeDamage (GameObject enemy)
    {
        // probably remove health, flash sprite, and then do anything specific to enemy. push back will likely be enemy specific
        if (!isDamaged) {
            isDamaged = true;
            StartCoroutine(IFrames(1.0f));
            GameObject ragdoll = GameObject.FindGameObjectWithTag("Ragdoll");
            SpriteRenderer[] sprites = ragdoll.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                StartCoroutine(FlashSprite(sprite));
            }
            switch (enemy.tag) {
                case "Scuttler":
                    HitByScuttler (enemy);
                break;
                case "Vice":
                    HitByVice (enemy);
                break;
                default:
                break;
            }
        }
    }

    public void HitByScuttler (GameObject enemy)
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log("Hit by a cute lil scuttler");
            Bounds bounds = enemy.GetComponent<SpriteRenderer>().bounds; // Assuming the object has a SpriteRenderer

            float middleX = (bounds.min.x + bounds.max.x) / 2f;
            float middleY = (bounds.min.y + bounds.max.y) / 2f;
            Vector2 startPosition = new Vector2(middleX, middleY);
            Vector2 ragdollStartPosition = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 direction = (ragdollStartPosition - startPosition).normalized;
            if (direction.x > 0.8) direction = new Vector2 (0.8f, 0.6f);
            if (direction.x < -0.8) direction = new Vector2 (-0.8f, 0.6f);
            rb.position = new Vector2(rb.position.x, rb.position.y + 0.1f);
            groundChecker.isGrounded = false;
            // direction = new Vector2(1, 1);
            // rb.velocity = direction * pushForce;
            // Debug.Log(ragdollStartPosition);
            // Debug.Log(startPosition);
            // Debug.Log(pushForce);
            // Debug.Log(direction * pushForce);
            rb.AddForce(direction * pushForce, ForceMode2D.Impulse);
        }
    }

    public void HitByVice (GameObject enemy)
    {
        // take the head and release head and move to center
        Debug.Log("Hit by vice");
        isSeparated = true;
        headCapturedByVice = true;
        // headGrabbed = false;
        // rbHead.gravityScale = 0;
        // rbHead.velocity = Vector2.zero;
        // ragdollHead.SetActive(true);
        // headPositionLastFrame = ragdollHead.transform.position;
        ragdollHead.transform.parent = null;
        isLaunching = false;
        rbHead.constraints = RigidbodyConstraints2D.None;
        headGrabbed = false;
        rbHead.gravityScale = headGravity;
        rbHead.velocity = headVelocity;
    }

    public void TogglePlayerVisible(bool toggle)
    {
        Debug.Log("thisran");
        ragdollBody.GetComponent<SpriteRenderer>().enabled = toggle;
        ragdollHead.GetComponent<SpriteRenderer>().enabled = toggle;
        ragdollTieLeft.GetComponent<SpriteRenderer>().enabled = toggle;
        ragdollTieRight.GetComponent<SpriteRenderer>().enabled = toggle;
    }

    public void GetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
        interactionManager.ragdollMain = this;
        interactionManager.audioManager = gameManager.audioManager;
        interactionManager.ragdollHealth = healthManager;

    }

    public IEnumerator GoToCheckpoint(Vector3 targetPosition, Animator escortAnimator)
    {
        //Hardcoded offset
        targetPosition += new Vector3(1.24f, 0, 0);
        Debug.Log(targetPosition);
        animator.SetBool("isMoving", true);
        float movementDirection = (targetPosition - transform.position).x;
        if (movementDirection > 0)
            movementDirection = movementDirection / movementDirection;
        else
            movementDirection = movementDirection / -movementDirection;
        Debug.Log((targetPosition - transform.position).x);
        while (Mathf.Abs((transform.position.x - targetPosition.x)) > 0.5f)
        {
            rb.velocity = new Vector2(movementDirection * moveSpeed, 0);
            yield return new WaitForEndOfFrame();
            Debug.Log(Mathf.Abs((transform.position.x - targetPosition.x)));
        }
        escortAnimator.SetTrigger("CheckpointEnter");
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
    }

    IEnumerator FlashSprite(SpriteRenderer sprite)
    {
        for (int i = 0; i < numFlashes * 2; i++)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
        sprite.enabled = true; // Ensure sprite is visible after flashing
    }

    IEnumerator IFrames(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isDamaged = false;
    }
}
