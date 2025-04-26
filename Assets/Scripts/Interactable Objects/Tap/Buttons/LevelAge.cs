using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAge : ShakingStatic
{
    [SerializeField] private LevelDistributor distributor;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        IsDead = true;
    }
    public override void TakeDamage(float damageAmount)
    {
        if (IsHearting)
            return;

        animator.SetBool("IsDead", IsDead = !IsDead);
        IsHearting = true;

        shakeDuration = 0.05f;

        damageParticle.Play();
        damageAudio.Play();

        shakeDuration = 0.05f;
        Instantiate(damageParticle, transform.position, Quaternion.identity);

        if (IsDead)
            distributor.MakeNegativeVoid();
        else
            distributor.MakePositiveVoid();

    }

    public void Check()
    {
        if (IsHearting)
            return;

        animator.SetBool("IsDead", IsDead = !IsDead);
        IsHearting = true;
    }
}
