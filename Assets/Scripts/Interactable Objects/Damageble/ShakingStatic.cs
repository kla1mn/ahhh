using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingStatic : ShakingObject
{
    protected override void StopShake()
    {
        base.StopShake();
        ReturnToInitialPosition();
    }
}
