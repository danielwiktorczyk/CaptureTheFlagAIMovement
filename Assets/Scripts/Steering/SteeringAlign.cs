using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAlign : MonoBehaviour
{
    protected AIMovement Character;
    protected Rigidbody Rigidbody;

    public float MaxRotation;
    public float TargetRadius; // For arriving and stopping (Inner)
    public float SlowRadius; // For beginning to slow down (Outer)
    public float TimeToTarget; // Over which to achieve a target speed

    public void Awake()
    {
        Character = GetComponent<AIMovement>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public SteeringOutput GetSteering(float targetOrientation)
    {
        SteeringOutput output = new SteeringOutput();

        // Get the naive rotation direction to the target.
        var rotationDirection = targetOrientation - transform.rotation.eulerAngles.y;
        rotationDirection %= 360f;
        if (rotationDirection > 180f)
            rotationDirection -= 360f;
        else if (rotationDirection < -180f)
            rotationDirection += 360f;

        // If we're aligned (as per the target), return
        if (Mathf.Abs(rotationDirection) < TargetRadius)
            return output;

        float targetRotation;

        // If outside, use maximum rotation speed
        if (Mathf.Abs(rotationDirection) > SlowRadius)
            targetRotation = MaxRotation;
        else // Scaled 
            targetRotation = MaxRotation * (Mathf.Abs(rotationDirection) / SlowRadius);

        // Final target rotation combines the included speed with direction
        targetRotation *= rotationDirection / Mathf.Abs(rotationDirection);

        // Acceleration tries to get to the target rotation.
        output.Angular = (targetRotation - Rigidbody.angularVelocity.y) / TimeToTarget;

        // Check if the acceleration is too great.
        var angularAcceleration = Mathf.Abs(output.Angular);
        //Debug.Log(angularAcceleration);
        if (angularAcceleration > Character.MaxAngularAcceleration)
            output.Angular = output.Angular / angularAcceleration * Character.MaxAngularAcceleration;

        // We don't look at velocity here that's not rotational
        output.Linear = Vector3.zero;

        return output;
    }
}
