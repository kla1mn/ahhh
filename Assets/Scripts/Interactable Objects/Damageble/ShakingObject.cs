using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShakingObject : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;

    [SerializeField] private Transform deadPart;

    [SerializeField] protected ParticleSystem damageParticle;

    [SerializeField] protected float shakeDuration = 0f;

    [SerializeField] private float shakeMagnitude = 0.125f;

    [SerializeField] private float dampingSpeed = 0.5f;

    [Space]
    [Header("AudioSource")]
    [SerializeField] protected AudioSource damageAudio;
    [SerializeField] private AudioSource deathAudio;

    private Vector3 initialPosition;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { get; set; }
    public float HeartingDuration { get; set; }
    public float HeartingTimer { get; set; }
    public bool IsHearting { get; set; }
    public bool IsDead { get; set; }

    public bool CanHurt { get; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    protected virtual void Update()
    {
        Hearting();
    }

    public virtual void TakeDamage(float damageAmount)
    {
        if (!IsHearting)
        {
            initialPosition = transform.localPosition;
            damageParticle.Play();
            damageAudio.Play();

            shakeDuration = 0.05f;
            Instantiate(damageParticle, transform.position, Quaternion.identity);

            IsHearting = true;
            CurrentHealth -= damageAmount;
        }
    }

    public void Die()
    {
        deathAudio.transform.parent = null;
        deathAudio.Play();

        gameObject.SetActive(false);

        IsDead = true;

        if (deadPart == null)
            return;

        deadPart.parent = null;
        deadPart.gameObject.SetActive(true);
    }

    public void Hearting()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if (IsHearting)
            StopHearting();
    }

    public void StopHearting()
    {
        StopShake();

        if (CurrentHealth < 0)
            Die();
    }
    protected virtual void StopShake()
    {
        shakeDuration = 0f;
        IsHearting = false;
    }

    protected void ReturnToInitialPosition()=>
        transform.localPosition = initialPosition;
}
