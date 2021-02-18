using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Teams
{
    Red, 
    Blue, 
    Green,
    Neutral
}

public enum CTFActions
{
    Idle,
    GetFlag,
    RescueTeamate,
    PersueEnemy,
    EvadeEnemy,
    Frozen
}

public class CTFCharacterController : NPCCharacterController
{
    public Teams Team;
    public CTFBase HomeBase;
    public Teams CurrentBase;
    public CTFActions CurrentAction;
    public bool IsFlagChaser;

    public GameObject Body;
    public Material FlagRoleMaterial;
    public Material NormalMaterial;

    public static bool RedFlagSeekerActive;
    public static bool BlueFlagSeekerActive;
    public static bool GreenFlagSeekerActive;

    public bool IsCarryingFlag;
    public GameObject TargetFlag;
    public Flag HomeFlag;

    public CTFCharacterController ClosestEnemyInRange;
    public CTFCharacterController ClosestFrozenAllyInRange;
    public bool IsEnemyInRange;
    public float EnemyRangeRadius;

    public bool IsFrozenInJail;

    override public void Update()
    {
        CalculateClosestEnemy();
        CalculateClosestFrozenAlly();
        CalculateCurrentBase();

        DetermineActionUpdate();

        if (!IsFrozenInJail)
            base.Update();
    }

    public void Start()
    {
        RandomizeTransformInHomeBase();
    }

    private void RandomizeTransformInHomeBase()
    {
        Bounds bounds = HomeBase.GetComponent<Collider>().bounds;
        Vector3 originalPosition = transform.position;
        transform.position = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                0,
                Random.Range(bounds.min.z, bounds.max.z)
                );
        transform.position = new Vector3(
            transform.position.x,
            originalPosition.y,
            transform.position.z
            );

