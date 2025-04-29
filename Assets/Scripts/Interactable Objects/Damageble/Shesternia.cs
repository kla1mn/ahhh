using UnityEngine;

public class Shesternia : ShakingRB
{

    public override void TakeDamage(float damageAmount)
    {

        base.TakeDamage(damageAmount);
    }
    public override void MakeRepulsion()
    {
        if (IsRepulsing)
        {
            rb.AddTorque(20f, ForceMode2D.Impulse);

        }
        base.MakeRepulsion();
    }
}
