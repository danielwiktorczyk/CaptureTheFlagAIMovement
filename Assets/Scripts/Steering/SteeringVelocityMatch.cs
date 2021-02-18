using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringVelocityMatch : MonoBehaviour, ISteeringMovement
{
    private AIMovement Character;
    private Rigidbody Rigidbody;

    [SerializeField] private float TimeToTarget; // Over which to achieve a target speed

    private void Awake()
    {
        Character = GetComponent<AIMovement>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();

        // Acceleration tries to get to the target velocity.
        output.Linear = (Character.TargetRigidbody.velocity - Rigidbody.velocity) / TimeToTarget;

        // Check if the acceleration is too fast.
        if (output.Linear.magnitude > Character.MaxAcceleration)
            output.Linear = output.Linear.normalized * Character.MaxAcceleration;

        output.Angular = 0;

        return output;
    }
}
