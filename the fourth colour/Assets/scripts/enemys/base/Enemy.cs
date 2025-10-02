using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IEnemyMovable
{

    public GameObject player;
    public int chaseRange;
    public NavMeshAgent Agent { get; set; }
    public Transform Transform { get; set; }

    #region Health Variables

    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }

    #endregion

    #region Idle Variables

    public float randomMoveRange = 5f;
    public int maxWait;
    public int minWait;
    public Vector3 patrolPoint;

    #endregion

    #region State machine variables

    public EnemeyStateMachine StateMachine {get; set;}
    public EnemyIdleState IdleState {get; set;}
    public EnemyChaseState ChaseState {get; set;}
    public EnemyAttackState AttackState {get; set;}

    #endregion

    private void Awake()
    {
        StateMachine = new EnemeyStateMachine();
        
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        patrolPoint = transform.position;
        Agent = GetComponent<NavMeshAgent>();
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Health

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    
    #endregion

    #region Movement Functions

    public void ChangeTarget(Vector3 target)
    {
        Agent.SetDestination(target);
    }

    public Vector3 GetTarget()
    {
        return Agent.destination;
    }
    
    #endregion

    #region  Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        EnemyDamage,
        PlayFootstepSound
    }

    #endregion
}
