using UnityEngine;
using UnityEngine.AI;

public class EnemeyStateMachine
{
    public EnemyState CurrentEnemyState { get; set; }

    public void Initialize(EnemyState startingState)
    {
        CurrentEnemyState =  startingState;
        CurrentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState =  newState;
        CurrentEnemyState.EnterState();
    }
}
