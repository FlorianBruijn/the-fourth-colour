using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemeyStateMachine enemeyStateMachine;

    public EnemyState(Enemy enemy, EnemeyStateMachine enemeyStateMachine)
    {
        this.enemy = enemy;
        this.enemeyStateMachine = enemeyStateMachine;
    }

    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public virtual void FrameUpdate() { }

    public virtual void PhysicsUpdate() { }
    
    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) { }
}
