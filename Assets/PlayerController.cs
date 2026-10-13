using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float runSpeed= 13f;
    public float jumpForce = 12f;
    public float invincibleTime = 2f;
    public int extraJumpsValue = 1; 
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isBig = false;
    private float invincibleUntil = 0f;
    private SpriteRenderer sr;
    private int extraJumps;
    private Animator anim;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        extraJumps = extraJumpsValue;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = CheckGrounded();

        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (extraJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                extraJumps--;
            }
        }
        if (Time.time < invincibleUntil)
            sr.enabled = (int)(Time.time * 10) % 2 == 0;
        else
            sr.enabled = true;
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetBool("Grounded", isGrounded);
        if (moveInput != 0) sr.flipX = moveInput < 0;
    }

    void FixedUpdate()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        float gravity = Physics2D.gravity.y * rb.gravityScale;
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * gravity * (fallMultiplier - 1) * Time.fixedDeltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector2.up * gravity * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;

        float leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero).x + 0.5f;
        if (rb.position.x < leftEdge)
        {
            rb.position = new Vector2(leftEdge, rb.position.y);
            if (rb.velocity.x < 0) rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            Destroy(collision.gameObject);
            Grow();
            return;
        }

        if (collision.gameObject.CompareTag("Block") && collision.GetContact(0).normal.y < -0.5f)
        {
            collision.gameObject.GetComponent<QuestionBlock>().Hit();
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.name == "Enemy")
        {
            if (collision.GetContact(0).normal.y > 0.5f)
            {
                Destroy(collision.gameObject);
                CoinManager.instance.AddScore(100);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce / 1.5f);
            }
            else
            {
                TakeDamage();
            }
        }
    }

    void Grow()
    {
        CoinManager.instance.AddScore(1000);
        if (isBig) return;
        isBig = true;
        transform.localScale = new Vector3(1, 1, 1);
        transform.position += Vector3.up * 0.5f;
    }

    void TakeDamage()
    {
        if (Time.time < invincibleUntil) return;

        if (isBig)
        {
            isBig = false;
            transform.localScale = new Vector3(1, 0.5f, 1);
            invincibleUntil = Time.time + invincibleTime;
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    bool CheckGrounded()
    {
        Bounds b = col.bounds;
        Vector2 feet = new Vector2(b.center.x, b.min.y);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.1f);

        Collider2D[] hits = Physics2D.OverlapBoxAll(feet, size, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Ground") || hit.CompareTag("Block"))
                return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c == null) return;
        Bounds b = c.bounds;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(new Vector3(b.center.x, b.min.y, 0), new Vector3(b.size.x * 0.9f, 0.1f, 0));
    }
}