using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 6.0f;
    // sprint here if i decide to implement it

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float groundCheckRadius = 0.2f;   // adjusts how far the JumpCheck has to search for ground,, adjustable for tweaks
    [SerializeField] private LayerMask groundLayer;
    private Transform groundCheck;   // the invisible box child'd under the player that detects collision with ground
    private bool isGrounded;          // cant jump in mid-air (while falling or jumping)

    [Header("Misc")]
    private Rigidbody2D rb;
    private float moveInput;

    [Header("Scale")]
    private Vector3 startingScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Transform child = transform.Find("GroundCheck");
        if (child != null) groundCheck = child;
        else Debug.Log("Player can't find GroundCheck child.");

        startingScale = transform.localScale;         // the source sprite has *unique* proportions, so this scaling step ensures it displays correctly in-game.
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);        // continuously checks and updates check based on if our jump check object is touching the ground layer or not
        HandleFlip();

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if (!isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Can't jump, not grounded.");
        }
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * movementSpeed, rb.linearVelocity.y);
    }
    private void HandleFlip()
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(
                Mathf.Abs(startingScale.x),
                startingScale.y,
                startingScale.z
            );
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(
                -Mathf.Abs(startingScale.x),
                startingScale.y,
                startingScale.z
            );
        }
    }
}
