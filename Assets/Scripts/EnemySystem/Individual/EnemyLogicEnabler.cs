using System;
using UnityEngine;

public abstract class EnemyLogicEnabler : MonoBehaviour
{
    [SerializeField] private float timeBeforeReborn;

    private bool wasChangedEnemyType;

    protected EnemyState enemyState;


    private void Awake()
    {
        enemyState = GetComponent<EnemyState>();
    }
    void Update()
    {
        if (IfNeedToStartFight())
        {
            StartFight();
        }

        if (!enemyState.IsAlive && !wasChangedEnemyType)
        {
            wasChangedEnemyType = true;
            Invoke(nameof(RebornEnemy), timeBeforeReborn);
        }
    }

    protected virtual void StartFight()
    {
        enemyState.Type = EnemyType.Attacker;
        enemyState.CreateEnemy();
    }

    protected virtual bool IfNeedToStartFight() => true;

    protected virtual void RebornEnemy()
    {
        Debug.LogWarning("StopEnemy");

        enemyState.Type = EnemyType.Checkpoint;
        enemyState.CreateEnemy();
    }


}
