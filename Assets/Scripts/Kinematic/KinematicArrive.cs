using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TB edition 3 p54
public class KinematicArrive : MonoBehaviour, IKinematicMovement
{
    public float SatisfactionRadius;
    public float InnerStoppingRadius;
    public float TimeToTarget; // Use something higher than MaxSpeed / (SatisfactionRadius)

    private AIMovement Character;

    private void Awake()
    {
        Character = GetComponent<AIMovement>();
    }

    public KinematicSteeringOutput GetSteering()
    {
        KinematicSteeringOutput output = new KinematicSteeringOutput();

        // Get the direction to the target.
        Vector3 direction = Character.Target.transform.position - transform.position;
        direction.y = 0;

        float targetRotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;
        output.Rotation = AngleMapper.MapDegreesMidpointZero(targetRotation);

        // Check if we’re outside radius.
        if (direction.magnitude > SatisfactionRadius)
        {
            output.Velocity = direction.normalized * Character.MaxSpeed;
            return output; // If so, we're done
        }
        else if (direction.magnitude > InnerStoppingRadius)
        {
            // Otherwise, we use t2t.
            float adjustedSpeed = Mathf.Min(Character.MaxSpeed, direction.magnitude / TimeToTarget);
            output.Velocity = direction.normalized * adjustedSpeed;
        }
        else
        {
            output.Velocity = Vector3.zero; // Stop
        }

        return output;
    }
}
