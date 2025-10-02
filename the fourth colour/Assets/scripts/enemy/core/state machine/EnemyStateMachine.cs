using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState{ get; set; }

    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.OnStateEnter();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentState.OnStateExit();
        CurrentState = newState;
        CurrentState.OnStateEnter();
    }
    
}
