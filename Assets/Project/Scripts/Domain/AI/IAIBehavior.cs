using UnityEngine;

public interface IAIBehavior
{
    void OnEnter();                // 👈 THÊM
    void Execute(float deltaTime);

    Vector2 CurrentVelocity { get; }
    bool IsComplete { get; }
}
