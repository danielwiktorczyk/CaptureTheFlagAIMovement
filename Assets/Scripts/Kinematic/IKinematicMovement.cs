using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKinematicMovement
{
    KinematicSteeringOutput GetSteering();
}
