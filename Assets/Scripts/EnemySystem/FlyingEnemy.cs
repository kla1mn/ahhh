using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform target;
    public Transform visualPart;
    public float rotationSmoothness = 5f;
    public bool isActive = false;

    private Rigidbody2D rb;

    private EnemyHealth health;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        health = GetComponent<EnemyHealth>();
        target = GameObject.FindGameObjectWithTag("PlayerRb").transform;

        if (visualPart == null && transform.childCount > 0)
            visualPart = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (!isActive || target == null || health.IsHearting || health.IsDead) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        // ������� ������ �� ���������
        if (visualPart != null)
        {
            // ���������� ���� ������ �� ������������ ����������
            float verticalAngle = Mathf.Atan2(direction.y, 1) * Mathf.Rad2Deg;

            // ������� ������� ������ �� ��� Z
            Quaternion targetRotation = Quaternion.AngleAxis(verticalAngle, Vector3.forward);

            // ������� �������
            visualPart.rotation = Quaternion.Lerp(
                visualPart.rotation,
                targetRotation,
                rotationSmoothness * Time.deltaTime
            );

            // ��������� ������� �� X ��� ����������� ��������
            Vector3 newScale = visualPart.localScale;
            newScale.x = Mathf.Sign(direction.x);
            visualPart.localScale = newScale;
        }
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }
}