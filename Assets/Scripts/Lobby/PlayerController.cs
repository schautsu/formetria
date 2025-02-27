using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 8f, jumpForce = 8f;
    [SerializeField]
    private LayerMask jumpableGround;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer srend;
    private Animator anim;

    private float dirX = 0f;
    private Vector3 startPos;

    private enum MovementState { idle, running, jumping, falling }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        coll = GetComponent<BoxCollider2D>();
        srend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        dirX = SimpleInput.GetAxisRaw("Horizontal");
            
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y); 

        if (SimpleInput.GetButton("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            srend.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            srend.flipX = true;
        }
        else state = MovementState.idle;

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level"))
        {
            SceneController.Instance.LoadNewScene(collision.GetComponent<Checkpoint>().sceneIndex);
        }
    }
}