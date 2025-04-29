using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationDelay = 3f; // Полное время на разворот
    public float brakingDistance = 1f; // Дистанция для начала торможения

    [Header("Target Settings")]
    private Transform target;
    public Transform visualPart;

    private Rigidbody2D rb;
    [SerializeField]private bool isActive = false;
    private float currentDirection = 1f;
    private float rotationTimer = 0f;
    private bool isBraking = false;

    private EnemyHealth enemyHealth;


    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
            target = GameObject.FindGameObjectWithTag("PlayerRb").transform;

        if (visualPart == null && transform.childCount > 0)
            visualPart = transform.GetChild(0);

        currentDirection = visualPart.localScale.x;
    }

    private void FixedUpdate()
    {
        if (!isActive || target == null || enemyHealth.IsHearting || enemyHealth.IsDead) return;

        Vector2 direction = (target.position - transform.position).normalized;
        float targetDirection = Mathf.Sign(direction.x);

        // Проверка необходимости разворота
        if (Mathf.Sign(currentDirection) != targetDirection)
        {
            rotationTimer += Time.fixedDeltaTime;
            isBraking = true;

            // Плавное торможение
            float brakeProgress = Mathf.Clamp01(rotationTimer / rotationDelay);
            float speedMultiplier = 1f - brakeProgress;

            rb.linearVelocity = new Vector2(
                currentDirection * moveSpeed * speedMultiplier,
                rb.linearVelocity.y
            );

            // Полная остановка и разворот
            if (rotationTimer >= rotationDelay)
            {
                currentDirection = targetDirection;
                rotationTimer = 0f;
                isBraking = false;

                // Применяем разворот
                Vector3 newScale = visualPart.localScale;
                newScale.x = currentDirection;
                visualPart.localScale = newScale;
            }
        }
        else if (!isBraking)
        {
            // Обычное движение
            rb.linearVelocity = new Vector2(
                currentDirection * moveSpeed,
                rb.linearVelocity.y
            );
        }
    }

    public void SetActive(bool state)
    {
        isActive = state;
        if (!state)
        {
            rb.linearVelocity = Vector2.zero;
            rotationTimer = 0f;
            isBraking = false;
        }
    }

    // Визуализация зоны торможения
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, brakingDistance);
        }
    }
}