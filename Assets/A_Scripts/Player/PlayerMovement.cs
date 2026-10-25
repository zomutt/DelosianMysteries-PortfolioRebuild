using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Jump = Animator.StringToHash("Jump");

    /// <summary>
    /// This script purely handles player movement and jumping. Please see PlayerCombat.cs for combat-related mechanics, and PlayerStats.cs for player stats and health management.
    /// </summary>
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 6.0f;
    private float currentSpeed;
    // TODO: Sprint
    private Vector2 moveInput;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private LayerMask groundLayer;
    private readonly float groundCheckRadius = 0.2f; // how far the ground check searches for ground, adjustable for tweaks
    private Transform groundCheck;                   // the invisible child under the player that detects collision with ground
    private bool isGrounded;                         // can't jump in midair (while falling or jumping)
    public bool canMove = true;
    
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Scale")]
    private Vector3 startingScale;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Transform child = transform.Find("GroundCheck");
        if (child) groundCheck = child;
        else Debug.Log("Player can't find GroundCheck child.");

        startingScale =
            transform.localScale; // Quite literally everything will break without this. I'm not joking.
    }

    private void Update()
    {
        // Continuously checks and updates check based on if our jump check object is touching the ground layer or not.
        if (groundCheck)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = false;
        }
        // makes sure player is facing the correct way
        HandleFlip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * movementSpeed, rb.linearVelocity.y);
        currentSpeed = rb.linearVelocity.magnitude;
        if (isGrounded) animator.SetFloat(Walk, currentSpeed);
        else  animator.SetFloat(Walk, 0);
    }

    private void OnMove(InputValue value)
    {
        if (!canMove) return;
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if(!canMove || !isGrounded)  return;
        rb.linearVelocity = Vector2.up * jumpForce;
        animator.SetTrigger(Jump);
    }

    private void HandleFlip()        // Left/right facing
    {
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(startingScale.x),
                startingScale.y,
                startingScale.z
            );
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(startingScale.x),
                startingScale.y,
                startingScale.z
            );
        }
    }
}
