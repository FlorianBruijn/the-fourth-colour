using UnityEngine;

public class EnemyState
{
    protected Enemy Enemy;
    protected EnemyStateMachine EnemyStateMachine;

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        Enemy = enemy;
        EnemyStateMachine = enemyStateMachine;
    }
    
    virtual public void OnStateEnter() { }
    virtual public void OnStateExit() { }
    virtual public void OnStateUpdate() { }
    virtual public void OnStateFixedUpdate() { }
}
