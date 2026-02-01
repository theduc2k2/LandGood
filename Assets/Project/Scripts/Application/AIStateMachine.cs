using System.Collections.Generic;

public class AIStateMachine
{
    private readonly Dictionary<string, IAIState> states = new();
    private IAIState currentState;

    private readonly AIContext context = new();

    public AIContext Context => context;

    public void Register(string key, IAIState state)
    {
        states[key] = state;
    }

    public void ChangeState(string key)
    {
        currentState?.Exit(context);
        currentState = states[key];
        currentState.Enter(context);
    }

    public void Tick(float deltaTime)
    {
        currentState?.Tick(context, deltaTime);

        if (currentState != null &&
            currentState.CanTransition(context, out var next))
        {
            ChangeState(next);
        }
    }
}
