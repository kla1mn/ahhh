using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    [SerializeField] private float separationDistance = 1.5f;
    [SerializeField] private float separationStrength = 2f;
    [SerializeField] private LayerMask enemyLayer;
    private EnemyState enemyState;
    private Rigidbody2D rb;

    private void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance, enemyLayer);

        Vector2 separationVector = Vector2.zero;
        int count = 0;

        foreach (Collider2D enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.gameObject == gameObject) continue;

            EnemyState otherEnemyState = enemyCollider.GetComponentInParent<EnemyState>();
            if (otherEnemyState == null) continue;

            // ����� ������ ���� � ����� �����������
            if (enemyState.IsRight != otherEnemyState.IsRight) continue;

            Vector2 direction = (transform.position - enemyCollider.transform.position).normalized;
            separationVector += direction;
            count++;
        }

        if (count > 0)
        {
            separationVector /= count;
            rb.velocity += separationVector * separationStrength * Time.fixedDeltaTime;
        }
    }
}
