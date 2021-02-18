using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TB edition 3 p62-63
public class SteeringArrive : MonoBehaviour
{
    public float TargetRadius; // Radius for arriving
    public float SlowRadius; // Radius to begin to slow down
    public float TimeToTarget; // over which to achieve target speed

    private Rigidbody Rigidbody;
    private AIMovement Character;

    private void Awake()
    {
        Character = GetComponent<AIMovement>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();

        // Get the direction and distance to the target.
        Vector3 direction = Character.Target.transform.position - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        // Won't be steering rotation here, instead delegate this to Align?
        output.Angular = 0;

        // Again, unlike KinematicArrive, we do not change orientation here

        // Check if we're already there
        if (distance < TargetRadius)
        {
            output.Linear = Vector3.zero;
            return output; // If so, we're done
        }

        // Check if we're outside the slow radius
        if (distance > SlowRadius)
        {
            output.Linear = direction.normalized * Character.MaxSpeed;
        }
        else // Calculate a scaled speed to slow down
        {
            float targetSpeed = Character.MaxSpeed * distance / SlowRadius;
            Vector3 targetVelocity = direction.normalized * targetSpeed;

            // Acceleration tries to get to target velocity
            output.Linear = (targetVelocity - Rigidbody.velocity) / TimeToTarget;

            // Clip if acceleration is too fast
            if (output.Linear.magnitude > Character.MaxAcceleration)
                output.Linear = output.Linear.normalized * Character.MaxAcceleration;
        }

        return output;
    }
}
