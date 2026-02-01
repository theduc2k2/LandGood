using UnityEngine; 
public class IdleState : IAIState
{
    private float timer;
    private readonly float duration;

    public IdleState(float duration)
    {
        this.duration = duration;
    }

    public void Enter(AIContext context)
    {
        timer = duration;

        context.Velocity = Vector2.zero;
        context.AnimationState = AIAnimationState.IdleDown;
    }

    public void Tick(AIContext context, float deltaTime)
    {
        timer -= deltaTime;
    }

    public bool CanTransition(AIContext context, out string nextState)
    {
        if (timer <= 0f)
        {
            nextState = "Wander";
            return true;
        }

        nextState = null;
        return false;
    }

    public void Exit(AIContext context) { }
}
