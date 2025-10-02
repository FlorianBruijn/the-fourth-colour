using System.Collections;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 target;
    public EnemyIdleState(Enemy enemy, EnemeyStateMachine enemeyStateMachine) : base(enemy, enemeyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        SetNewPosition();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Vector3 transformPosition = enemy.transform.position;
        Vector3 playerPosition = enemy.player.transform.position;
        float distance = Vector3.Distance(transformPosition, playerPosition);
        
        if ((transformPosition - target).magnitude < 1.1f)
        {
            SetNewPosition();
        }
        
        if (!(distance < enemy.chaseRange)) return;
        if (Physics.Raycast(transformPosition, playerPosition, distance)) return;
        enemy.StateMachine.ChangeState(enemy.ChaseState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    private Vector3 GetRandomPointInCircle()
    {
        var toReturn = enemy.patrolPoint + (Vector3)UnityEngine.Random.insideUnitCircle * enemy.randomMoveRange;
        toReturn.z = toReturn.y;
        toReturn.y = 0;
        return toReturn;
    }

    private void SetNewPosition()
    {
        target = GetRandomPointInCircle();
        enemy.ChangeTarget(target);
        target = enemy.GetTarget();
    }
}
