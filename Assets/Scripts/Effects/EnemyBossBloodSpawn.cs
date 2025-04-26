using UnityEngine;

public class EnemyBossBloodSpawn : BloodSpawner
{
    private EnemyState enemyState;

    [SerializeField] private GameObject anotherBloodParticle;

    private void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
    }

    public override void SpawnAllBloodPrefabs()
    {
        base.SpawnAllBloodPrefabs();

        if (!SpawnCondition())
        {
            var a = Instantiate(anotherBloodParticle, bloodSpawnTransformWall).transform;
            a.parent = null;
            a.localScale = new Vector3(1, 1, 1);
        }
    }

    protected override bool SpawnCondition()
    {
        return !enemyState.IsAlive;
    }
}
