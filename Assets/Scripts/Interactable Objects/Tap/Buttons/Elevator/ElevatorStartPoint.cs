using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStartPoint : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private ElevatorMove elevatorMove;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionObj = collision.gameObject;
        if (playerMask == (playerMask | (1 << collisionObj.layer)) && !elevatorMove.IsMoving)
            elevatorMove.StartMove();
    }
}
