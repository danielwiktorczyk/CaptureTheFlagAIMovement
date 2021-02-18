using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum SteeringBehaviourSelection
{
    Idle,
    SteeringWander,
    SteeringSeek,
    SteeringFlee,
    SteeringArrive,
    SteeringPersue,
    SteeringEvade
}

public enum OrientationBehaviourSelection
{
    None,
    SteeringAlign, 
    SteeringFace,
    SteeringLookWhereYoureGoing
}

public class SteeringCharacter : MonoBehaviour, AIMovement
{
    public SteeringBehaviourSelection SteeringBehaviourSelection;
    public OrientationBehaviourSelection OrientationBehaviourSelection;

    private SteeringOutput SteeringOutput;

    private Rigidbody Rigidbody;

    public float MaxSpeed;
    public float MaxAcceleration;
    public float MaxAngularSpeed;
    public float MaxAngularAcceleration;
    public GameObject Target;
    internal Rigidbody TargetRigidbody;

    [SerializeField] private SteeringWander SteeringWander;
    [SerializeField] private SteeringSeek SteeringSeek;
    [SerializeField] private SteeringFlee SteeringFlee;
    [SerializeField] private SteeringArrive SteeringArrive;
    [SerializeField] private SteeringAlign SteeringAlign;
    [SerializeField] private SteeringPersue SteeringPersue;
    [SerializeField] private SteeringFace SteeringFace;
    [SerializeField] private SteeringLookWhereYoureGoing SteeringLookWhereYoureGoing;
    [SerializeField] private SteeringEvade SteeringEvade;

    GameObject AIMovement.Target { get => Target; }
    float AIMovement.MaxSpeed { get => MaxSpeed; }
    float AIMovement.MaxAcceleration { get => MaxAcceleration; }
    float AIMovement.MaxAngularSpeed { get => MaxAngularSpeed; }
    float AIMovement.MaxAngularAcceleration { get => MaxAngularAcceleration; }
    Rigidbody AIMovement.TargetRigidbody => TargetRigidbody;

    Transform AIMovement.transform => transform;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        TargetRigidbody = Target.GetComponent<Rigidbody>();
    }

    private void Move()
    {
        switch (SteeringBehaviourSelection)
        {
            case SteeringBehaviourSelection.SteeringWander:
                SteeringOutput = SteeringWander.GetSteering();
                break;
            case SteeringBehaviourSelection.SteeringSeek:
                SteeringOutput = SteeringSeek.GetSteering(Target.transform.position);
                break;
            case SteeringBehaviourSelection.SteeringFlee:
                SteeringOutput = SteeringFlee.GetSteering(Target.transform.position);
                break;
            case SteeringBehaviourSelection.SteeringArrive:
                SteeringOutput = SteeringArrive.GetSteering();
                break;
            case SteeringBehaviourSelection.SteeringPersue:
                SteeringOutput = SteeringPersue.GetSteering();
                break;
            case SteeringBehaviourSelection.SteeringEvade:
                SteeringOutput = SteeringEvade.GetSteering();
                break;
            default:
                break;
        }

        switch (OrientationBehaviourSelection)
        {
            case OrientationBehaviourSelection.None:
                break;
            case OrientationBehaviourSelection.SteeringAlign:
                SteeringOutput.Angular = SteeringAlign.GetSteering(Target.transform.rotation.eulerAngles.y).Angular;
                break;
            case OrientationBehaviourSelection.SteeringFace:
                if (SteeringBehaviourSelection != SteeringBehaviourSelection.SteeringWander)
                    SteeringOutput.Angular = SteeringFace.GetSteering(Target.transform.position).Angular; 
                break;
            case OrientationBehaviourSelection.SteeringLookWhereYoureGoing:
                SteeringOutput.Angular = SteeringLookWhereYoureGoing.GetSteering().Angular; 
                break;
        };

        UpdateKinematics();
    }

    void Update()
    {
        Move();
    }

    private void ClipMaxVelocities()
    {
        if (Mathf.Abs(Rigidbody.velocity.magnitude) > MaxSpeed)
            Rigidbody.velocity = Rigidbody.velocity.normalized * MaxSpeed;

        if (Mathf.Abs(Rigidbody.angularVelocity.magnitude) > MaxAngularSpeed)
            Rigidbody.velocity = Rigidbody.angularVelocity.normalized * MaxAngularSpeed;
    }

    private void UpdateKinematics()
    {
        // Update position and orientation
        // We Let the Rigidbody do this! 

        // Update velocity and angular velocity
        Rigidbody.velocity += SteeringOutput.Linear * Time.deltaTime;
        Rigidbody.angularVelocity += SteeringOutput.Angular * Time.deltaTime * Vector3.up;

        ClipMaxVelocities();
    }
}
