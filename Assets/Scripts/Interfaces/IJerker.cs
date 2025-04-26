using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJerker
{
    float LittleJerkForce { get; }
    float MiddleJerkForce { get; }
    float StrongJerkForce { get; }

    void DoJerk(float power);
}