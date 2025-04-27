using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{
    [Header("Enemy References")]
    public GroundEnemy[] groundEnemies;
    public FlyingEnemy[] flyingEnemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerRb"))
        {
            ActivateEnemies();
        }
    }

    private void ActivateEnemies()
    {
        foreach (var enemy in groundEnemies)
        {
            if (enemy != null) enemy.SetActive(true);
        }

        foreach (var enemy in flyingEnemies)
        {
            if (enemy != null) enemy.SetActive(true);
        }
        
        Destroy(gameObject);
    }
}