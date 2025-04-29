using UnityEngine;

public class EnemyHitZone : HitZone
{

    [Header("Enemy")]
    [SerializeField] private EnemyHealth health;

    [Space]

    [Header("Variables")]
    [SerializeField] private Vector2 repulsionVelocity;

    [SerializeField] private bool isRep;
    [SerializeField] private bool isVamp;

    [SerializeField] private float vampirismFloat;


    protected override void Damage(IDamageable damageableObj)
    {
        base.Damage(damageableObj);

        if (isRep)
        {
            health.TakeDamage( !isVamp || vampirismFloat == 0 ? 0 : -Mathf.Min(vampirismFloat, health.MaxHealth - health.CurrentHealth));
        }


    }

    protected override void Repulse(GameObject collisionObj)
    {
        base.Repulse(collisionObj);

        if (repulsionVelocity == Vector2.zero)
            return;

        var a = collisionObj.GetComponent<IRepulsive>();
        a.AcceptedRepulciveVelocity = repulsionVelocity;
    }
}
