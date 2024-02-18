using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RagdollMain : MonoBehaviour
{

    //Initialize
    public Animator animator;
    public Rigidbody2D rbHead;
    public SpriteRenderer spriteRenderer;
    public Vector3 headVelocity;
    private Vector3 headPositionLastFrame;
    private Rigidbody2D rb;
    

    //Runtime Variables
    private bool isSeparated = false;
    private bool isLaunching = false;
    private bool headGrabbed = false;
    //private bool flipped = false;
    private bool isGrounded = false;
    private float health;
    private float currentHeadClamp;
    


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
    [SerializeField] private GameObject ragdollHead;
    [SerializeField] private GameObject ragdollBody;
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
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = ragdollBody.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerInput playerInput = GetComponent<PlayerInput>();

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

        if (isGrounded == true) rb.velocity = new Vector2(movementInput.x * moveSpeed, rb.velocity.y);
        else rb.velocity = new Vector2(movementInput.x * (moveSpeed / 2), rb.velocity.y);

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
            Debug.Log("Mouse: " + mousePosition);
            Debug.Log("Head: " + ragdollHead.transform.position);
            Debug.DrawLine(headAnchor + new Vector3(0, headYOffset,0), mousePosition, Color.green);

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
        if (context.performed && isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    public void GrabHead(InputAction.CallbackContext context)
    {
        if (context.started)
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
        if (context.canceled)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "LightPressurePlate":
                collision.gameObject.GetComponent<ITriggerable>().ActivateLinked();
                break;
            case "Recharger":
                headMeter = maxHeadMeter;
                Destroy(collision.gameObject);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "LightPressurePlate":
                collision.gameObject.GetComponent<ITriggerable>().DeactivateLinked();
                break;
        }
    }

}
