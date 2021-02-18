using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSteeringOutput
{
    public Vector3 Velocity;
    public float Rotation;

    public KinematicSteeringOutput()
    {
        Velocity = Vector3.zero;
        Rotation = 0;
    }
}
