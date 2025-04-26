using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;

    private EnemyCollisions collisions;

    private IDamageable healthSystem;
    private IEnemyMovement enemyMovement;
    private IEnemyTargetManager targetManager;
    private IEnemyAttackSystem attackSystem;
    private IJerker attackJerker;
    private IEnemyBossSystem bossSystem;

    public EnemyType Type
    {
        get => enemyType; 
        set => enemyType = value;
    }
    public bool IsAlive => !healthSystem.IsDead;
    public bool IsTakingDamage => healthSystem.IsHearting;
    public bool IsRight => (int)enemyType > 0 && enemyMovement.IsRight;

    public bool IsMoving => (int)enemyType > 0 && enemyMovement.IsMoving;
    public bool IsFalling => (int)enemyType > 0 && enemyMovement.IsFalling;

    public bool CantFlipAfterAttack => enemyType >= EnemyType.Attacker && attackSystem.IsRotateCoolDown;
    public bool IsAttacking => (int)enemyType > 2 && attackSystem.IsAttacking;

    public bool IsAttackCoolDown => (int)enemyType > 2 && attackSystem.IsAttackCoolDown;
    public bool IsStrongAttack => (int)enemyType > 2 && attackSystem.IsStrongAttack;
    public bool IsCollisionAttack => (int)enemyType > 2 && attackSystem.IsCollisionAttack;
    public int CurrentAttack => (int)enemyType > 2 ? attackSystem.CurrentAttack : 0;

    public float Damage => enemyType >= EnemyType.Attacker ? attackSystem.DamageValue : 0;

    public bool AbleToTakeDamage { get => ableToTakeDamage; set => ableToTakeDamage = value; }
    public bool AbleToMove { get => ableToMove; set => ableToMove = value; }
    public bool AbleToFollowTarget { get => ableToFollowTarget; set => ableToFollowTarget = value; }
    public bool AbleToFollowCheckPoint { get => ableToFollowCheckPoint; set => ableToFollowCheckPoint = value; }
    public bool AbleToAttack { get => ableToAttack; set => ableToAttack = value; }
    public bool AbleToStrongAttack { get => ableToStrongAttack; set => ableToStrongAttack = value; }

    private bool ableToTakeDamage;
    private bool ableToMove;
    private bool ableToFollowTarget;
    private bool ableToFollowCheckPoint;
    private bool ableToAttack;
    private bool ableToStrongAttack;

    private void Awake()
    {
        CreateEnemy();
    }

    public void CreateEnemy()
    {
        collisions = GetComponent<EnemyCollisions>();
        healthSystem = GetComponentInChildren<IDamageable>();

        EnableAllActions();

        if (enemyType == EnemyType.Maneken)
            return;

        enemyMovement = GetComponentInChildren<IEnemyMovement>();

        if (enemyType == EnemyType.Checkpoint)
            return;

        targetManager = GetComponentInChildren<IEnemyTargetManager>();

        if (enemyType == EnemyType.FullFollower)
            return;

        attackSystem = GetComponentInChildren<IEnemyAttackSystem>();
        attackJerker = GetComponentInChildren<IJerker>();

        if (enemyType == EnemyType.Attacker)
            return;

        bossSystem = GetComponentInChildren<IEnemyBossSystem>();
    }

    public void EnableAllActions()
    {
        healthSystem.IsDead = false;

        AbleToTakeDamage = true;
        AbleToMove = (int)enemyType > 0;
        AbleToFollowTarget = (int)enemyType > 1;
        AbleToFollowCheckPoint = (int)enemyType > 0;
        AbleToAttack = (int)enemyType > 2;
        AbleToStrongAttack = (int)enemyType > 2;
    }

    public void DisableAllActions() 
    {
        AbleToTakeDamage = false;
        AbleToMove = false;
        AbleToFollowTarget = false;
        AbleToFollowCheckPoint = false;
        AbleToAttack = false;
        AbleToStrongAttack = false;
    }

    public void SetCollisionAttackRules(bool canAttack) => attackSystem.CanCollisionAttack = canAttack;
    public void StopAttack() => attackSystem.FinishAttack();
    public void StopAttackCoolDown() => attackSystem.ResetCooldown();

    public void GetLowJerk() => attackJerker.DoJerk(attackJerker.LittleJerkForce);
    public void GetMidleerk() => attackJerker.DoJerk(attackJerker.MiddleJerkForce);
    public void GetStrongerk() => attackJerker.DoJerk(attackJerker.StrongJerkForce);

}

public enum EnemyType
{
    Maneken,        
    Checkpoint,     
    FullFollower,   
    Attacker,       
    Boss            
}
