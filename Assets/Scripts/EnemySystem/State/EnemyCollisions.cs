using System;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    private const float CheckDistance = 0.5f;

    private EnemyState enemyState;

    [SerializeField, HideInInspector] private Transform cliffCheck;
    [SerializeField, HideInInspector] private Transform wallLegCheck;
    [SerializeField, HideInInspector] private Transform wallBodyCheck;
    [SerializeField, HideInInspector] private Transform wallHeadCheck;
    [SerializeField, HideInInspector] private Transform groundCheck;

    [SerializeField, HideInInspector] private Transform targetCheck;

    [Header("Attack Variables")]
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float collisionAttackDistance = .4f;

    [Header("Collision Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask targetLayer;

    public bool IsCliff { get; private set; }
    public bool IsWall { get; private set; }
    public bool InAir { get; private set; }
    public bool IsTarget { get; private set; }

    public Transform CollisionTarget { get; private set; }

    private void Awake()
    {
        enemyState = GetComponent<EnemyState>();
    }

    private void Update()
    {
        CheckForCliffs();
        CheckForWalls();
        CheckForAttackTarget();
    }

    private void CheckForAttackTarget()
    {
        if (!enemyState.AbleToFollowTarget)
            return;

        IsTarget = Physics2D.Raycast(targetCheck.position, transform.right, (enemyState.IsRight ? 1 : -1) * attackDistance, targetLayer);
        CollisionTarget = Physics2D.Raycast(targetCheck.position, transform.right, (enemyState.IsRight ? 1 : -1) * collisionAttackDistance, targetLayer).transform;
    }

    private void CheckForCliffs()
    {
        // Проверяем, есть ли поверхность под groundCheck
        IsCliff = !Physics2D.Raycast(cliffCheck.position, Vector2.down, CheckDistance, groundLayer);

        InAir = !Physics2D.Raycast(groundCheck.position, Vector2.down, CheckDistance*2, groundLayer);
    }

    private void CheckForWalls()
    {
        // Проверяем, есть ли препятствие перед wallCheck
        IsWall = Physics2D.Raycast(wallLegCheck.position, transform.right, (enemyState.IsRight ? 1 : -1) * CheckDistance, groundLayer) ||
           Physics2D.Raycast(wallBodyCheck.position, transform.right, (enemyState.IsRight ? 1 : -1) * CheckDistance, groundLayer) ||
           Physics2D.Raycast(wallHeadCheck.position, transform.right, (enemyState.IsRight ? 1 : -1) * CheckDistance, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // Визуализация проверки обрыва
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cliffCheck.position, cliffCheck.position + Vector3.down * CheckDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * CheckDistance * 2);

        // Визуализация проверки стены
        if (enemyState != null)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(wallLegCheck.position, wallLegCheck.position + (enemyState.IsRight ? 1 : -1) * CheckDistance * transform.right);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(targetCheck.position, targetCheck.position + (enemyState.IsRight ? 1 : -1) * attackDistance * transform.right);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetCheck.position, targetCheck.position + (enemyState.IsRight ? 1 : -1) * collisionAttackDistance * transform.right);
        }
        else
        {
            Gizmos.DrawLine(wallLegCheck.position, wallLegCheck.position + 1 * CheckDistance * transform.right);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(targetCheck.position, targetCheck.position + 1 * attackDistance * transform.right);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetCheck.position, targetCheck.position + 1 * collisionAttackDistance * transform.right);
        }
    }
}
