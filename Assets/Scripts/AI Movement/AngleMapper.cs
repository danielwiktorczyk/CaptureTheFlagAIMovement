using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AngleMapper
{
    public static float MapDegreesMidpointZero(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
            angle -= 360f;
        else if (angle < -180f)
            angle += 360f;
        return angle;
    }
}
