using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage);

    void Die();
    
    float Health { get; set; }
    float MaxHealth { get; set; }
}
