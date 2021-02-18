using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringLookWhereYoureGoing : SteeringAlign
{
    public SteeringOutput GetSteering()
    {
        // Calculate the target to delegate to align
        var velocity = Rigidbody.velocity;

        // Check for a zero direction, and make no change if so.
        if (velocity.magnitude <= 0.01f) // Can paramterize this
            return new SteeringOutput();


        // Set the target based on the velocity, delegate to Align
        return base.GetSteering(Quaternion.LookRotation(velocity, Vector3.up).eulerAngles.y);
    }
}
