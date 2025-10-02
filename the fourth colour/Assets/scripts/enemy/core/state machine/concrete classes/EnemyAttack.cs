using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Enemy.EnemyAttackBaseInstance.DoEntryStateLogic();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Enemy.EnemyAttackBaseInstance.DoExitStateLogic();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        Enemy.EnemyAttackBaseInstance.DoUpdateStateLogic();
    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();
        Enemy.EnemyAttackBaseInstance.DoFixedUpdateStateLogic();
    }
}
