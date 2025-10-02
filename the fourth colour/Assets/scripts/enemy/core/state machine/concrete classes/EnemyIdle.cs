using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Enemy.EnemyIdleBaseInstance.DoEntryStateLogic();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Enemy.EnemyIdleBaseInstance.DoExitStateLogic();
        
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Enemy.EnemyIdleBaseInstance.DoUpdateStateLogic();
        
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        Enemy.EnemyIdleBaseInstance.DoFixedUpdateStateLogic();
        
    }
}
