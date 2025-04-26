using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitZone : HitZone
{
    [SerializeField] private bool isStrong;

    [SerializeField] private PlayerAttackSistem playerAttackSistem;
    [SerializeField] private PlayerState state;
    [SerializeField] private CineMashineEffects effects;

    protected override void Damage(IDamageable damageableObj)
    {
        damage = playerAttackSistem.CurrentDamage * (isStrong ? 1.5f : 1f);
        effects.StartAttackShake();

        base.Damage(damageableObj);
    }

    protected override void Repulse(GameObject collisionObj)
    {
        var repulsiveObj = collisionObj.GetComponent<IRepulsive>();
        if (repulsiveObj != null)
        {
            repulsiveObj.Direction = state.IsFacingRight ? 1 : -1;
            repulsiveObj.AcceptedRepulciveVelocity = playerAttackSistem.RepulsioonVelocity;
        }
    }
}
