using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public Teams Team;
    Vector3 SpawnPosition;

    private void Awake()
    {
        SpawnPosition = transform.position;
    }

    internal void ResetPosition()
    {
        transform.position = SpawnPosition;
    }
}
