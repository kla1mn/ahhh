using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float WalkSpeed = 5f;
    private const float SprintSpeed = 8f;
    private const float CrouchSpeed = 2f;
    private const float WalkStopDuration = .15f;
    private const float SprintStopDuration = .3f;
    private const float StopCoeficient = .8f;

    [Header("AudioSources")]
    [SerializeField, HideInInspector] private AudioSource walkAudio;

    private bool pointIsRight;
    private bool sprintButtonIsPressed;

    private int directionToPoint;

    private float currentSpeed = WalkSpeed;
    private float horizontalInput;

    private Vector2 point;

    private Rigidbody2D rigidBody;

    private InputManager inputManager;

    private PlayerCollisions playerCollisions;
    private PlayerState playerState;

    public bool IsMoving { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsStopping { get; private set; }
    public bool IsMovingToPoint { get; private set; }

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        rigidBody = GameObject.FindGameObjectWithTag("PlayerRb").GetComponent<Rigidbody2D>();
        playerCollisions = GetComponent<PlayerCollisions>();
        playerState = GetComponent<PlayerState>();
        inputManager.OnSprintStarted += (snd, args) => sprintButtonIsPressed = true;
        inputManager.OnSprintCanceled += (snd, args) => sprintButtonIsPressed = false;
    }

    private void Start()
    {
        rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
    }

    private void Update()
    {
        horizontalInput = inputManager.GetPlayerMovementVector().x;
        MoveConditionsCheck();
        SpeedHandler();
        MovingToPoint();
    }

    private void FixedUpdate()
    {
        ProcessMove();
    }

    private void MoveConditionsCheck()
    {
        var hitWall = playerCollisions.IsTouchingWallWithHead || playerCollisions.IsTouchingWallWithBody ||
                      playerCollisions.IsTouchingWallWithLeg;

        IsMoving = ((horizontalInput != 0f || IsMoving) && !hitWall) && playerState.AbleToMove
             && !playerState.IsDashing && !playerState.IsSliding && !playerState.IsHearting;


        IsSprinting = IsMoving && sprintButtonIsPressed && playerState.AbleToSprint && playerCollisions.IsGrounded;
        
        if (!(IsMoving && playerCollisions.IsGrounded) && walkAudio.isPlaying)
            walkAudio.Stop();
        if (IsMoving && playerCollisions.IsGrounded && !walkAudio.isPlaying)
            walkAudio.Play();
    }

    private void ProcessMove()
    {
        if (IsMoving)
        {
            rigidBody.linearVelocity = new Vector2(horizontalInput * currentSpeed, rigidBody.linearVelocity.y);

            CheckToStopMove();
        }
        else if (CanZeroVelocity() || playerState.IsFalling && !IsMoving && !playerState.IsHearting)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x * StopCoeficient, rigidBody.linearVelocity.y);
        }
            
    }

    private void CheckToStopMove()
    {
        if (IsMoving && CanZeroVelocity())
            StopMove(IsSprinting);
    }

    private void SpeedHandler()
    {
        if (IsSprinting)
            currentSpeed = SprintSpeed;
        else if (playerState.IsAttacking || playerState.IsComboWaiting)
            currentSpeed = CrouchSpeed;
        else
            currentSpeed = WalkSpeed;
    }

    public void StopMove(bool wasSprinting)
    {
        var delay = !wasSprinting ? WalkStopDuration : SprintStopDuration;
        IsMoving = false;
        rigidBody.linearVelocity = new Vector2(0, 0);

        Invoke(nameof(ResumeMovingProcess), delay);
    }

    public void ForceStopMove()
    {
        playerState.AbleToMove = false;
        playerState.AbleToFlip = false;

        rigidBody.linearVelocity = new Vector2(0, 0);
    }

    private void ResumeMovingProcess()
    {
        playerState.AbleToMove = !playerState.IsHearting;
        playerState.AbleToFlip = !playerState.IsHearting
                  && !playerState.IsAttacking;
    }

    private bool CanZeroVelocity() => horizontalInput < .1f && horizontalInput > -.1f && !playerState.IsDashing
                  && !playerState.IsSliding && !playerState.IsJumping && !playerState.IsFalling && !playerState.IsHearting;

    public void StartMoveToPoint(Vector2 point, bool isRight)
    {
        IsMovingToPoint = true;

        this.point = point;

        var currentRight = rigidBody.transform.position.x < point.x;

        if (currentRight != playerState.IsFacingRight)
            playerState.ForceFlip();

        directionToPoint = currentRight ? 1 : -1;
        pointIsRight = isRight;


    }

    private void MovingToPoint()
    {
        if (IsMovingToPoint)
        {
            var lenToPoint = Math.Abs(point.x - rigidBody.transform.position.x);

            if (lenToPoint < .25f)
                StopMovingToPoint();
            else
            {
                rigidBody.linearVelocity = new Vector2(directionToPoint * currentSpeed, rigidBody.linearVelocity.y);
            }

        }
    }

    private void StopMovingToPoint()
    {
        rigidBody.linearVelocity = Vector2.zero;
        if (playerState.IsFacingRight != pointIsRight)
            playerState.ForceFlip();
        IsMovingToPoint = false;
    }
}
