using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private const float DashPower = 15f;
    private const float DashDuration = .28f;
    private const float SlidePower = 9f;
    private const float SlideDuration = .4f;
    private const float SlideCullDownDuration = .25f;

    [Header("AudioSources")]
    [SerializeField, HideInInspector] private AudioSource dashAudio;
    [SerializeField, HideInInspector] private AudioSource slideAudio;

    private bool dashReset = true;

    private float slideCullDownTimer;
    private float dashTime;
    private float slideTime;
    private float direction;

    private Rigidbody2D rigidBody;

    private InputManager inputManager;

    private PlayerCollisions playerCollisions;
    private PlayerState playerState;

    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }
    
    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        rigidBody = GameObject.FindGameObjectWithTag("PlayerRb").GetComponent<Rigidbody2D>();
        playerCollisions = GetComponent<PlayerCollisions>();
        playerState = GetComponent<PlayerState>();
        inputManager.OnDashAction += DashConditionsCheck;
    }

    private void FixedUpdate()
    {
        Dash();
        Slide();
    }

    private void Dash()
    {
        if (dashTime > 0)
        {
            dashReset = false;
            IsDashing = true;
            dashTime -= Time.deltaTime;
            rigidBody.linearVelocity = new Vector2(DashPower * direction, 0);
        }
        else
        {
            if (IsDashing)
                rigidBody.gameObject.layer = 11;

            if (playerCollisions.IsGrounded)
                dashReset = true;
            IsDashing = false;
            
        }
    }
    
    private void Slide()
    {
        if (slideTime > 0 )
        {
            IsSliding = true;
            slideTime -= Time.deltaTime;
            rigidBody.linearVelocity = new Vector2(SlidePower * direction, rigidBody.linearVelocity.y);
            if (playerCollisions.IsTouchingWallWithBody || !playerCollisions.IsGrounded)
                slideTime = 0;
            slideCullDownTimer = SlideCullDownDuration;
        }
        else
        {
            if (IsSliding)
                rigidBody.gameObject.layer = 11;
            if (slideCullDownTimer > 0)
                slideCullDownTimer -= Time.deltaTime;
            IsSliding = false;

            playerState.AbleToFlip = true;
        }
    }

    private void DashConditionsCheck(object sender, EventArgs e)
    {
        direction = playerState.IsFacingRight ? 1 : -1;
        if (playerState.IsMoving && playerCollisions.IsGrounded && playerState.AbleToSlide && slideCullDownTimer <= 0)
        {
            slideTime = SlideDuration;
            rigidBody.gameObject.layer = 8;
            slideAudio.Play();

            if (playerState.IsAttacking)
                playerState.HeartingDisable();
        }
        else if (!playerCollisions.IsGrounded && playerState.AbleToDash && slideTime <= 0 && dashReset)
        {
            if (playerState.IsAttacking)
                playerState.HeartingDisable();
            dashTime = DashDuration;
            rigidBody.gameObject.layer = 8;
            dashAudio.Play();
        }
    }

    public void StartDashCoolDown()
    {
        slideCullDownTimer = SlideCullDownDuration;
    }
}
