using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHitZone : HitZone
{
    [SerializeField] private EnemyState enemyState;
    private EnemyTargetFinder targetFinder;
    private Rigidbody2D rb;

    [SerializeField] private Vector2 attackRepulsion;

    private void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
        targetFinder = GetComponentInParent<EnemyTargetFinder>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        var collisionObj = collision.gameObject;
        SetDamage(!enemyState.IsStrongAttack ? enemyState.Damage : enemyState.Damage * 1.6f);
        base.OnTriggerStay2D(collision);
    }

    protected override void Repulse(GameObject collisionObj)
    {
        var repulsiveObj = collisionObj.GetComponent<IRepulsive>();

        if (repulsiveObj != null)
        {
            repulsiveObj.Direction = targetFinder.Target.position.x > rb.position.x ? 1 : -1;
            repulsiveObj.AcceptedRepulciveVelocity = attackRepulsion;
        }
    }

    public void SetDamage(float d) => damage = d;
}