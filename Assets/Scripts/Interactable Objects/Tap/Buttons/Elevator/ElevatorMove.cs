using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMove : MonoBehaviour
{
    [SerializeField] private ElevatorMovePoint[] movePoints;
    [Space]
    [SerializeField] private float elevatorStopInaccuracy;
    
    public bool IsMoving;

    private ElevatorMovePoint mainPoint;

    private void Awake()
    {
        foreach (var movePoint in movePoints)
            if (movePoint.IsPrimal)
            {
                mainPoint = movePoint;
            }

        if (Vector2.Distance(transform.position, mainPoint.Point.position) > elevatorStopInaccuracy)
        {
            StartMove();
        }
    }

    private void Update()
    {
        Move();
    }



    private void Move()
    {
        if (!IsMoving)
            return;

        transform.position = new Vector2(transform.position.x,transform.position.y + Time.deltaTime * mainPoint.Speed);

        if (Vector2.Distance(transform.position, mainPoint.Point.position) < elevatorStopInaccuracy)
            StopPove();
    }

    private void StopPove()
    {
        transform.position = mainPoint.Point.position;
        mainPoint = movePoints[mainPoint.NextPointId];

        IsMoving = false;
    }

    public void StartMove()
    {
        if (IsMoving) 
            return;

        IsMoving = true;

        if (mainPoint.LevelAge?.IsDead != mainPoint.isDeadLevel)
            mainPoint.LevelAge.TakeDamage(0);
    }

    public void ForceStartMove(int id)
    {
        IsMoving = false;
        mainPoint = movePoints[id];

        StartMove();
    }
}

[System.Serializable]
public class ElevatorMovePoint
{
    public Transform Point;
    public int NextPointId;
    public bool IsPrimal;
    public float Speed;

    public LevelAge LevelAge;
    public bool isDeadLevel;
}
