using UnityEngine;

[System.Serializable]
public class Checkpoint
{
    [SerializeField] private Transform point;
    [SerializeField] private float waitTime;

    public Transform Point => point;
    public float WaitTime => waitTime;
}

public interface IEnemyMovement
{
    bool IsMoving { get; }
    bool IsRight {  get; }

    bool IsFalling { get; }
}

public interface IEnemyTargetManager
{
    bool IsFollowingTarget { get;}
    Transform Target { get; }

    void ForceLoseTarget();
    Vector2 GetDistanceToTarget();
}

public interface IEnemyAttackSystem
{
    bool IsAttacking { get; }
    bool IsStrongAttack {  get;}
    bool IsCollisionAttack { get; }
    bool IsAttackCoolDown { get; }
    bool IsRotateCoolDown { get; }

    bool CanCollisionAttack { set; }

    int CurrentAttack {  get; }

    float DamageValue { get; }

    public void FinishAttack();
    public void ResetCooldown();
}

public interface IEnemyBossSystem
{
    bool IsInSpecialPhase { get; set; }
    void ChangePhase(int phaseIndex);
    void ExecuteSpecialAttack();
}


