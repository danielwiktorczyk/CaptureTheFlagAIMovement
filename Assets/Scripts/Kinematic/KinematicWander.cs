using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicWander : MonoBehaviour, IKinematicMovement
{
    public float MaxRotationSpeed;

    private AIMovement Character;

    private void Awake()
    {
        Character = GetComponent<AIMovement>();
    }

    public KinematicSteeringOutput GetSteering()
    {
        return new KinematicSteeringOutput
        {
            Velocity = Character.MaxSpeed * transform.forward, // Get velocity from the vector form of the orientation.
            Rotation = RandomBinomial() * MaxRotationSpeed // Change our orientation randomly.
        };
    }

    private static float RandomBinomial() => Random.value - Random.value;
}  
