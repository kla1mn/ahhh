using System;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyTargetFinder : MonoBehaviour, IEnemyTargetManager
{
    private const float LoseTargetRadius = 28f;

    [SerializeField] private RaycastHit2D target;

    private IDamageable damageableTarget;

    private EnemyState enemyState;
    private Rigidbody2D rb;

    [Header("HeadEyeObject")]
    [SerializeField] private Transform headEyeObject;

    [Header("See Target Variables")]
    [SerializeField] private float forwardVision;
    [SerializeField] private float backwardVision;

    [Header("Target Masks")]
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private LayerMask targetMask;

    public bool IsFollowingTarget { get; private set; }
    public Transform Target => IsFollowingTarget ? target.transform : null;

    private void Awake()
    {
        enemyState = GetComponentInParent<EnemyState>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleEnemyVision();
        HandleLoseTarget();
    }

    public Vector2 GetDistanceToTarget()
    {
        var currentTarget = target.transform != null ? target.transform : rb.transform;
        return currentTarget.position - rb.transform.position;
    }

    public void ForceLoseTarget()
    {
        target = new RaycastHit2D();
        IsFollowingTarget = false;
    }

    private void HandleEnemyVision()
    {
        if (!enemyState.AbleToFollowTarget || !TryGetTarget(out var newTarget))
            return;

        if (!IsFollowingTarget)
        {
            Debug.Log("sdfsfgsfd");
            target = newTarget;
            damageableTarget = target.transform.GetComponent<IDamageable>();

            IsFollowingTarget = true;
        }
    }

    private bool TryGetTarget(out RaycastHit2D newTarget)
    {
        Debug.Log("1");
        var forwardDir = enemyState.IsRight ? Vector2.right : Vector2.left;

        var forwardTarget = Physics2D.Raycast(headEyeObject.position, forwardDir,
            forwardVision, visionMask);
        var forwardUpTarget = Physics2D.Raycast(headEyeObject.position,
            forwardDir + new Vector2(0, 1), forwardVision * 1 / 3, visionMask);
        var forwardDownTarget = Physics2D.Raycast(headEyeObject.position,
            forwardDir + new Vector2(0, -1), forwardVision * 1 / 3, visionMask);
        var backwardTarget = Physics2D.Raycast(headEyeObject.position, -1 * forwardDir,
            backwardVision, visionMask);


        if (forwardTarget && targetMask == (targetMask | (1 << forwardTarget.transform.gameObject.layer)))
        {
            newTarget = forwardTarget;
            Debug.Log("2");
        }
        else if (forwardUpTarget && targetMask == (targetMask | (1 << forwardUpTarget.transform.gameObject.layer)))
        {
            Debug.Log("3");
            newTarget = forwardUpTarget;
        }
        else if (forwardDownTarget && targetMask == (targetMask | (1 << forwardDownTarget.transform.gameObject.layer)))
        {
            Debug.Log("4");
            newTarget = forwardDownTarget;
        }
        else if (backwardTarget && targetMask == (targetMask | (1 << backwardTarget.transform.gameObject.layer)))
        {
            newTarget = backwardTarget;
            Debug.Log("5");
        }
        else
        {
            Debug.Log("6");
            newTarget = new RaycastHit2D();
            return false;
        }

        return true;
    }

    private void HandleLoseTarget()
    {
        if (!IsFollowingTarget)
            return;

        if (Vector2.Distance(target.transform.position, rb.position) > LoseTargetRadius || damageableTarget.IsDead || !enemyState.AbleToFollowTarget || !enemyState.IsAlive)
            ForceLoseTarget();

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        if (enemyState)
        {
            var direction = enemyState.IsRight ? 1 : -1;
            Gizmos.DrawLine(headEyeObject.position,
                new Vector3(headEyeObject.position.x + direction * forwardVision, headEyeObject.position.y, 0));

            Gizmos.DrawLine(headEyeObject.position,
                new Vector3(headEyeObject.position.x + direction * forwardVision * 1 / 3,
                    headEyeObject.position.y + forwardVision * 1 / 3, 0));
            Gizmos.DrawLine(headEyeObject.position,
                new Vector3(headEyeObject.position.x + direction * forwardVision * 1 / 3,
                    headEyeObject.position.y - forwardVision * 1 / 3, 0));

            Gizmos.DrawLine(headEyeObject.position,
                new Vector3(headEyeObject.position.x - direction * backwardVision, headEyeObject.position.y, 0));


            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, LoseTargetRadius);
        }
    }
}
