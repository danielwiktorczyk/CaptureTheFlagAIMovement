using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringFace : SteeringAlign
{
    public SteeringOutput GetSteering(Vector3 targetPosition)
    {
        // Calculate the target to delegate to align

        // Get the direction to the target.
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;


        // Check if we're there. If so, do nothing
        if (direction.magnitude <= 0.1f) // could paramaterize this
            return new SteeringOutput(); // TB says "return target..." 

        // Delegate to align
        return base.GetSteering(Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y);
    }
}
