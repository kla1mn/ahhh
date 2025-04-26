using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;


public class EnemyHealthManager : MonoBehaviour, IDamageable, IRepulsive
{
    private readonly Random rnd = new();

    private bool isHearting;
    private bool isDead;
    private bool isRepulsing;
    private bool disableAdditional;

    private float currentHealth;

    private Rigidbody2D rb;

    private EnemyState enemyState;
    private BloodSpawner spawner;

    [SerializeField, HideInInspector] private int deadLayerIndex;

    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float heartingTimer;


    [Header("Repulsion")]
    [SerializeField] private Vector2 ownRepulsiveVelocity;
    [SerializeField, HideInInspector] private float repulsiveDuration = 0.15f;

    [Space]

    [Header("Audio")]
    [SerializeField] private AudioSource[] groanSounds;
    [SerializeField] private AudioSource[] agonySounds;

    [SerializeField] private AudioSource defaultHit;
    [SerializeField] private AudioSource successfullHit;
    [SerializeField] private AudioSource deathHit;




    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float HeartingTimer => heartingTimer;

    public bool IsHearting => isHearting;
    public bool IsDead 
    {
        get  => isDead;
        set => isDead = value;
    }

    public Vector2 AcceptedRepulciveVelocity {  get;  set; }

    public Vector2 OwnRepulciveVelocity 
    {
        get => ownRepulsiveVelocity;
        private set => ownRepulsiveVelocity = value;
    }
    

    public bool IsRepulsing => isRepulsing;
    public float RepulsiveDuration => repulsiveDuration;
    public int Direction { set; get; }
   

    private void Awake()
    {
        currentHealth = maxHealth;

        enemyState = gameObject.GetComponentInParent<EnemyState>();

        spawner = GetComponent<BloodSpawner>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MakeRepulsion();
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead || disableAdditional) return;

        currentHealth -= damageAmount;
        
        if (currentHealth <= 0)
            Die();
        else
            Hearting();

        spawner.SpawnAllBloodPrefabs();
    }

    public void Die()
    {
        isDead = true;
        gameObject.layer = deadLayerIndex;
        StopRepulsing();

        deathHit.Play();
        agonySounds[rnd.Next(agonySounds.Length)].Play();
    }

    public void Hearting()
    {
        disableAdditional = true;
        Invoke(nameof(EnabelAdditional), .2f);

        StartRepulse();

        if (!isHearting)
            Invoke(nameof(StopHearting), heartingTimer);

        isHearting = true;

        if (enemyState.IsAttacking)
            Invoke(nameof(StopHearting), .05f);

        PlayAudio();
    }

    private void PlayAudio()
    {
        (enemyState.IsAttacking ? defaultHit : successfullHit).Play();

        var painId = rnd.Next(groanSounds.Length * 3);

        if (painId < groanSounds.Length && !IsDead && groanSounds.Length != 0)
            groanSounds[painId].Play();
    }

    public void StopHearting() => isHearting = false;

    private void EnabelAdditional () => disableAdditional = false;

    public void StartRepulse()
    {
        if (isRepulsing) return;

        isRepulsing = true;

       Invoke(nameof(StopRepulsing), repulsiveDuration);
    }

    public void MakeRepulsion()
    {
        if (!isRepulsing) return;

        var direction = new Vector2(Direction * (AcceptedRepulciveVelocity.x + ownRepulsiveVelocity.x) / 2, (AcceptedRepulciveVelocity.y + ownRepulsiveVelocity.y) / 2);
        rb.velocity = direction;
    }

    public void StopRepulsing()
    {
        isRepulsing = false;
        if (!enemyState.IsAttacking)
            rb.velocity = Vector2.zero;
    }
}
