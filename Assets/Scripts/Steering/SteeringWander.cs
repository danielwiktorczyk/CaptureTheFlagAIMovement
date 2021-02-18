using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWander : SteeringFace
{
    // For the wander circle
    public float WanderOffest;
    public float WanderRadius;
    public float WanderRate; // The max rate that the orientation can change
    
    private float WanderOrientation; // The current orientation of the wander target

    public SteeringOutput GetSteering()
    {
        // Calculate the target to delegate to Face

        // Update Wanderer Orientation
        WanderOrientation += RandomBinomial() * WanderRate;

        // Calculate the combined target orientation
        var targetOrientation = WanderOrientation + Character.transform.rotation.eulerAngles.y;

        // Calculate the center of the wander circle
        var centerWanderCircle = Character.transform.position + WanderOffest * Character.transform.forward;
        Debug.DrawLine(transform.position, centerWanderCircle, Color.blue);

        // Calculate the target location
        var targetPosition = centerWanderCircle 
            + WanderRadius * (Quaternion.AngleAxis(targetOrientation, Vector3.up) * Vector3.forward);
        Debug.DrawLine(centerWanderCircle, targetPosition, Color.green);

        // Delegate to face
        var output = base.GetSteering(targetPosition);

        // Set linear acceleration to be at full
        output.Linear = Character.MaxAcceleration * Character.transform.forward;

        return output;
    }

    private static float RandomBinomial() => Random.value - Random.value;
}
