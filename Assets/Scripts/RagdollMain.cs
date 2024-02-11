using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RagdollMain : MonoBehaviour
{

    public Animator animator;
    public Rigidbody2D rbHead;
    public SpriteRenderer spriteRenderer;
    public Vector3 headVelocity;
    private Vector3 headPositionLastFrame;
    

    [SerializeField]
    public float health;
    public float moveSpeed;
    public float jumpPower;
    public bool flipped = false;
    public bool isGrounded = false;
    public bool isSeparated = false;
    public bool headGrabbed = false;
    public Sprite ragDollBodySprite;
    public Sprite ragDollWholeSprite;
    public Vector2 movementInput = Vector2.zero;

    [SerializeField]
    public GameObject ragdollHead;
    public Rigidbody2D rb;

    public struct playerActions
    {
        public InputActions inputActions;
        public InputAction movement; // Default: WASD
        public InputAction reattach; // Default: Right Mouse Button
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
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rbHead = ragdollHead.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        PlayerInput playerInput = GetComponent<PlayerInput>();

        playerControls.inputActions = new InputActions();
        playerControls.inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = playerControls.movement.ReadValue<Vector2>();

        if (isGrounded == true) rb.velocity = new Vector2(movementInput.x * moveSpeed, rb.velocity.y);
        else rb.velocity = new Vector2(movementInput.x * (moveSpeed / 2), rb.velocity.y);

        if (movementInput.x > 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (movementInput.x < 0) transform.localScale = new Vector3(1, 1, 1);
    }

    private void FixedUpdate()
    {
        // Debug.Log(headPositionLastFrame);
        if (headGrabbed == true)
        {
            headVelocity = (ragdollHead.transform.position - headPositionLastFrame) / Time.deltaTime;
            headPositionLastFrame = ragdollHead.transform.position;
            ragdollHead.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            
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
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "RagdollHead")
                {
                    headGrabbed = true;
                    rbHead.gravityScale = 0;
                }
                
                else if (isSeparated == false && hit.transform.gameObject.tag == "Ragdoll")
                {
                    isSeparated = true;
                    ragdollHead.SetActive(true);
                    spriteRenderer.sprite = ragDollBodySprite;
                    headPositionLastFrame = ragdollHead.transform.position;
                    headGrabbed = true;
                    rbHead.gravityScale = 0;
                }
            }
        }
    }

    public void ReleaseHead(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            headGrabbed = false;
            rbHead.gravityScale = 3;
            rbHead.velocity = headVelocity;
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

}
