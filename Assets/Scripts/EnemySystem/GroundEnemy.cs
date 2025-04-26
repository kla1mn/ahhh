using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 3f;
    public Transform visualPart;

    [Header("Detection")]
    public bool isActive = false;

    private Transform target;
    private Rigidbody2D rb;

    [SerializeField] private EnemyHealth EnemyHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("PlayerRb").transform;

        if (visualPart == null && transform.childCount > 0)
            visualPart = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (EnemyHealth.IsHearting || EnemyHealth.IsDead)
            return;

        if (!isActive || target == null) return;

        // Horizontal movement only
        float direction = Mathf.Sign(target.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // Visual flip
        if (visualPart != null)
        {
            Vector3 newScale = visualPart.localScale;
            newScale.x = Mathf.Sign(direction);
            visualPart.localScale = newScale;
        }
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }
}