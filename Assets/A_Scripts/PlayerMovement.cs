using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Idle = Animator.StringToHash("Idle");

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

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Scale")]
    private Vector3 startingScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Transform child = transform.Find("GroundCheck");
        if (child) groundCheck = child;
        else Debug.Log("Player can't find GroundCheck child.");

        startingScale =
            transform.localScale; // the source sprite has *unique* proportions, so this scaling step ensures it displays correctly in-game.
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
        // bool moving = Mathf.Abs(moveInput.x) > 0.01f;
        // if (moving) Debug.Log(moving);
        // animator.SetBool(Walk, moving);
        // animator.SetBool(Walk, !moving);
        
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * movementSpeed, rb.linearVelocity.y);
        currentSpeed = rb.linearVelocity.magnitude;
        animator.SetFloat(Walk, currentSpeed);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        rb.linearVelocity = Vector2.up * jumpForce;
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
