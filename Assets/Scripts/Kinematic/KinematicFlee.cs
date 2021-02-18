using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KinematicFlee : MonoBehaviour, IKinematicMovement
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

        //// Won't be steering rotation here, instead we orient directly
        //output.Rotation = 0;
        //transform.LookAt(Character.Target.transform, Vector3.up);

        // The velocity is along this OPPOSITE direction, at full speed.
        output.Velocity = -Character.MaxSpeed * direction.normalized;

        //// Face in the direction we want to move. (The TB just straight up changes our direction here)
        //transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        float targetRotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y;
        output.Rotation = AngleMapper.MapDegreesMidpointZero(targetRotation);

        return output;
    }
}
