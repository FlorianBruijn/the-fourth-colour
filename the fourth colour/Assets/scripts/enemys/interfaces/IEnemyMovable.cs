using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMovable
{
    NavMeshAgent Agent { get; set; }
    Transform Transform { get; set; }
    
    void ChangeTarget(Vector3 target);
}
