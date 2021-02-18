using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum KinematicBehaviourSelection
{
    Idle,
    KinematicWander,
    KinematicSeek,
    KinematicFlee,
    KinematicArrive
}

public class KinematicCharacter : MonoBehaviour, AIMovement
{
    public KinematicBehaviourSelection KinematicBehaviourSelection;

    private KinematicSteeringOutput KinematicSteeringOutput;
    
    private Rigidbody Rigidbody;

    public float MaxSpeed;
    public GameObject Target;
    internal Rigidbody TargetRigidbody;

    [SerializeField] private KinematicWander KinematicWander;
    [SerializeField] private KinematicSeek KinematicSeek;
    [SerializeField] private KinematicFlee KinematicFlee;
    [SerializeField] private KinematicArrive KinematicArrive;

    GameObject AIMovement.Target { get => Target; }
    float AIMovement.MaxSpeed { get => MaxSpeed; }
    float AIMovement.MaxAcceleration { get => 0; }
    float AIMovement.MaxAngularSpeed { get => 0; }
    float AIMovement.MaxAngularAcceleration { get => 0; }
    Rigidbody AIMovement.TargetRigidbody => TargetRigidbody;

    Transform AIMovement.transform => transform;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        TargetRigidbody = Target.GetComponent<Rigidbody>();
    }

    private void Move()
    {
        switch (KinematicBehaviourSelection)
        {
            case KinematicBehaviourSelection.KinematicWander:
                KinematicSteeringOutput = KinematicWander.GetSteering();
                break;
            case KinematicBehaviourSelection.KinematicSeek:
                KinematicSteeringOutput = KinematicSeek.GetSteering();
                break;
            case KinematicBehaviourSelection.KinematicFlee:
                KinematicSteeringOutput = KinematicFlee.GetSteering();
                break;
            case KinematicBehaviourSelection.KinematicArrive:
                KinematicSteeringOutput = KinematicArrive.GetSteering();
                break;
            default:
                break;
        }

        UpdateKinematics();

        ClipMaxVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void ClipMaxVelocity()
    {
        if (Rigidbody.velocity.magnitude > MaxSpeed)
            Rigidbody.velocity = Rigidbody.velocity.normalized * MaxSpeed;
    }

    private void UpdateKinematics()
    {
        // Update position
        //transform.position += Rigidbody.velocity * Time.deltaTime; // We can let RB handle this! 
        Debug.DrawLine(transform.position, transform.position + Rigidbody.velocity);

        // Update orientation
        //float newRotationAngle = transform.rotation.eulerAngles.y + (KinematicSteeringOutput.Rotation * Time.deltaTime);
        float newRotationAngle = Mathf.Lerp(
            AngleMapper.MapDegreesMidpointZero(transform.rotation.eulerAngles.y),
            KinematicSteeringOutput.Rotation,
            1f * Time.deltaTime
        );
        Rigidbody.angularVelocity = Vector3.zero; // We want to force our rotations
        transform.rotation = Quaternion.AngleAxis(newRotationAngle, Vector3.up);

        // Update velocity
        Rigidbody.velocity = KinematicSteeringOutput.Velocity; // += or =, I think just =

        // We do not update Angular velocity for KinematicMovement 
    }
}
