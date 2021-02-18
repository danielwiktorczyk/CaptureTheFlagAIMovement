using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringOutput
{
    public Vector3 Linear;
    public float Angular;

    public SteeringOutput()
    {
        Linear = Vector3.zero;
        Angular = 0;
    }
}
