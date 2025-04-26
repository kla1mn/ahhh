using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class PlayerHealth : MonoBehaviour, IDamageable, IRepulsive
{
    public float MaxHealth { get; set; } = 150f;
    public float CurrentHealth { get; set; } = 150f;
    public float HeartingDuration { get; set; } = .6f;
    public float HeartingTimer { get; set; }    
    public bool IsHearting { get; set; }
    public bool IsDead { get ; set ; }
    public bool CanHurt { get => !state.IsDashing && !IsHearting /*&& !state.IsTakingDamageInBlock*/; }
    public bool IsRepulsing { get; set; }
    public Rigidbody2D Rb { get; set; }
    public float RepulsiveDuration { get; set; } = .2f;
    public int Direction { get; set; }
    public Vector2 AcceptedRepulciveVelocity { get; set; } = new Vector2 (6, 6.5f);

    public Vector2 OwnRepulciveVelocity { get; set; } = new Vector2(6, 6.5f);

    private readonly Random rnd = new();

    private float repulsiveDopDuration = 0f;
    
    private int updatesSkipAmount = 1;
    private int updatesSkiped;
    private float damageAmount;

    private PlayerState state;
    private BloodSpawner spawner;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioSource[] damageSound;
    [SerializeField] private AudioSource[] painSound;
    [SerializeField] private AudioSource criticalSound;
    [SerializeField] private AudioSource deathSound;

    private void Awake()
    {
        state = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
        spawner = GetComponent<BloodSpawner>();
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Hearting();
        MakeRepulsion();
    }
    
    void FixedUpdate()
    {
        DamageHandler();
    }
    
    public void TakeDamage(float damage)
    {
        damageAmount = damage;
        updatesSkiped = 0;
    }

    private void DamageHandler()
    {
        if (damageAmount <= 0)
            return;
        else
        {
            Damage(damageAmount);
            damageAmount = -1f;
        }
    }

    private void Damage(float damage)
    {
        if (CanHurt)
        {
            CurrentHealth -= damage;
            gameObject.layer = 15;
            state.HeartingDisable();
            StartRepulse();

            spawner.SpawnAllBloodPrefabs();
            AudioPlay();

            if (CurrentHealth < 0)
                Die();
            else
            {
                IsHearting = true;
                state.DissableAllActions();
            }

            Debug.LogWarning("[HEART] Pain");
        }
        
    }

    private void AudioPlay()
    {
        var randdamage = rnd.Next(damageSound.Length);

        damageSound[randdamage].Play();

        var painId = rnd.Next(20);

        if (painId < painSound.Length)
            painSound[painId].Play();

        if (CurrentHealth < MaxHealth / 5)
            criticalSound.Play();
    }

    public void Die()
    {
        IsDead = true;
        state.DissableAllActions();

        Rb.sharedMaterial = null;
        var collider = GetComponent<CapsuleCollider2D>();
        collider.sharedMaterial = null;

        Time.timeScale = .8f;

        deathSound.Play();
    }

    public void Hearting()
    {
        if (IsHearting && HeartingTimer < HeartingDuration)
            HeartingTimer += Time.deltaTime;
        else if (IsHearting)
            StopHearting();

    }

    public void StopHearting()
    {
        HeartingTimer = 0;
        IsHearting = false;
        gameObject.layer = 11;

        state.EnableAllActions();
    }

    public void MakeRepulsion()
    {
        if (IsRepulsing)
            Rb.velocity = new Vector2((OwnRepulciveVelocity.x + AcceptedRepulciveVelocity.x) / 2 * Direction, 1 * (OwnRepulciveVelocity.y + AcceptedRepulciveVelocity.y) / 2);
    }

    public void StopRepulsing()
    {
        IsRepulsing = false;
        Rb.velocity = new Vector2(OwnRepulciveVelocity.x * Direction, 0);
        AcceptedRepulciveVelocity = new Vector2(OwnRepulciveVelocity.x, OwnRepulciveVelocity.y);
    }

    public void StartRepulse()
    {
        IsRepulsing = true;
        Invoke(nameof(StopRepulsing), RepulsiveDuration + AcceptedRepulciveVelocity.x / 100);
    }
}
