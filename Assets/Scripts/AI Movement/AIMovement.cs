using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AIMovement
{
    public GameObject Target { get; }
    public float MaxSpeed { get; }
    public float MaxAcceleration { get; }
    public float MaxAngularSpeed { get; }
    public float MaxAngularAcceleration { get; }
    public Rigidbody TargetRigidbody { get; }
    public Transform transform { get; }
}
