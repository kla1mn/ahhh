using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingRB : ShakingObject, IRepulsive
{
    private Rigidbody2D rb;

    [SerializeField] private float forceX;
    [SerializeField] private float forceY;

    public bool IsRepulsing { get; set; } = false;

    public float RepulsiveDuration { get; set; } = .05f;
    public int Direction { get; set; }
    public Vector2 AcceptedRepulciveVelocity { get; set; }

    public Vector2 OwnRepulciveVelocity { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        OwnRepulciveVelocity = new Vector2(forceX, forceY);
    }

    protected override void Update()
    {
        base.Update();
        MakeRepulsion();
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        StartRepulse();
    }

    public void MakeRepulsion()
    {
        if (IsRepulsing)
        {
            rb.linearVelocity = new Vector2(Direction * OwnRepulciveVelocity.x, OwnRepulciveVelocity.y);
        }
    }

    public void StartRepulse()
    {
        IsRepulsing = true;
        Invoke("StopRepulsing", RepulsiveDuration);
    }

    public void StopRepulsing()
    {
        Direction = 0;
        IsRepulsing = false;
    }

    protected override void StopShake()
    {
        base.StopShake();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }


}
