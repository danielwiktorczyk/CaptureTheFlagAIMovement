using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KinematicSeek : MonoBehaviour, IKinematicMovement
{
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

        // The velocity is along this direction, at full speed.
        output.Velocity = Character.MaxSpeed * direction.normalized;

        //// Face in the direction we want to move. (The TB just straight up changes our direction here)
        //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        //output.Rotation = 0;
        float targetRotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;
        output.Rotation = AngleMapper.MapDegreesMidpointZero(targetRotation);

        return output;
    }
}
