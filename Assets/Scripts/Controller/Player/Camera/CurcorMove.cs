using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurcorMove : MonoBehaviour
{

    [SerializeField] private float dis = 10f;
    [SerializeField] private float speed = 1f;
    void Update()
    {
 
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dis);
        Vector3 mp = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = Vector3.Lerp(transform.position, mp, Time.deltaTime * speed);
    }

}
