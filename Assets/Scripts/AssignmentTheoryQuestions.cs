using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentTheoryQuestions : MonoBehaviour
{
    void Start()
    {
        //Q1a();
        //Q1b();
        Q1c();
    }

    private void Q1a()
    {
        Q1Data q1Data = new Q1Data()
        {
            Pc = new Vector2(11, 2),
            Pt = new Vector2(6, 6),
            deltaTime = 0.5f,
            Vm = 10f,
            Am = 24f
        };

        for (int update = 0; update < 5; ++update)
        {
            Vector3 velocityDirection = q1Data.Pt - q1Data.Pc;
            Debug.Log("velocityDirection " + velocityDirection.ToString("F3"));

            Vector3 normalizedVelocityDirection = velocityDirection.normalized;
            Debug.Log("normalizedVelocityDirection " + normalizedVelocityDirection.ToString("F3"));

            Vector3 kinematicSeekVelocity = q1Data.Vm * normalizedVelocityDirection;
            Debug.Log("kinematicSeekVelocity " + kinematicSeekVelocity.ToString("F3"));

            q1Data.Pc += kinematicSeekVelocity * q1Data.deltaTime;
            Debug.Log("Updated q1Data.Pc " + q1Data.Pc.ToString("F3"));
        }
    }

    private void Q1b()
    {
        Q1Data q1Data = new Q1Data()
        {
            Pc = new Vector2(11, 2),
            Pt = new Vector2(6, 6),
            deltaTime = 0.5f,
            Vm = 10f,
            Am = 24f
        };

        for (int update = 0; update < 5; ++update)
        {
            Vector3 velocityDirection = - (q1Data.Pt - q1Data.Pc);
            Debug.Log("velocityDirection " + velocityDirection.ToString("F3"));

            Vector3 normalizedVelocityDirection = velocityDirection.normalized;
            Debug.Log("normalizedVelocityDirection " + normalizedVelocityDirection.ToString("F3"));

            Vector3 kinematicSeekVelocity = q1Data.Vm * normalizedVelocityDirection;
            Debug.Log("kinematicSeekVelocity " + kinematicSeekVelocity.ToString("F3"));

            q1Data.Pc += kinematicSeekVelocity * q1Data.deltaTime;
            Debug.Log("Updated q1Data.Pc " + q1Data.Pc.ToString("F3"));
        }
    }

    private void Q1c()
    {
        Q1Data q1Data = new Q1Data()
        {
            Pc = new Vector2(11, 2),
            Pt = new Vector2(6, 6),
            deltaTime = 0.5f,
            Vm = 10f,
            Am = 24f,
            Vc = new Vector2(0, 4),
        };

        for (int update = 0; update < 5; ++update)
        {
            Vector3 steeringSeekAcceleration = q1Data.Am * (q1Data.Pt - q1Data.Pc).normalized;
            Debug.Log("steeringSeekAcceleration " + steeringSeekAcceleration.ToString("F3"));

            q1Data.Vc += steeringSeekAcceleration * q1Data.deltaTime;
            Debug.Log("q1Data.Vc " + q1Data.Vc.ToString("F3"));

            if (q1Data.Vc.magnitude > q1Data.Vm)
                q1Data.Vc = q1Data.Vm * q1Data.Vc.normalized;
            Debug.Log("q1Data.Vc after clipping " + q1Data.Vc.ToString("F3"));

            q1Data.Pc += q1Data.Vc * q1Data.deltaTime;
            Debug.Log("Updated q1Data.Pc " + q1Data.Pc.ToString("F3"));
        }
    }

    private void Q2a()
    {

    }

    public class Q1Data
    {
        public Vector3 Pc;
        public Vector3 Pt;
        public float deltaTime;
        public float Vm;
        public float Am;
        public Vector3 Vc;
        public Vector3 Ac;
    }

    public class Q2Data
    {
        public Vector3 Pc;
        public Vector3 Pt;
        public float deltaTime;
        public float Vm;
        public float Am;
        public Vector3 Vc;
        public Vector3 Ac;
    }
}