        transform.rotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
    }

    private void DetermineActionUpdate()
    {
        switch (CurrentAction)
        {
            case CTFActions.Idle:
                IdleUpdate();
                break;
            case CTFActions.GetFlag:
                GetFlagUpdate();
                break;
            case CTFActions.RescueTeamate:
                RescueTeamateUpdate();
                break;
            case CTFActions.PersueEnemy:
                PersueEnemyUpdate();
                break;
            case CTFActions.EvadeEnemy:
                EvadeEnemyUpdate();
                break;
            case CTFActions.Frozen:
                FrozenUpdate();
                break;
        }
    }

    private void CalculateClosestEnemy()
    {
        Vector3 closestEnemyDistance;
        if (ClosestEnemyInRange == null || ClosestEnemyInRange.IsFrozenInJail)
        {
            ClosestEnemyInRange = null;
            closestEnemyDistance = new Vector3();
        }
        else
        {
            closestEnemyDistance = ClosestEnemyInRange.transform.position - transform.position;
            if (closestEnemyDistance.magnitude > EnemyRangeRadius)
            {
                ClosestEnemyInRange = null;
                closestEnemyDistance = new Vector3();
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, EnemyRangeRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            CTFCharacterController cTFCharacterController = hitCollider.gameObject.GetComponent<CTFCharacterController>();
            if (cTFCharacterController == null)
                continue;

            if (cTFCharacterController.Team == Team)
                continue;

            if (cTFCharacterController.IsFrozenInJail)
                continue;

            if (ClosestEnemyInRange == null)
            {
                ClosestEnemyInRange = cTFCharacterController;
                closestEnemyDistance = cTFCharacterController.transform.position - transform.position;
            }

            Vector3 distance = cTFCharacterController.transform.position - transform.position;

            if (distance.magnitude < closestEnemyDistance.magnitude)
            {
                ClosestEnemyInRange = cTFCharacterController;
                closestEnemyDistance = distance;
            }
        }

        IsEnemyInRange = ClosestEnemyInRange != null;
    }

    private void CalculateCurrentBase()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(0.001f, 10f, 0.001f));
        CurrentBase = Teams.Neutral;

        foreach (Collider hitCollider in hitColliders)
        {
            CTFBase ctfBase = hitCollider.gameObject.GetComponent<CTFBase>();

            if (ctfBase == null)
                continue;

            CurrentBase = ctfBase.Team;
            return;
        }
    }


    private void IdleUpdate()
    {
        if (ClosestEnemyInRange != null)
        {
            if (CurrentBase == ClosestEnemyInRange.Team)
            {
                // AVOID
                CurrentAction = CTFActions.EvadeEnemy;
                EvadeEnemyUpdate();
                return;
            }

            if (ClosestEnemyInRange.CurrentBase == Team)
            {
                // CHASE
                CurrentAction = CTFActions.PersueEnemy;
                PersueEnemyUpdate();
                return;
            }
        }

        switch (Team)
        {
            case Teams.Red:
                if (!RedFlagSeekerActive || IsFlagChaser)
                {
                    CurrentAction = CTFActions.GetFlag;
                    RedFlagSeekerActive = true;
                    GetFlagUpdate();
                    return;
                }
                break;
            case Teams.Blue:
                if (!BlueFlagSeekerActive || IsFlagChaser)
                {
                    CurrentAction = CTFActions.GetFlag;
                    BlueFlagSeekerActive = true;
                    GetFlagUpdate();
                    return;
                }
                break;
            case Teams.Green:
                if (!GreenFlagSeekerActive || IsFlagChaser)
                {
                    CurrentAction = CTFActions.GetFlag;
                    GreenFlagSeekerActive = true;
                    GetFlagUpdate();
                    return;
                }
                break;
            case Teams.Neutral:
                break;
        }

        if (ClosestFrozenAllyInRange != null)
        {
            CurrentAction = CTFActions.RescueTeamate;
            RescueTeamateUpdate();
        }

        SteeringBehaviourSelection = SteeringBehaviourSelection.SteeringWander;
        KinematicBehaviourSelection = KinematicBehaviourSelection.KinematicWander;
        OrientationBehaviourSelection = OrientationBehaviourSelection.SteeringLookWhereYoureGoing;
        HumanHeuristicMode = HumanHeuristicMode.None;
    }

    private void CalculateClosestFrozenAlly()
    {
        Vector3 closestAllyDistance;
        if (ClosestFrozenAllyInRange == null || !ClosestFrozenAllyInRange.IsFrozenInJail)
        {
            ClosestFrozenAllyInRange = null;
            closestAllyDistance = Vector3.zero;
        }
        else
        {
            closestAllyDistance = ClosestFrozenAllyInRange.transform.position - transform.position;
            if (closestAllyDistance.magnitude > EnemyRangeRadius)
            {
                ClosestFrozenAllyInRange = null;
                closestAllyDistance = Vector3.zero;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, EnemyRangeRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            CTFCharacterController cTFCharacterController = hitCollider.gameObject.GetComponent<CTFCharacterController>();
            if (cTFCharacterController == null)
                continue;

            if (cTFCharacterController.Team != Team)
                continue;

            if (!cTFCharacterController.IsFrozenInJail)
                continue;

            if (ClosestFrozenAllyInRange == null)
            {
                ClosestFrozenAllyInRange = cTFCharacterController;
                closestAllyDistance = cTFCharacterController.transform.position - transform.position;
            }

            Vector3 distance = cTFCharacterController.transform.position - transform.position;

            if (distance.magnitude < closestAllyDistance.magnitude)
            {
                ClosestFrozenAllyInRange = cTFCharacterController;
                closestAllyDistance = distance;
            }
        }
    }

    private void GetFlagUpdate()
    {
        IsFlagChaser = true;

        Body.GetComponent<Renderer>().material = FlagRoleMaterial;

        if (ClosestEnemyInRange != null && CurrentBase == ClosestEnemyInRange.Team)
        {
            if (!IsCarryingFlag)
            {
                RemoveFlagChaserRole();
                CurrentAction = CTFActions.EvadeEnemy;
                DetermineActionUpdate();
                return;
            }
            else
            {
                CurrentAction = CTFActions.EvadeEnemy;
                Target = ClosestEnemyInRange.gameObject;
                TargetRigidbody = Target.GetComponent<Rigidbody>();
                DetermineActionUpdate();
                return;
            }
        }

        if(IsCarryingFlag)
        { // Head on home! To Victory! 
            TargetFlag.transform.position = transform.position;
            Target = HomeBase.gameObject; // replace with closest point, needs architectural update :(
            TargetRigidbody = Target.GetComponent<Rigidbody>();

            float distanceFromGoal = (new Vector3(HomeBase.transform.position.x, transform.position.y, HomeBase.transform.position.z) - transform.position)
                .magnitude;
            if (distanceFromGoal <= 1f)
                FlagBroughtHome();
        }
        else
        { // Find the flag
            GameObject[] flags = GameObject.FindGameObjectsWithTag("flag");
            flags = flags.Where(flag => flag.GetComponent<Flag>().Team != Team).ToArray();
            TargetFlag = flags[0];
            Target = TargetFlag;
            TargetRigidbody = Target.GetComponent<Rigidbody>();

            Vector3 distanceFromFlag = transform.position - Target.transform.position;
            distanceFromFlag.y = 0;

            if (distanceFromFlag.magnitude < SteppingRadius)
                IsCarryingFlag = true;
        }

        SteeringBehaviourSelection = SteeringBehaviourSelection.SteeringArrive;
        KinematicBehaviourSelection = KinematicBehaviourSelection.KinematicSeek;
        OrientationBehaviourSelection = OrientationBehaviourSelection.SteeringFace;
        HumanHeuristicMode = HumanHeuristicMode.ReachingATarget;
    }

    private void FlagBroughtHome()
    {
        Debug.Log("Team " + Team + " wins, for bringing the flag back home first!");

        RemoveFlagChaserRole();

        // let's make sure there are no more goal seekers
        switch (Team)
        {
            case Teams.Red:
                RedFlagSeekerActive = true;
                break;
            case Teams.Blue:
                BlueFlagSeekerActive = true;
                break;
            case Teams.Green:
                GreenFlagSeekerActive = true;
                break;
            case Teams.Neutral:
                break;
        }

        CurrentAction = CTFActions.Idle;
        DetermineActionUpdate();
    }

    private void RemoveFlagChaserRole()
    {
        IsFlagChaser = false;
        switch (Team)
        {
            case Teams.Red:
                RedFlagSeekerActive = false;
                break;
            case Teams.Blue:
                BlueFlagSeekerActive = false;
                break;
            case Teams.Green:
                GreenFlagSeekerActive = false;
                break;
            case Teams.Neutral:
                break;
        }
        Body.GetComponent<Renderer>().material = NormalMaterial;
    }

    private void RescueTeamateUpdate()
    {
        if ((ClosestEnemyInRange != null && CurrentBase == ClosestEnemyInRange.Team)
            || ClosestFrozenAllyInRange == null)
        {
            CurrentAction = CTFActions.Idle;
            DetermineActionUpdate();
            return;
        }

        Target = ClosestFrozenAllyInRange.gameObject;
        TargetRigidbody = Target.GetComponent<Rigidbody>();

        SteeringBehaviourSelection = SteeringBehaviourSelection.SteeringArrive;
        KinematicBehaviourSelection = KinematicBehaviourSelection.KinematicSeek;
        OrientationBehaviourSelection = OrientationBehaviourSelection.SteeringFace;
        HumanHeuristicMode = HumanHeuristicMode.ReachingATarget;
    }

    private void PersueEnemyUpdate()
    {
        // if no more enemy to persue, or the enemy is out of our base
        if (ClosestEnemyInRange == null || ClosestEnemyInRange.CurrentBase != Team)
        {
            CurrentAction = CTFActions.Idle;
            DetermineActionUpdate();
            return;
        }

        Target = ClosestEnemyInRange.gameObject;
        TargetRigidbody = Target.GetComponent<Rigidbody>();

        SteeringBehaviourSelection = SteeringBehaviourSelection.SteeringPersue;
        KinematicBehaviourSelection = KinematicBehaviourSelection.KinematicSeek;
        OrientationBehaviourSelection = OrientationBehaviourSelection.SteeringFace;
        HumanHeuristicMode = HumanHeuristicMode.ReachingATarget;
    }

    private void EvadeEnemyUpdate()
    {
        if (ClosestEnemyInRange == null || CurrentBase != ClosestEnemyInRange.Team)
        {
            CurrentAction = CTFActions.Idle;
            DetermineActionUpdate();
            return;
        }

        if (IsFlagChaser)
            TargetFlag.transform.position = transform.position;

        Target = ClosestEnemyInRange.gameObject;
        TargetRigidbody = Target.GetComponent<Rigidbody>();

        SteeringBehaviourSelection = SteeringBehaviourSelection.SteeringEvade;
        KinematicBehaviourSelection = KinematicBehaviourSelection.KinematicFlee;
        OrientationBehaviourSelection = OrientationBehaviourSelection.SteeringLookWhereYoureGoing;
        HumanHeuristicMode = HumanHeuristicMode.StayingAwayFromATarget;
    }

    private void FrozenUpdate()
    {
        SteeringBehaviourSelection = SteeringBehaviourSelection.Idle;
        KinematicBehaviourSelection = KinematicBehaviourSelection.Idle;
        OrientationBehaviourSelection = OrientationBehaviourSelection.None;
        HumanHeuristicMode = HumanHeuristicMode.None;

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (CurrentAction)
        {
            case CTFActions.RescueTeamate:
                CTFCharacterController ally = other.GetComponent<CTFCharacterController>();
                if (ally == null || ally.Team != Team || !ally.IsFrozenInJail)
                    return;
                ally.Rescue();
                CurrentAction = CTFActions.Idle;
                break;
            case CTFActions.PersueEnemy:
                if (CurrentBase != Team)
                    return;
                CTFCharacterController enemy = other.GetComponent<CTFCharacterController>();
                if (enemy == null || enemy.Team == Team)
                    return;
                enemy.Jail();
                CurrentAction = CTFActions.Idle;
                break;
            default:
                break;
        }

        // even if we aren't rescuing, let's unfreeze any allies we cross
        CTFCharacterController frozenAlly = other.GetComponent<CTFCharacterController>();
        if (frozenAlly != null && frozenAlly.Team == Team && frozenAlly.IsFrozenInJail)
            frozenAlly.Rescue();
    }

    public void Jail()
    {
        if (IsCarryingFlag)
        {
            RemoveFlagChaserRole();
            HomeFlag.GetComponent<Flag>().ResetPosition();
        }

        // If we want to send them back to Home base
        //transform.position = 5f * Vector3.forward + new Vector3(
        //    HomeBase.transform.position.x,
        //    transform.position.y,
        //    HomeBase.transform.position.z
        //);

        IsFlagChaser = false;
        IsCarryingFlag = false;
        IsFrozenInJail = true;

        CurrentAction = CTFActions.Frozen;

        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    public void Rescue()
    {
        IsCarryingFlag = false;
        IsFrozenInJail = false;

        CurrentAction = CTFActions.Idle ;
    }

}
