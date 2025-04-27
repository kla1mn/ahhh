using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaveEffects : MonoBehaviour
{
    [Header("Wave Animations")]
    [SerializeField] private Animation[] fistAnimations;

    [SerializeField] private PlayerState state;

    public void StartWaveEffect()
    {
        var currentAnimation = SelectAnimationFromState();
        currentAnimation.Play();
    }

    private Animation SelectAnimationFromState()
    {
       return fistAnimations[state.CurrentAttack - 1];
    }
}
