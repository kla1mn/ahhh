 using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerCollisions : MonoBehaviour
{
    private const float CircleCheckRadius = .15f;
    private const float RayLength = .9f;
    private const float RayLenghtDown = .32f;
    private const float stunningVelocityY = -228f;
    private const float stunningDuration = 1.2f;

    [SerializeField, HideInInspector] private Transform groundCheck;
    [SerializeField, HideInInspector] private Transform ceilCheck;
    [SerializeField, HideInInspector] private Transform headEyeObject;
    [SerializeField, HideInInspector] private Transform bodyEyeObject;
    [SerializeField, HideInInspector] private Transform legEyeObject;

    [SerializeField, HideInInspector] private AudioSource landingAudio;
    [SerializeField, HideInInspector] private AudioSource fallAudio;

    [SerializeField, HideInInspector] private GameObject dustParticle;
    [SerializeField, HideInInspector] private Transform particleParent;

    [SerializeField, HideInInspector] private LayerMask platformMask;

    [Header("Gizmos")]
    [SerializeField] private bool drawGizmos;

    private bool wasLanding;

    private LayerMask groundMask;
    private LayerMask wallMask;

    private Rigidbody2D rb;

    private PlayerState playerState;

    public bool IsGrounded { get; private set; }
    public bool NeedToCheckWall { get; set; }
    public bool HasCeil { get; private set; }
    public bool IsTouchingWallWithBody { get; private set; }
    public bool IsTouchingWallWithHead { get; private set; }
    public bool IsTouchingWallWithLeg { get; private set; }
    public bool IsAboveGround { get; private set; }
    public bool IsGroundStunning {  get; private set; }

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
        groundMask = LayerMask.GetMask("Ground", "DestrObj", "Platform");
        wallMask = LayerMask.GetMask("Wall", "Ground", "Platform");

        rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void Update()
    {
        CheckCeilCollision();
        CheckGroundCollision();
        CheckWallCollision();

    }

    private void LayingOnGround()
    {
        if (IsGroundStunning && !playerState.IsHearting)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (playerState.IsHearting)
            IsGroundStunning = false;
    }

    private void CheckWallCollision()
    {
        var direction = playerState.IsFacingRight ? new Vector2(1, 0) : new Vector2(-1, 0);
        IsTouchingWallWithHead = Physics2D.Raycast(headEyeObject.position, direction, RayLength, wallMask);
        IsTouchingWallWithBody = Physics2D.Raycast(bodyEyeObject.position, direction, RayLength, wallMask);
        IsTouchingWallWithLeg = Physics2D.Raycast(legEyeObject.position, direction, RayLength, wallMask);
    }

    private void CheckGroundCollision()
    {
        var directionDown = new Vector2(0, -1);

        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, CircleCheckRadius, groundMask);
        IsAboveGround = Physics2D.Raycast(groundCheck.position, directionDown, RayLenghtDown, groundMask);

        ToLand();
    }

    private void ToLand()
    {
        if (IsAboveGround && rb.linearVelocity.y < -2f && !wasLanding)
        {
            if (rb.linearVelocity.y < stunningVelocityY)
                StartGroundStunning();

            landingAudio.Play();
            wasLanding = true;
        }
        if (wasLanding && IsGrounded)
            wasLanding = false;
    }

    private void StartGroundStunning()
    {
        playerState.DissableAllActions();
        rb.linearVelocity = Vector2.zero;
        IsGroundStunning = true;
        fallAudio.Play();

        Invoke(nameof(EndGroundStunning), stunningDuration);
    }

    private void EndGroundStunning()
    {
        IsGroundStunning = false;
        playerState.EnableAllActions();
    }

    private void CheckCeilCollision()
    {
        HasCeil = Physics2D.OverlapCircle(ceilCheck.position, CircleCheckRadius, wallMask);
    }

    public Vector3 GetHeadEyeObjectPosition() => headEyeObject.position;
    public bool OnPlatform() => Physics2D.OverlapCircle(groundCheck.position, CircleCheckRadius, platformMask);

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            var direction = new Vector2(1, 0);
            if (playerState != null)
                direction = playerState.IsFacingRight ? new Vector2(1, 0) : new Vector2(-1, 0);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(headEyeObject.position, headEyeObject.position + new Vector3(direction.x * RayLength, 0, 0));
            Gizmos.DrawLine(bodyEyeObject.position, bodyEyeObject.position + new Vector3(direction.x * RayLength, 0, 0));
            Gizmos.DrawLine(legEyeObject.position, legEyeObject.position + new Vector3(direction.x * RayLength, 0, 0));
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -RayLenghtDown, 0));


            Gizmos.DrawSphere(ceilCheck.position, CircleCheckRadius);
            Gizmos.DrawSphere(groundCheck.position, CircleCheckRadius);
        }
    }
}
