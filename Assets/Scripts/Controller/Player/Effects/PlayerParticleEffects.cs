using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    private bool isMoveAnimating;
    private bool isSprintAnimating;

    private Rigidbody2D rb;

    private PlayerState state;
    private PlayerCollisions collisions;

    [Header("Effects")]

    [SerializeField] private ParticleSystem moveParticle;
    [SerializeField] private ParticleSystem sprintParticle;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        state = GetComponent<PlayerState>();
        collisions = GetComponent<PlayerCollisions>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoveParticleStatus();
    }

    private void CheckMoveParticleStatus()
    {
        if (isMoveAnimating)
        {
            if (TryToStopMovingAnimation())
                return;
            SetParticleStatus();
        }
        else
        {
            if (state.IsMoving && collisions.IsGrounded)
            {
                isMoveAnimating = true;
                moveParticle.Play();
            }
        }
    }

    private bool TryToStopMovingAnimation()
    {
        if (!collisions.IsGrounded || rb.linearVelocity.x < .2f && rb.linearVelocity.x > -.2f || !state.IsMoving)
        {
            isMoveAnimating = false;
            moveParticle.Stop();
            sprintParticle.Stop();
            return true;
        }
        return false;
    }
    private void SetParticleStatus()
    {
        if (state.IsSprinting && !isSprintAnimating)
        {
            sprintParticle.Play();
            moveParticle.Stop();

            isSprintAnimating = true;
        }
        if (!state.IsSprinting && isSprintAnimating)
        {
            sprintParticle.Stop();
            moveParticle.Play();

            isSprintAnimating = false;
        }       
    }


}
