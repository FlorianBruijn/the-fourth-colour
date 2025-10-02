using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    #region IDamagable

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }

    public void Die()
    {
        GameObject.Destroy(this.gameObject);
    }
    
    public float Health { get; set; }
    [field: SerializeField] public float MaxHealth { get; set; }

    #endregion

    #region StateMachine

    public EnemyStateMachine  StateMachine { get; set; }

    public EnemyIdle Idle { get; set; }
    public EnemyChase Chase { get; set; }
    public EnemyAttack Attack { get; set; }

    #endregion

    #region ScriptableObjects

    [SerializeField] private EnemyIdleSOBase enemyIdleBase;
    [SerializeField] private EnemyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    
    public EnemyIdleSOBase EnemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase EnemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase EnemyAttackBaseInstance { get; set; }

    #endregion
    
    private void Awake()
    {
        EnemyIdleBaseInstance = Instantiate(enemyIdleBase);
        EnemyChaseBaseInstance = Instantiate(enemyChaseBase);
        EnemyAttackBaseInstance = Instantiate(enemyAttackBase);
        
        StateMachine = new EnemyStateMachine();
        
        Idle = new EnemyIdle(this, StateMachine);
        Chase = new EnemyChase(this, StateMachine);
        Attack = new EnemyAttack(this, StateMachine);
    }

    private void Start()
    {
        Health = MaxHealth;
        
        EnemyIdleBaseInstance.Initialize(gameObject, this);
        EnemyChaseBaseInstance.Initialize(gameObject, this);
        EnemyAttackBaseInstance.Initialize(gameObject, this);
        
        StateMachine.Initialize(Idle);
    }

    private void Update()
    {
        StateMachine.CurrentState.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.OnStateFixedUpdate();
    }
}
