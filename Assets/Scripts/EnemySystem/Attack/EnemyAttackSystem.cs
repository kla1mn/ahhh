using System;
using UnityEngine;

public class EnemyAttackSystem : MonoBehaviour, IEnemyAttackSystem, IJerker
{
    private bool needToCollisionAttack;
    private bool needToAttack;
    private bool isJerking;
    
    private float currentJerkForce;

    private EnemyState enemyState;
    private EnemyCollisions enemyCollisions;
    private Rigidbody2D rb;

    [Header("Attack Variables")]
    [SerializeField] private float damage;
    [SerializeField] private int maxComboAttacks = 5;
    [SerializeField] private float collisionDmg;

    [Header("Repulcive")]
    [SerializeField] private Vector2 repulsiveVelocity;

    [Header("Jerk Variables")]
    [SerializeField] private float littleJerkForce;
    [SerializeField] private float middleJerkForce;
    [SerializeField] private float strongJerkForce;

    [Header("AudioSource")]
    [SerializeField] private AudioSource attackSound;

    public bool IsAttacking { get; private set; }
    public bool IsStrongAttack { get; private set; }
    public bool IsCollisionAttack { get; private set; }
    public bool IsAttackCoolDown { get; private set; }
    public bool IsRotateCoolDown { get; private set; }

    public bool CanCollisionAttack { get; set; } = true;

    public int CurrentAttack { get; private set; }

    public float LittleJerkForce => littleJerkForce;
    public float MiddleJerkForce => middleJerkForce;
    public float StrongJerkForce => strongJerkForce;

    public float DamageValue => damage;

    private void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
        enemyCollisions = GetComponentInParent<EnemyCollisions>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        AttackHandler();

        ResetHandler();
    }

    private void FixedUpdate()
    {
        Jerking();
    }

    public void DoJerk(float force)
    {
        isJerking = true;
        currentJerkForce = force;

        attackSound.Play();

        Invoke(nameof(StopJerk), .25f);
    }

    public void ResetCooldown()
    {
        IsAttackCoolDown = false;

        Invoke(nameof(CanMove), .05f);
    }

    public void FinishAttack()
    {
        if (IsCollisionAttack)
        {
            FinishCollisionAttack();
            return;
        }

        StopJerk();
        IsAttacking = false;
        IsStrongAttack = false;
        CanCollisionAttack = true;

        enemyState.AbleToMove = false;

        IsAttackCoolDown = true;
        IsRotateCoolDown = true;
    }

    //Attack
    private void AttackHandler()
    {
        needToAttack = enemyCollisions.IsTarget && enemyState.AbleToAttack && !enemyState.IsAttacking && !enemyState.IsCollisionAttack &&
            !enemyState.IsAttackCoolDown && !enemyState.IsFalling;
        needToCollisionAttack = enemyCollisions.CollisionTarget && CanCollisionAttack && !enemyState.IsTakingDamage && enemyState.IsAlive &&
            !enemyState.IsStrongAttack && !enemyState.IsFalling;

        if (needToCollisionAttack)
        {
            CollisionAttack();
            return;
        }

        if (needToAttack)
            Attack();

    }

    private void Attack()
    {
        if (!enemyState.AbleToAttack || IsAttacking || enemyState.IsTakingDamage || !enemyState.IsAlive || IsAttackCoolDown) return;

        float attackChance = enemyState.AbleToStrongAttack ? UnityEngine.Random.value : 0f;

        enemyState.AbleToMove = false;

        if (attackChance <= 0.7f)
            NormalAttack();
        else
            StrongAttack();
    }

    private void NormalAttack()
    {
        IsAttacking = true;
        CurrentAttack = (CurrentAttack) % maxComboAttacks + 1;
    }

    private void StrongAttack()
    {
        IsAttacking = true;
        IsStrongAttack = true;
        CurrentAttack = 0;
    }

    //CollisionAttack
    private void CollisionAttack()
    {
        enemyState.AbleToMove = false;

        IsAttacking = true;
        IsCollisionAttack = true;
        CanCollisionAttack = false;
    }

    private void FinishCollisionAttack()
    {
        StopJerk();
        IsAttacking = false;
        IsStrongAttack = false;
        CanCollisionAttack = true;

        enemyState.AbleToMove = false;
        IsCollisionAttack = false;
        CanMove();
    }

    //Reset
    private void ResetHandler()
    {
        if (enemyState.IsAttacking && !enemyState.IsAlive || enemyState.IsCollisionAttack && (!enemyState.IsAlive || enemyState.IsFalling))
            FinishAttack();

        if (enemyState.IsAttackCoolDown && (enemyState.IsTakingDamage || !enemyState.IsAlive || enemyState.IsFalling))
            ResetCooldown();
    }

    private void CanMove()
    {
        IsRotateCoolDown = false;
        enemyState.AbleToMove = true;
    }

    //Jerk
    private void Jerking()
    {
        if (!isJerking)
            return;

        rb.velocity = new Vector2(enemyState.IsRight ? currentJerkForce : -currentJerkForce, rb.velocity.y);
    }

    private void StopJerk()
    {
        isJerking = false;
    }
}
