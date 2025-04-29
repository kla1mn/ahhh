using System;
using UnityEngine.UI;
using UnityEngine;

public class PlayerAttackSistem : MonoBehaviour, IJerker
{
    [SerializeField] private float ComboWaitDuration = .25f;

    [Header("Audio")]
    [SerializeField] private AudioSource fistAttackAudio;

    [Header("Variables")]
    [SerializeField] private float damage;
    [SerializeField] private Vector2 repulsive;


    private float comboWaitTimer;

    private PlayerState playerState;

    private InputManager inputManager;
    [SerializeField] private float UnlockDuration;

    public bool IsAttacking { get; private set; }
    public bool IsComboWaiting { get; private set; }
    public int CurrentAttack { get; private set; }

    public float CurrentDamage {get => damage; set => damage = value; }

    public Vector2 RepulsioonVelocity => repulsive;

    public AudioSource AttackSound => fistAttackAudio;
    
    public bool NeedToJerk => throw new NotImplementedException();

    public float LittleJerkForce => throw new NotImplementedException();

    public float MiddleJerkForce => throw new NotImplementedException();

    public float StrongJerkForce => throw new NotImplementedException();

    private Rigidbody2D rb;

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();


            inputManager.OnAttackStarted += StartAttack;

        rb = GetComponentInChildren<Rigidbody2D>();
        

        playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        ComboWaiting();
    }

    private void StartAttack(object sender, EventArgs eventArgs)
    {
        if (playerState.CanAttack && CurrentAttack < 3)
        {
            IsAttacking = true;
            CurrentAttack++;

            IsComboWaiting = false;
            comboWaitTimer = 0;

            playerState.AttackDisableActions();

            rb.linearVelocityY = 7f;
        }
    }

    private void StartComboWaiting()
    {
        IsComboWaiting = true;
    }

    private void ComboWaiting()
    {
        if (IsComboWaiting)
        {
            comboWaitTimer += Time.deltaTime;
            if (comboWaitTimer > ComboWaitDuration)
                StopWaitCombo();
        }
    }

    private void StopWaitCombo()
    {
        IsComboWaiting = false;
        comboWaitTimer = 0;
        CurrentAttack = 0;
        Invoke(nameof(EnableActions), UnlockDuration);
        StartCoolDownAfterAttack();
        
    }

    public void EndAttack()
    {
        IsAttacking = false;

        if (CurrentAttack < 3)
            StartComboWaiting();
        else
        {
            CurrentAttack = 0;
            Invoke(nameof(EnableActions), UnlockDuration);
            StartCoolDownAfterAttack();
        }
    }

    private void StartCoolDownAfterAttack()
    {
        playerState.AbleToAttack = false;
    }

    private void EnableActions()
    {
        playerState.EnableAllActions();
    }

    public void AbortAttack()
    {
        IsAttacking = false;
        IsComboWaiting = false;
        CurrentAttack = 0;
        comboWaitTimer = 0;

        //StartCoolDownAfterAttack();
    }

    public void DoJerk(float power)
    {
        throw new NotImplementedException();
    }
}
