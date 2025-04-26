using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private const float FlipDuration = .01f;
    private const float WallJumpFlipDuration = .2f;

    [Header("PlayerAvatar")]
    [SerializeField, HideInInspector] private Transform playerAvatar;
    [SerializeField, HideInInspector] private Transform playerSlashParent;

    [Header("Effects")]
    [SerializeField, HideInInspector] private ParticleSystem flipParticle;

    private InputManager inputManager;

    private PlayerState playerState;
    private PlayerCollisions collisions;

    public bool IsFacingRight { get; private set; }
    public bool IsFliping { get; private set; }

    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        IsFacingRight = true;
        playerState = GetComponent<PlayerState>();
        collisions = GetComponent<PlayerCollisions>();
    }
    
    private void Update()
    {
        FlipHandler();
    }
    
    private void FlipHandler()
    {
        var horizontal = inputManager.GetPlayerMovementVector().x;
        if (((IsFacingRight && horizontal < 0f || !IsFacingRight && horizontal > 0f) && !playerState.IsSliding
             && !playerState.IsDashing && playerState.AbleToFlip 
             && !IsFliping || playerState.NeedToFlip && !IsFliping && playerState.AbleToFlip))
        {
            playerState.NeedToFlip = false;
            IsFacingRight = !IsFacingRight;
            IsFliping = true;

            RotateTransform(playerAvatar);

            if (collisions.IsGrounded)  
                flipParticle.Play();
            Invoke(nameof(StopFlip), FlipDuration);
        }

        if (NeedToRotateSlash() && !playerState.IsAttacking)
            RotateTransform(playerSlashParent);
    }

    private bool NeedToRotateSlash() => playerAvatar.localScale.normalized.x != playerSlashParent.localScale.normalized.x;

    private void RotateTransform(Transform transform)
    {
        var localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void StopFlip()=>IsFliping = false;
    public void ForcedFlip()
    {
        IsFacingRight = !IsFacingRight;
        var localScale = playerAvatar.localScale;
        localScale.x *= -1f;
        playerAvatar.localScale = localScale;

        RotateTransform(playerSlashParent);
    }
}
