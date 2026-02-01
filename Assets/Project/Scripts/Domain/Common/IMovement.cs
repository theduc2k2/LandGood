using UnityEngine;

public interface IMovement
{
    void Move(Vector3 target);
    bool HasReached(Vector3 target);

    Vector2 CurrentVelocity { get; }
}
