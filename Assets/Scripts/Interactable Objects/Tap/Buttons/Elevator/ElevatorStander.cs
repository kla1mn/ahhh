using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStander : MonoBehaviour
{
    [SerializeField] private LayerMask stagedMask;
    [SerializeField] private LayerMask playerMask;
    [Space]
    [SerializeField] private Transform playerParent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionObj = collision.gameObject;
        if (stagedMask == (stagedMask | (1 << collisionObj.layer)))
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var collisionObj = collision.gameObject;
        if (stagedMask == (stagedMask | (1 << collisionObj.layer)))
        {
            collision.transform.parent = playerMask == (playerMask | (1 << collisionObj.layer)) ? playerParent : null;
        }
    }
}
