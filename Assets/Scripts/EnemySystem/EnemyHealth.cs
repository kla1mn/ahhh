using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable, IRepulsive
{
    [Header("Health Settings")] [SerializeField]
    private float maxHealth = 100f;

    private float currentHealth;
    [SerializeField] private float heartingTimer = 0.5f;
    private bool isHearting;
    private bool isDead;

    [Header("Repulsion Settings")] [SerializeField]
    private float repulsiveDuration = 0.3f;

    [SerializeField] private float repulsiveForce = 5f;
    private bool isRepulsing;
    private int direction;

    [Header("Visual Settings")] [SerializeField]
    private SpriteRenderer[] spriteRenderers;

    private readonly Color damageColor = Color.red;
    private readonly Color originalColor = Color.white;
    [SerializeField] private float flashSpeed = 10f;

    [Header("Target Reference")] [SerializeField]
    private Transform target; // ��������� ������ �� ����


    [SerializeField] private AudioSource damageSource;
    [SerializeField] private AudioSource deadSource;

    private Rigidbody2D rb;
    private Vector2 acceptedRepulsiveVelocity;
    private Vector2 ownRepulsiveVelocity;
    private TMP_Text healthText;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float HeartingTimer => heartingTimer;
    public bool IsHearting => isHearting;

    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }

    public Vector2 AcceptedRepulciveVelocity
    {
        get => acceptedRepulsiveVelocity;
        set => acceptedRepulsiveVelocity = value;
    }

    public Vector2 OwnRepulciveVelocity => ownRepulsiveVelocity;
    public bool IsRepulsing => isRepulsing;
    public float RepulsiveDuration => repulsiveDuration;

    public int Direction
    {
        get => direction;
        set => direction = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        target = GameObject.FindGameObjectWithTag("PlayerRb").transform;

        currentHealth = maxHealth;

        // ������������� ������� ���� ���� �� ������
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        // ������������� ������� ��������� ���� �� ������
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        healthText = GetComponentInChildren<TMP_Text>();
    }

    // ���������� IDamageable.TakeDamage ��� ��������� ���������
    public void TakeDamage(float damageAmount)
    {
        if (IsDead || IsHearting)
            return;

        currentHealth -= damageAmount;

        // ���������� ����������� ����� ������������ ����
        if (target != null)
        {
            Direction = (int)Mathf.Sign(transform.position.x - target.position.x);
        }
        else
        {
            Direction = Random.value > 0.5f ? 1 : -1; // ���� ���� �� �������, ��������� �����������
        }

        StartHearting();
        StartRepulse();

        damageSource.Play();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        IsDead = true;
        rb.gravityScale = 3f;

        foreach (var collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        foreach (var renderer in spriteRenderers)
        {
            if (renderer != null)
            {
                renderer.sortingLayerName = "hud";
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, -2);

        deadSource.Play();

        Destroy(gameObject, 2f);
    }

    public void Hearting()
    {
        if (!isHearting)
            return;

        float lerpValue = Mathf.PingPong(Time.time * flashSpeed, 1);
        Color currentColor = Color.Lerp(originalColor, damageColor, lerpValue);

        foreach (var renderer in spriteRenderers)
        {
            if (renderer != null)
            {
                renderer.color = currentColor;
            }
        }
    }

    public void StartHearting()
    {
        isHearting = true;
        CancelInvoke(nameof(StopHearting));
        Invoke(nameof(StopHearting), heartingTimer);
    }

    public void StopHearting()
    {
        isHearting = false;
        foreach (var renderer in spriteRenderers)
        {
            if (renderer != null)
            {
                renderer.color = originalColor;
            }
        }
    }

    public void StartRepulse()
    {
        isRepulsing = true;
        ownRepulsiveVelocity = new Vector2(Direction * repulsiveForce, repulsiveForce);
        rb.linearVelocity = ownRepulsiveVelocity;
        Invoke(nameof(StopRepulsing), repulsiveDuration);
    }

    public void MakeRepulsion()
    {
        if (!isRepulsing)
            return;
        rb.linearVelocity = ownRepulsiveVelocity;
    }

    public void StopRepulsing()
    {
        isRepulsing = false;
        ownRepulsiveVelocity = Vector2.zero;
    }

    private void Update()
    {
        if (IsHearting)
            Hearting();
        if (IsRepulsing)
            MakeRepulsion();
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"{currentHealth:0}/{maxHealth:0}";
        else
            Debug.Log("Health Text is null");
    }
}