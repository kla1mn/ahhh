using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum ControllerType
    {
        OnlyMove,
        WithoutAttack,
        FullController
    }


    [SerializeField] private ControllerType controllerType;

    public bool isMenu;

    private PlayerFlip playerFlip;
    private PlayerJump playerJump;
    private PlayerMove playerMove;
    private PlayerDash playerDash;
    private PlayerCollisions playerCollisions;
    private PlayerAttackSistem attackSistem;
    private PlayerHealth playerHealth;
    
    public bool IsFacingRight => playerFlip.IsFacingRight;
    public bool IsFliping => playerFlip.IsFliping;
    public bool IsMoving => playerMove.IsMoving || playerMove.IsMovingToPoint;
    public bool IsMoveToPoint => playerMove.IsMovingToPoint;
    public bool IsSprinting => playerMove.IsSprinting;
    public bool IsJumping => controllerType != ControllerType.OnlyMove && playerJump.IsJumping;
    public bool IsFalling => !playerCollisions.IsGrounded && !IsJumping && !IsDashing;
    public bool IsAboveGround => playerCollisions.IsAboveGround && !playerCollisions.IsGrounded;
    public bool IsDashing => controllerType != ControllerType.OnlyMove && playerDash.IsDashing;
    public bool IsSliding => controllerType != ControllerType.OnlyMove && playerDash.IsSliding;
    public bool IsGroundStunning => playerCollisions.IsGroundStunning;
    public bool IsHearting => playerHealth.IsHearting;
    public bool IsDead => playerHealth.IsDead;
    public bool IsAttacking => controllerType == ControllerType.FullController && attackSistem.IsAttacking;
    public bool IsComboWaiting => controllerType == ControllerType.FullController && attackSistem.IsComboWaiting;
    public int CurrentAttack  => controllerType != ControllerType.FullController ? 0 : attackSistem.CurrentAttack; 

    public bool CanAttack =>  !IsAttacking &&
        !IsSliding && !IsDashing && !IsHearting && AbleToAttack && Time.timeScale != 0;


    public bool NeedToFlip { get; set; }
     
    public bool AbleToFlip { get; set; }
    public bool AbleToMove { get; set; }
    public bool AbleToSprint { get; set; }
    public bool AbleToWallJump { get; set; }
    public bool AbleToJump { get; set; }
    public bool AbleToDash { get; set; }
    public bool AbleToSlide { get; set; }
    public bool AbleToAirJump { get; set; }
    public bool AbleToHangOnEdge { get; set; }
    public bool AbleToWallSlide { get; set; }
    public bool AbleToAttack { get; set; }
    public bool AbleToPause { get; set; } = true;
    public bool UnlockDash { get; set; } = false;
    public bool UnlockWallJump { get; set; } = false;
    public bool UnlockSlide { get; set; } = true;
    public bool UnlockAirJump { get; set; }
    public bool UnlockWallSlide { get; set; } = false; 
    
    public bool DamageInAir {  get; set; }


    public bool isFirstRespawn;

    private void Awake()
    {
        if (!isMenu)
        {
            CreatePlayer();
        }
    }


    private void CreatePlayer()
    {
        EnableAllActions();

        playerMove = GetComponent<PlayerMove>();
        playerFlip = GetComponent<PlayerFlip>();
        playerHealth = GameObject.FindWithTag("PlayerRb").GetComponent<PlayerHealth>();
        playerCollisions = GetComponent<PlayerCollisions>();

        if (controllerType == ControllerType.OnlyMove)
            return;


        playerJump = GetComponent<PlayerJump>();
        playerDash = GetComponent<PlayerDash>();

        if (controllerType == ControllerType.WithoutAttack)
            return;

        attackSistem = GetComponent<PlayerAttackSistem>();
    }

    public void RecreatePlayer(ControllerType playerType)
    {
        controllerType = playerType;

        DissableAllActions();

        playerMove = null;
        playerFlip = null;
        playerHealth = null;
        playerCollisions = null;
        playerJump = null;
        attackSistem = null;

        CreatePlayer();
    }

    public void EnableAllActions()
    {
        AbleToFlip = true;
        AbleToMove = true;
        AbleToSprint = controllerType != ControllerType.OnlyMove;
        AbleToJump = controllerType != ControllerType.OnlyMove;
        AbleToDash = UnlockDash && controllerType != ControllerType.OnlyMove;
        AbleToSlide = UnlockSlide && controllerType != ControllerType.OnlyMove;
        AbleToAirJump = UnlockAirJump && controllerType != ControllerType.OnlyMove;
        AbleToWallJump = UnlockWallJump && controllerType != ControllerType.OnlyMove;
        AbleToHangOnEdge = controllerType != ControllerType.OnlyMove;
        AbleToWallSlide = UnlockWallSlide && controllerType != ControllerType.OnlyMove;
        AbleToAttack = controllerType == ControllerType.FullController;
    }
    
    public void DissableAllActions()
    {
        //playerMove.ForceStopMove();
        AbleToFlip = false;
        AbleToMove = false;
        AbleToSprint = false;
        AbleToJump = false;
        AbleToDash = false;
        AbleToSlide = false;
        AbleToAirJump = false;
        AbleToWallJump = false;
        AbleToHangOnEdge = false;
        AbleToWallSlide = false;
        AbleToAttack = false;
    }

    public void AttackDisableActions()
    {
        AbleToFlip = false;
        AbleToSprint = false;
        AbleToJump = false;
        AbleToAirJump = false;
        AbleToWallJump = false;
        AbleToHangOnEdge = false;
        AbleToWallSlide = false;
    }
    

    public void FirstRespawn()
    {
        DissableAllActions();
        isFirstRespawn = true;
    }

    public void StopAttack()
    {
        attackSistem.EndAttack();
    }

    public void PlayAttackAudio()
    {
        attackSistem.AttackSound.Play();
    }

    public void HeartingDisable()
    {
        attackSistem.AbortAttack();
        DamageInAir = IsFalling || IsJumping || IsDashing;
    }

    public float SetHealth(float value)
    {
        if (playerHealth == null)
            playerHealth = GameObject.FindWithTag("PlayerRb").GetComponent<PlayerHealth>();

        return playerHealth.CurrentHealth = value;
    }


    //Force Func

    public void ForceFlip() => playerFlip.ForcedFlip();

    public void MoveToPoint(Vector2 point, bool isRight) 
        => playerMove.StartMoveToPoint(point, isRight);
}
