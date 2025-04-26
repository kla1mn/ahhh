using UnityEngine;

public class EnemyBlockAnimation : EnemyAnimatorManager
{
    private EnemyTargetFinder finder;
    private Rigidbody2D rb;

    private bool isSetOnThisPain;

    protected override void Awake()
    {
        base.Awake();

        finder = GetComponentInParent<EnemyTargetFinder>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (finder.Target && !isSetOnThisPain && enemyState.IsTakingDamage)
        {
            animator.SetBool("IsEqualsRight", finder.Target.position.x < rb.position.x && !enemyState.IsRight 
                || finder.Target.position.x > rb.position.x && enemyState.IsRight);

            isSetOnThisPain = true;          
        }

        if (isSetOnThisPain && !enemyState.IsTakingDamage)
            isSetOnThisPain = false;

        animator.SetInteger("Type", (int)enemyState.Type);

        base.Update();
    }
}
