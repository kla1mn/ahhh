using UnityEngine;

public class EnemyAnimatorManager : MonoBehaviour
{
    protected Animator animator;
    protected EnemyState enemyState;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        enemyState = GetComponentInParent<EnemyState>();
    }

    protected virtual void Update()
    {
        UpdAnimations();
    }

    private void UpdAnimations()
    {
        animator.SetBool("IsTakingDamage", enemyState.IsTakingDamage);
        animator.SetBool("IsDead", !enemyState.IsAlive);

        if (enemyState.Type == EnemyType.Maneken)
            return;

        animator.SetBool("IsMoving", enemyState.IsMoving);
        animator.SetBool("IsFalling", enemyState.IsFalling);

        if (enemyState.Type < EnemyType.Attacker)
            return;

        animator.SetBool("IsAttacking", enemyState.IsAttacking);
        animator.SetBool("IsStrongAttack", enemyState.IsStrongAttack);
        animator.SetBool("IsAttackCoolDown", enemyState.IsAttackCoolDown);
        animator.SetBool("IsCollisionAttack", enemyState.IsCollisionAttack);

        animator.SetInteger("CurrentAttack", enemyState.CurrentAttack);
    }

    //AnimationEvents

    public void DisableCollisionAttack() => enemyState.SetCollisionAttackRules(false);

    public void LoweJerk() => enemyState.GetLowJerk();
    public void MiddleJerk() => enemyState.GetMidleerk();
    public void StrongeJerk() => enemyState.GetStrongerk();

    public void EndAttack() => enemyState.StopAttack();
    public void EndAttackCoolDown() => enemyState.StopAttackCoolDown();

}
