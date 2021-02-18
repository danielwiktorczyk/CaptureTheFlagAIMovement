using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBounded : MonoBehaviour
{
    [SerializeField] private GameObject Arena;

    private float ArenaWidth;
    private float HalfArenaWidth;
    private float ArenaLength;
    private float HalfArenaLength;

    private void Awake()
    {
        InitializeArena();
    }

    // Update is called once per frame
    void Update()
    {
        BoundPositionToArena();
    }

    private void BoundPositionToArena()
    {
        transform.position = new Vector3(
            BoundValueAbsolute(transform.position.x, HalfArenaWidth),
            transform.position.y,
            BoundValueAbsolute(transform.position.z, HalfArenaLength)
            );
    }

    // TODO revisit this, seems to be a little off at the edges. Maybe the % with floats...?
    private float BoundValueAbsolute(float position, float bound)
    {
        float boundedPosition = position;

        if (position > bound)
            boundedPosition = ((position + bound) % (2 * bound)) - bound;
        else if (position < -bound)
            boundedPosition = ((position - bound) % (2 * bound)) + bound;

        return boundedPosition;
    }

    private void InitializeArena()
    {
        Renderer arenaRenderer = Arena.GetComponent<Renderer>();
        ArenaWidth = arenaRenderer.bounds.size.x;
        ArenaLength = arenaRenderer.bounds.size.z;
        HalfArenaWidth = ArenaWidth / 2;
        HalfArenaLength = ArenaLength / 2;
        //Debug.Log("Arena dimensions as " + new Vector3(ArenaWidth, 0, ArenaLength));
    }
}
