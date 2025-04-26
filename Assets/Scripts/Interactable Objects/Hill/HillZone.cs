using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillZone : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject hint;

    private bool onZone;

    [SerializeField] private UPReffects effects;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private InputManager inputManager;

    [SerializeField] private AudioSource hillAudio;

    private void Awake()
    {
        inputManager.OnUsePerfomed += TryToHill;
    }

    private void TryToHill(object sender, EventArgs e)
    {
        if (!onZone)
            return;

        effects.StartDepthOfFieldAnimation(true);
        hillAudio.Play();

        playerHealth.CurrentHealth = playerHealth.MaxHealth;

        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionObj = collision.gameObject;
        if (playerLayer == (playerLayer | (1 << collisionObj.layer)))
        {
            onZone = true;
            hint.gameObject.SetActive(true);
        }

    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        var collisionObj = collision.gameObject;
        if (playerLayer == (playerLayer | (1 << collisionObj.layer)))
        {
            onZone = false;
            hint.gameObject.SetActive(false);
        }
    }


}
