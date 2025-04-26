using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLevelDistributor : LevelDistributor
{
    [SerializeField] private ElevatorMove elevator;
    [Space]
    [SerializeField] private int enableId;
    [SerializeField] private int disableId;

    public override void MakeNegativeVoid()
    {
        elevator.ForceStartMove(disableId);
    }

    public override void MakePositiveVoid()
    {
        elevator.ForceStartMove(enableId);
    }
}
