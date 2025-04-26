using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] private LayerMask hertableObjects;


    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        var collisionObj = collision.gameObject;
        if (hertableObjects == (hertableObjects | (1 << collisionObj.layer)))
        {
            Repulse(collisionObj.gameObject);
            Damage(collisionObj.GetComponent<IDamageable>());
        }

    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        var collisionObj = collision.gameObject;
        if (hertableObjects == (hertableObjects | (1 << collisionObj.layer)))
        {
            Repulse(collisionObj.gameObject);
            Damage(collisionObj.GetComponent<IDamageable>());
        }
    }
    protected virtual void Damage(IDamageable damageableObj)
    {
        damageableObj.TakeDamage(damage);
    }
    protected virtual void Repulse(GameObject collisionObj)
    {
        var repulsiveObj = collisionObj.GetComponent<IRepulsive>();

        if (repulsiveObj != null) 
            repulsiveObj.Direction = collisionObj.transform.position.x < transform.position.x ? -1 : 1;
    }

}


