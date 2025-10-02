using UnityEngine;

public class EnemyAttackSOBase : ScriptableObject
{
    protected Enemy Enemy;
    protected Transform Transform;
    protected GameObject GameObject;
    
    protected Transform PlayerTransform;

    public virtual void Initialize(GameObject gameObject, Enemy enemy)
    {
        GameObject = gameObject;
        Transform =  gameObject.transform;
        Enemy = enemy;
        
        PlayerTransform = GameObject.FindWithTag("Player").transform;
    }
    
    virtual public void DoEntryStateLogic() { }
    virtual public void DoExitStateLogic() { }
    virtual public void DoUpdateStateLogic() { }
    virtual public void DoFixedUpdateStateLogic() { }
}
