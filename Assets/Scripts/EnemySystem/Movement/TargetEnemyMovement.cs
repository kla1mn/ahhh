using UnityEngine;
using System;

public class TargetEnemyMovement : CheckpointEnemyMovement
{
    private const float WaitIfObstacleTime = 8f;
    private IEnemyTargetManager targetFinder;

    [Space]

    [Header("Chaise Variables")]
    [SerializeField] private float chaiseSpeed;


    protected override void Awake()
    {
        base.Awake();

        targetFinder = GetComponent<EnemyTargetFinder>();
    }

    private void Update()
    {
        Fall();

        if (targetFinder.IsFollowingTarget && enemyState.AbleToFollowTarget)
        {
            isWaiting = isWaiting && !targetFinder.IsFollowingTarget;

            ChaiseTarget();

            return;
        }

        Patrol();
    }

    public void ChaiseTarget()
    {
        IsMoving = enemyState.AbleToMove && enemyState.AbleToFollowTarget && !isWaiting
            && enemyState.IsAlive && !enemyState.IsTakingDamage && !enemyState.IsAttacking && !IsFalling;

        if (!IsMoving)
            return;

        if (enemyCollisions.IsWall || enemyCollisions.IsCliff)
        {
            if (!CanFlip(targetFinder.Target.position.x, rb.position.x))
            {
                Debug.LogWarning("Wait To Lose Target");

                targetFinder.ForceLoseTarget();
                ObstacleStopMovement(WaitIfObstacleTime);

                return;
            }

            ForceFlip();
        }

        FlipIfNeeded(targetFinder.Target.position.x, rb.position.x);
        ProcessMove();
    }

    private void ProcessMove()
    {
        var horizontalInput = targetFinder.GetDistanceToTarget().x;
        var input = !IsMoving ? 0 : Math.Sign(horizontalInput) * chaiseSpeed;
        rb.linearVelocity = new Vector2(input, rb.linearVelocity.y);
    }
}