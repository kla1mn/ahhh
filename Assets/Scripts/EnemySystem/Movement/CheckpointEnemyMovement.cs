using System;
using System.Collections;
using UnityEngine;

public class CheckpointEnemyMovement : MonoBehaviour, IEnemyMovement
{
    private const float CheckpointCompleteDistance = 1.8f;
    private const float GroundingTime = 1.3f;

    protected bool isWaiting = false;
    private bool isGrounded;

    protected Rigidbody2D rb;

    protected EnemyState enemyState;
    protected EnemyCollisions enemyCollisions;

    protected int currentCheckpointIndex = 0;

    [SerializeField, HideInInspector] private Transform animatorTransform;
    [SerializeField, HideInInspector] private Transform checkersTransform;

    [SerializeField] private float movementSpeed;


    [Header("Patrool Points")]
    [SerializeField] protected Checkpoint[] checkpoints;


    // IEnemyMovement Implementation
    public bool IsMoving { get; protected set; }

    public bool IsRight { get; private set; } = true;

    public bool IsFalling {  get; private set; }


    protected virtual void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
        enemyCollisions = GetComponentInParent<EnemyCollisions>();

        animatorTransform = GetComponentInChildren<Animator>().transform;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Patrol();
        Fall();
    }

    //Patrool
    protected void Patrol()
    {
        IsMoving = enemyState.AbleToMove && enemyState.AbleToFollowCheckPoint
            && !isWaiting && checkpoints.Length != 0 && !IsFalling
            && !enemyState.IsTakingDamage && enemyState.IsAlive && !enemyState.IsAttacking && !enemyState.IsAttackCoolDown && !IsFalling;

        if (isWaiting && !enemyState.IsTakingDamage && enemyState.IsAlive && !enemyState.IsAttacking)
            WaitingStopMovement();

        if (!IsMoving)
            return;

        var currentCheckpoint = checkpoints[currentCheckpointIndex = currentCheckpointIndex == -1 ? 0 : currentCheckpointIndex];
        MoveToPoint(currentCheckpoint.Point.position);

        FlipIfNeeded(currentCheckpoint.Point.position.x, rb.position.x);

        if (IsMoving && (Vector2.Distance(rb.position, currentCheckpoint.Point.position) < CheckpointCompleteDistance 
            || !isWaiting && (enemyCollisions.IsWall || enemyCollisions.IsCliff && !IsFalling && !isGrounded)))
            ObstacleStopMovement(enemyCollisions.IsWall || enemyCollisions.IsCliff ? 1f : currentCheckpoint.WaitTime);
    }

    protected void ObstacleStopMovement(float waitTime)
    {
        Debug.LogWarning("ObstclStop");

        WaitingStopMovement();

        currentCheckpointIndex = -1;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (CanFlip(checkpoints[i].Point.position.x, rb.position.x))
            {
                currentCheckpointIndex = i;
                break;
            }
        }

        if (currentCheckpointIndex == -1)
            return;
            
        Invoke(nameof(HandleFlip), waitTime);
        Invoke(nameof(HandleStartMove), waitTime + 0.1f);
    }

    private void MoveToPoint(Vector3 targetPosition)
    {
        Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;
        rb.velocity = direction * movementSpeed;
    }

    private void WaitingStopMovement()
    {
        rb.velocity = Vector2.zero;
        isWaiting = true;
    }

    private void HandleStartMove() => isWaiting = false;
    private void MoveToNextCheckpoint() => currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Length;

    //Flip
    protected void FlipIfNeeded(float posXTarget, float posXRB)
    {
        if (enemyState.IsTakingDamage || enemyState.IsAttacking || !enemyState.IsAlive || enemyState.CantFlipAfterAttack
            || !CanFlip(posXTarget, posXRB))
            return;

        ForceFlip();
    }

    protected bool CanFlip(float posXTarget, float posXRB) => IsRight && posXTarget < posXRB || !IsRight && posXTarget > posXRB;

    protected void ForceFlip()
    {
        animatorTransform.localScale = new Vector2(-animatorTransform.localScale.x, 1);
        checkersTransform.localScale = new Vector2(-checkersTransform.localScale.x, 1);
        IsRight = !IsRight;
    }

    private void HandleFlip() => ForceFlip();

    //Fall
    protected void Fall()
    {
        IsFalling = enemyCollisions.InAir 
            && enemyState.IsAlive && !enemyState.IsAttacking && !enemyState.IsTakingDamage;

        if (IsFalling && enemyState.AbleToMove)
        {
            enemyState.AbleToMove = false;
            enemyState.AbleToAttack = false;
        }

        GroundedHandler();
    }

    private void GroundedHandler()
    {
        if (!IsFalling && !enemyState.AbleToMove && !enemyState.AbleToAttack && !isGrounded 
            && !enemyState.IsTakingDamage && enemyState.IsAlive && !enemyState.IsAttackCoolDown && !enemyState.IsAttacking)
        {
            isGrounded = true;
            Invoke(nameof(StopGrounding), GroundingTime);
        }
    }

    private void StopGrounding()
    {
        Debug.LogError("StopGrounded");

        isGrounded = false;
        enemyState.AbleToMove = true;
        enemyState.AbleToAttack = enemyState.Type >= EnemyType.Attacker;
    }
}