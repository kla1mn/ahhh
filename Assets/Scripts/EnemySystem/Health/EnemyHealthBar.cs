using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] protected GameObject healthBar;
    

    private IDamageable health;

    private float barWidth;

    private void Awake()
    {
        health = GetComponent<IDamageable>();
        barWidth = .9f * rect.sizeDelta.x;
    }

    protected virtual void Update()
    {
        rect.localPosition = new Vector3(GetBarPosition(), 0, 0);

        DeathChecker();
    }

    private void DeathChecker()
    {
        if (health.CurrentHealth <= 0)
        {
            Destroy(healthBar);
            Destroy(this);
        }
    }

    private float GetBarPosition()
    {
        return (health.CurrentHealth - health.MaxHealth) * barWidth / health.MaxHealth;
    }
}
