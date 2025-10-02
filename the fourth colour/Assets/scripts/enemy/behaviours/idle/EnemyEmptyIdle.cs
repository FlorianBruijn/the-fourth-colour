using UnityEngine;

[CreateAssetMenu(fileName = "EnemyEmptyIdle", menuName = "Enemy logic/Idle/Empty")]

public class EnemyEmptyIdle : EnemyIdleSOBase
{
    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void DoEntryStateLogic()
    {
        base.DoEntryStateLogic();
    }

    public override void DoExitStateLogic()
    {
        base.DoExitStateLogic();
    }

    public override void DoUpdateStateLogic()
    {
        base.DoUpdateStateLogic();
    }

    public override void DoFixedUpdateStateLogic()
    {
        base.DoFixedUpdateStateLogic();
    }
}
