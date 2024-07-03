using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [Header("Speed")]
    public float speed;
    public RotateType rotateType;

    private void Update()
    {
        if(rotateType == RotateType.Y)
        {
            transform.Rotate(0, speed * Time.deltaTime, 0);
        }else if(rotateType == RotateType.X)
        {
            transform.Rotate(speed * Time.deltaTime, 0 , 0);
        }else if(rotateType == RotateType.Z)
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }

    }
}

public enum RotateType
{
    X, Y, Z
}
