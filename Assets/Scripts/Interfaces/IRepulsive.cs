using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepulsive
{
    public Vector2 AcceptedRepulciveVelocity {  get; set; }
    public Vector2 OwnRepulciveVelocity { get; }
    public bool IsRepulsing { get; }
    
    public float RepulsiveDuration { get; } // сколько по времени длиться отбрасывание

    public int Direction { set; get; }
    public void StartRepulse(); //начало отбрасывания
    public void MakeRepulsion(); //процесс отбрасывания
    public void StopRepulsing();

}