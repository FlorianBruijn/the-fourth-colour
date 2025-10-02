using UnityEngine;

public class EnemyChase : EnemyState
{
    public EnemyChase(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Enemy.EnemyChaseBaseInstance.DoEntryStateLogic();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Enemy.EnemyChaseBaseInstance.DoExitStateLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Enemy.EnemyChaseBaseInstance.DoUpdateStateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        Enemy.EnemyChaseBaseInstance.DoFixedUpdateStateLogic();
    }
}
