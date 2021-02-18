using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringFlee : MonoBehaviour
{
    protected AIMovement Character;

    private void Awake()
    {
        Character = GetComponent<AIMovement>();
    }

    public SteeringOutput GetSteering(Vector3 targetPosition)
    {
        SteeringOutput output = new SteeringOutput();

        // Get the direction to the target.
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;


        // The velocity is along this direction, at full acceleration.
        output.Linear = - Character.MaxAcceleration * direction.normalized;

        // We don't change orientation here, as we did with the Kinematic version. Instead, delegate this to Align! 
        output.Angular = 0;

        return output;
    }
}
