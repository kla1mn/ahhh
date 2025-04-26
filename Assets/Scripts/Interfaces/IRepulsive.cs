using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepulsive
{
    public Vector2 AcceptedRepulciveVelocity {  get; set; }
    public Vector2 OwnRepulciveVelocity { get; }
    public bool IsRepulsing { get; }
    
    public float RepulsiveDuration { get; } // ������� �� ������� ������� ������������

    public int Direction { set; get; }
    public void StartRepulse(); //������ ������������
    public void MakeRepulsion(); //������� ������������
    public void StopRepulsing();

}