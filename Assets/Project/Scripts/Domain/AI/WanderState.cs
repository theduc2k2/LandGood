using UnityEngine;
public class WanderState : IAIState
{
    private readonly IAIBehavior behavior;

    public WanderState(IAIBehavior behavior)
    {
        this.behavior = behavior;
    }

    public void Enter(AIContext context)
    {
        behavior.OnEnter(); // 👈 QUAN TRỌNG
    }

    public void Tick(AIContext context, float deltaTime)
    {
        behavior.Execute(deltaTime);

        Vector2 v = behavior.CurrentVelocity;
        context.Velocity = v;

        context.AnimationState =
            v.y >= 0
                ? AIAnimationState.RunUp
                : AIAnimationState.RunDown;
    }

    public bool CanTransition(AIContext context, out string nextState)
    {
        if (behavior.IsComplete)
        {
            nextState = "Idle";
            return true;
        }

        nextState = null;
        return false;
    }

    public void Exit(AIContext context)
    {
        context.Velocity = Vector2.zero;
    }
}
