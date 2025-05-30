using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpingVelocity = 13f;
    [SerializeField] private float longerJumpingCoeffieicent = .9f;

    [SerializeField] private GameObject jumpParcticle;
    [SerializeField] private Transform jumpParent;

    [SerializeField] private ParticleSystem jumpTrail;

    [SerializeField] private AudioSource jumpSource;

     
    private Rigidbody2D rigidBody;

    private InputManager inputManager;

    private PlayerState playerState;
    private PlayerCollisions playerCollisions;

    private bool jumpButtonIsPressed;
    private bool isAirJumpAvailable;
    
    public bool IsJumping { get; private set; }
    
    private void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>();
        rigidBody = GameObject.FindGameObjectWithTag("PlayerRb").GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        playerCollisions = GetComponent<PlayerCollisions>();


        inputManager.OnJumpStarted += (snd, args) => jumpButtonIsPressed = true;
        inputManager.OnJumpPerformed += Jump;
        inputManager.OnJumpCanceled += (snd, args) => jumpButtonIsPressed = false;
        
        
    }

    private void Update()
    {
        if ((playerCollisions.IsGrounded))
            isAirJumpAvailable = true;
        if (rigidBody.linearVelocity.y <= 0)
            IsJumping = false;
    }


    private void Jump(object sender, EventArgs eventArgs)
    {
        var jump = false;
        if (!playerState.IsDashing)
        {
            if (playerCollisions.IsGrounded && playerState.AbleToJump)
            {
                jump = true;
            }
            else if (isAirJumpAvailable && playerState.AbleToAirJump)
            {
                isAirJumpAvailable = false;
                jump = true;
            }
        }

        if (jump)
        {

            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpingVelocity);
            IsJumping = true;

            Debug.Log(IsJumping);


            jumpTrail.Play();
            Instantiate(jumpParcticle, jumpParent).transform.parent = null;

            jumpSource.Play();
        }
    }
    

}
