using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class BloodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] bloodPrefabsPlatform;
    [SerializeField] private GameObject[] bloodPrefabsWall;

    [SerializeField] private GameObject bloodParticle;

    private readonly Random rnd = new();

    [SerializeField] private Transform bloodSpawnTransformPlatform;
    [SerializeField] protected Transform bloodSpawnTransformWall;

    public virtual void SpawnAllBloodPrefabs()
    {
        if (SpawnCondition())
        {
            SpawnBloodPrefab(bloodPrefabsPlatform, bloodSpawnTransformPlatform);
            SpawnBloodPrefab(bloodPrefabsWall, bloodSpawnTransformWall);

            var a = Instantiate(bloodParticle, bloodSpawnTransformWall).transform;
            a.parent = null;
            a.localScale = new Vector3(1, 1, 1);
        }     
    }

    private void SpawnBloodPrefab(GameObject[] bloodPrefab, Transform parent)
    {
        var id = rnd.Next(0, bloodPrefab.Length);
        var obj = Instantiate(bloodPrefab[id], parent);
        obj.transform.parent = null;
    }

    protected virtual bool SpawnCondition() => true;
}
