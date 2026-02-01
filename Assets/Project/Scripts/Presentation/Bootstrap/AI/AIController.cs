using UnityEngine;

public class AIController : MonoBehaviour
{
    public AIStateMachine StateMachine { get; private set; }

    void Awake()
    {
        var movement = GetComponent<IMovement>();
        var targetProvider = GetComponent<ITargetProvider>();

        if (movement == null || targetProvider == null)
        {
            Debug.LogError("AIController missing dependencies", this);
            enabled = false;
            return;
        }

        StateMachine = new AIStateMachine();

        StateMachine.Register("Idle", new IdleState(2f));
        StateMachine.Register(
            "Wander",
            new WanderState(new WanderBehavior(movement, targetProvider))
        );
    }

    void Start()
    {
        StateMachine.ChangeState("Idle");
    }

    void Update()
    {
        StateMachine.Tick(Time.deltaTime);
    }
}
