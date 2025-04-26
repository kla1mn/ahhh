using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCollision : ShakingStatic
{
    private float externalInfluence;

    private Material material;

    private Rigidbody2D pushingObject;

    [SerializeField] private float speed;
    [SerializeField] private LayerMask pushingLayers;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        CurrentHealth = MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pushingObject = InLayerMask(collision.gameObject.layer) ? collision.GetComponent<Rigidbody2D>() : pushingObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (pushingObject != null)
            pushingObject = pushingObject.gameObject == collision.gameObject ? null : pushingObject;
    }
    private new void Update()
    {
        SetInfluence();
        Hearting();
    }

    private void SetInfluence()
    {
        if (pushingObject != null)
        {
            var influence = pushingObject.linearVelocity.x/3;

            if (externalInfluence < influence)
                externalInfluence += Time.deltaTime * speed;
            else
                externalInfluence -= Time.deltaTime * speed;

            material.SetFloat("_ExternalInfluence", externalInfluence);            
        }
        else
        {
            if (externalInfluence < .1f)
            {
                material.SetFloat("_ExternalInfluence", externalInfluence);
                externalInfluence += Time.deltaTime * speed;
            }
            if (externalInfluence > .1f)
            {
                externalInfluence -= Time.deltaTime * speed;
                material.SetFloat("_ExternalInfluence", externalInfluence);
            }
        }
    }

    private bool InLayerMask(int layer)
    {
        return pushingLayers == (pushingLayers | (1 << layer));
    }

}
