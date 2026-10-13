using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public float activateDistance = 12f;
    public int direction = -1;

    private Rigidbody2D rb;
    private bool active = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!active && Mathf.Abs(transform.position.x - Camera.main.transform.position.x) < activateDistance)
            active = true;
    }

    void FixedUpdate()
    {
        if (!active) return;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 n = collision.GetContact(i).normal;
            if (Mathf.Abs(n.x) > 0.5f && Mathf.Sign(n.x) != direction)
            {
                direction = -direction;
                return;
            }
        }
    }
}
