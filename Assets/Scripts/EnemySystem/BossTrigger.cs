using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss Settings")]
    [SerializeField] private GroundEnemy bossEnemy;
    [SerializeField] private FlyingEnemy bossFlyEnemy;
    [SerializeField] private GameObject flexObjts;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerRb"))
        {
            if (bossEnemy != null) bossEnemy.SetActive(true);
            if (bossFlyEnemy != null) bossFlyEnemy.SetActive(true);

            SwapVolume(true);
        }
    }

    

    private void SwapVolume(bool enableBossMode)
    {
        flexObjts.SetActive(enableBossMode);
    }


}