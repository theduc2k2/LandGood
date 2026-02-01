using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementAdapter : MonoBehaviour, IMovement
{
    public float speed = 1.5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void Move(Vector3 target)
    {
        Vector2 dir = ((Vector2)target - rb.position).normalized;
        rb.velocity = dir * speed;
    }

    public bool HasReached(Vector3 target)
    {
        if (Vector2.Distance(rb.position, target) < 0.1f)
        {
            rb.velocity = Vector2.zero;
            return true;
        }
        return false;
    }

    public Vector2 CurrentVelocity => rb.velocity; // ✅ FIX
}
