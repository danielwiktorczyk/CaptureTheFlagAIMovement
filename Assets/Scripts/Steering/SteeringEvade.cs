using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringEvade : SteeringFlee
{
    private Rigidbody Rigidbody;

    [SerializeField] private float TimeToTarget; // Over which to achieve a target speed
    [SerializeField] private float MaxPrediction; // The maximum prediction time

    private void Awake()
    {
        Character = GetComponent<AIMovement>();
        Rigidbody = GetComponent<Rigidbody>();
    }
    public SteeringOutput GetSteering()
    {
        // Calculate the target to delegate to seek
        Vector3 direction = Character.Target.transform.position - transform.position;
        direction.y = 0;
        var distance = direction.magnitude;

        // Work out current speed
        var speed = Rigidbody.velocity.magnitude;

        var prediction = 0f;
        // Check if speed gives a reasonable prediction time.
        if (speed <= distance / MaxPrediction)
            prediction = MaxPrediction;
        else // Otherwise calculate the prediction time
            prediction = distance / speed;

        // Put the target together
        var predictedTargetPosition = Character.Target.transform.position + Character.TargetRigidbody.velocity * prediction;
        Debug.DrawLine(transform.position, predictedTargetPosition, new Color(128, 0, 128));


        // Delegate to Seek
        return base.GetSteering(predictedTargetPosition);
    }
}
