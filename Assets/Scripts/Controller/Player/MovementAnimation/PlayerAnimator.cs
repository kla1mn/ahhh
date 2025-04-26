using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CineMashineEffects;

namespace PlayerController
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerState state;
        [SerializeField] private CineMashineEffects cameraEffects;

        [SerializeField] private BoxCollider2D dashCollider;
        [SerializeField] private CircleCollider2D crouchCollider;
        [SerializeField] private CapsuleCollider2D defaultCollider;

        [SerializeField] private SpriteRenderer[] playerToFgLayerRends;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            SetCollider();
            SetCameraFOV();

            SetAnimatorBooleans();
        }

        private void SetCollider()
        {

        }

        private void SetCameraFOV()
        {

        }

        private void SetAnimatorBooleans()
        {
            animator.SetBool("IsMoving", state.IsMoving);

            animator.SetBool("IsSprinting", state.IsSprinting);

            animator.SetBool("IsJumping", state.IsJumping);
            animator.SetBool("IsFalling", state.IsFalling);
            animator.SetBool("IsAboveGround", state.IsAboveGround);
            animator.SetBool("IsGroundStunning", state.IsGroundStunning);


            animator.SetBool("IsSliding", state.IsSliding);
            animator.SetBool("IsDashing", state.IsDashing);


            animator.SetBool("IsAttacking", state.IsAttacking);
            animator.SetInteger("CurrentAttack", state.CurrentAttack);
            animator.SetBool("IsComboWaiting", state.IsComboWaiting);


            animator.SetBool("IsHearting", state.IsHearting);
            animator.SetBool("IsDead", state.IsDead);
            animator.SetBool("DamageInAir", state.DamageInAir);

        }

        public void EndAttack() => state.StopAttack();

        public void LockJump() => state.AbleToWallJump = false;
        public void UnlockJump() => state.AbleToWallJump = true;
        public void AttackAudioPlay() => state.PlayAttackAudio();

        public void PlayerToForeGroundLayerEffect()
        {
            foreach (var rend in playerToFgLayerRends)
                rend.sortingLayerName = "playerClimb";

            Invoke("ForeGroundToPlayerLayerEffecct", .5f);
        }

        private void ForeGroundToPlayerLayerEffecct()
        {
            foreach (var rend in playerToFgLayerRends)
                rend.sortingLayerName = "player";
        }
    }
}
