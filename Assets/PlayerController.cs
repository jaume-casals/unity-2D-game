using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public int targetFrameRate = 60;

    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Sprite normalSprite;
    public Sprite runningSprite;
    public Sprite crouchingSprite;
    public Sprite jumpingSprite;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isJumping = false;
    private long frameCounter = 0;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFrameRate;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        HandleMovement();
        HandleJumping();
        HandleRunning();
        HandleCrouching();
        UpdateSprite();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float speed = moveSpeed;

        if (isRunning)
        {
            speed = runSpeed;
        }
        else if (isCrouching)
        {
            speed = crouchSpeed;
        }

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void HandleJumping()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            frameCounter = 0;
        }
        else if (isGrounded && frameCounter > 10)
        {
            isJumping = false;
        }

        frameCounter++;

    }

    void HandleRunning()
    {
        bool pressingShift = Input.GetKey(KeyCode.LeftShift);
        isRunning = (!pressingShift && isRunning && !isGrounded) || (pressingShift && (isRunning || isGrounded));
    }

    void HandleCrouching()
    {
        bool pressingCtrl = Input.GetKey(KeyCode.LeftControl);
        isCrouching = (!pressingCtrl && isCrouching && !isGrounded) || (pressingCtrl && (isCrouching || isGrounded));
    }
    
    void UpdateSprite()
    {
        if (isJumping)
        {
            spriteRenderer.sprite = jumpingSprite;
        }
        else if (isCrouching)
        {
            spriteRenderer.sprite = crouchingSprite;
        }
        else if (isRunning)
        {
            spriteRenderer.sprite = runningSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
