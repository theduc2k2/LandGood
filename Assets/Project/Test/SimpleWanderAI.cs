using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SimpleWanderAI_Transform : MonoBehaviour
{
    // =========================
    // CONFIG
    // =========================
    [Header("Wander Area")]
    [SerializeField] private float wanderRadius = 5f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private float arriveDistance = 0.05f;

    [Header("Idle")]
    [SerializeField] private float idleMinTime = 0.8f;
    [SerializeField] private float idleMaxTime = 2.0f;

    // =========================
    // STATE
    // =========================
    private Rigidbody2D rb;
    private enum State
    {
        Idle,
        Moving
    }

    private State currentState;

    // =========================
    // COMPONENTS
    // =========================
    private Animator animator;
    private SpriteRenderer sprite;

    // =========================
    // DATA
    // =========================
    private Vector2 homePosition;
    private Vector2 target;
    private Vector2 moveDir;
    private float idleTimer;
    private bool facingUp = false;

    // Animation mapping (float)
    // 0 = Idle_Down
    // 1 = Idle_Up
    // 2 = Run_Down
    // 3 = Run_Up

    // =========================
    // UNITY
    // =========================
    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        homePosition = transform.position;

        EnterIdle(forceDown: true);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;

            case State.Moving:
                UpdateMoving();
                break;
        }

        ApplyMovement();
        UpdateAnimation();
        UpdateFlip();
    }

    // =========================
    // STATE LOGIC
    // =========================
    void EnterIdle(bool forceDown = false)
    {
        currentState = State.Idle;
        moveDir = Vector2.zero;
        idleTimer = Random.Range(idleMinTime, idleMaxTime);

        if (forceDown)
            facingUp = false;
    }

    void UpdateIdle()
    {
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            target = GetRandomWorldTarget();
            currentState = State.Moving;
        }
    }

    void UpdateMoving()
    {
        Vector2 currentPos = transform.position;
        Vector2 toTarget = target - currentPos;

        if (toTarget.magnitude <= arriveDistance)
        {
            EnterIdle();
            return;
        }

        moveDir = toTarget.normalized;

        if (Mathf.Abs(moveDir.y) > 0.01f)
            facingUp = moveDir.y > 0;
    }

    // =========================
    // MOVEMENT (TRANSFORM)
    // =========================
    void ApplyMovement()
    {
        if (moveDir == Vector2.zero)
            return;

        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);
        // rb.velocity = moveDir * moveSpeed;
    }

    // =========================
    // TARGET
    // =========================
    Vector2 GetRandomWorldTarget()
    {
        Vector2 random = Random.insideUnitCircle * wanderRadius;
        return homePosition + random;
    }

    // =========================
    // ANIMATION
    // =========================
    void UpdateAnimation()
    {
        bool moving = moveDir.magnitude > 0.01f;

        float moveValue;
        if (!moving)
            moveValue = facingUp ? 1f : 0f;
        else
            moveValue = facingUp ? 3f : 2f;

        animator.SetFloat("Move", moveValue);
    }

    void UpdateFlip()
    {
        if (Mathf.Abs(moveDir.x) > 0.01f)
            sprite.flipX = moveDir.x < 0f;
    }

#if UNITY_EDITOR
    // =========================
    // DEBUG
    // =========================
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            Application.isPlaying ? homePosition : (Vector2)transform.position,
            wanderRadius
        );

        if (currentState == State.Moving)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target, 0.12f);
        }
    }
#endif
}
