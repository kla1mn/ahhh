using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour, IDamageable
{
    [SerializeField] private float health;
    public float MaxHealth { get => health; set => health = value; }
    public float CurrentHealth { get; set; }
    public float HeartingDuration { get; set; } = .35f;
    public float HeartingTimer { get; set; }
    public bool IsHearting { get; set; }
    public bool IsDead { get; set; }

    public bool CanHurt => !IsHearting && !IsDead;

    public void Damage(float damageAmount)
    {
        if (CanHurt && CurrentHealth - damageAmount > 0)
        {
            IsHearting = true;
            CurrentHealth -= damageAmount;
        }
        else if (CanHurt)
            Die();
    }

    public void Die()
    {
        IsDead = true;
        CurrentHealth = 0;

        gameObject.layer = LayerMask.NameToLayer("NoPlayer");
    }

    public void Hearting()
    {
         
    }

    public void StopHearting()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damageAmount)
    {
        throw new System.NotImplementedException();
    }
}
