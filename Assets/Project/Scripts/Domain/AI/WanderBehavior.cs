using UnityEngine;

public class WanderBehavior : IAIBehavior
{
    private readonly IMovement movement;
    private readonly ITargetProvider targetProvider;

    private Vector3 target;

    public WanderBehavior(IMovement movement, ITargetProvider targetProvider)
    {
        this.movement = movement;
        this.targetProvider = targetProvider;
    }

    public void OnEnter()
    {
        target = targetProvider.GetTarget(); // 👈 TẠO TARGET MỚI MỖI LẦN
    }

    public void Execute(float deltaTime)
    {
        movement.Move(target);
    }

    public Vector2 CurrentVelocity => movement.CurrentVelocity;

    public bool IsComplete => movement.HasReached(target);
}
