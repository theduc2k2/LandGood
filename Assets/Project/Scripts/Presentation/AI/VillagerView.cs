using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class VillagerView : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;

    private AIStateMachine stateMachine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stateMachine = GetComponent<AIController>().StateMachine;
    }

    void Update()
    {
        var ctx = stateMachine.Context;

        animator.SetInteger("Move", (int)ctx.AnimationState);

        if (Mathf.Abs(ctx.Velocity.x) > 0.01f)
        {
            sprite.flipX = ctx.Velocity.x < 0;
        }
    }
}
