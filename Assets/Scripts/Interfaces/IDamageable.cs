using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damageAmount);

    void Die();

    void Hearting();

    void StopHearting();

    float MaxHealth { get; }
    float CurrentHealth { get;}
    float HeartingTimer { get; }

    bool IsHearting { get; }
    bool IsDead { get; set; }
}
