using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundVisibility: MonoBehaviour
{
    private float minColorDistance = 4.5f;
    private float transparencyCoef = .2f;
    private float minTransparency = .1f;
    private float transparency;

    private Transform playerTransform;

    private SpriteRenderer render;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        render = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdColor();
    }

    private void UpdColor()
    {
        var distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance < minColorDistance)
            transparency = minTransparency + transparencyCoef * distance;
        else
            transparency = 1f;

        render.color = new Color(render.color.r, render.color.g, render.color.b, transparency);
    }
}
