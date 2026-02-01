public interface IAIState
{
    void Enter(AIContext context);
    void Tick(AIContext context, float deltaTime);
    void Exit(AIContext context);

    bool CanTransition(AIContext context, out string nextState);
}
