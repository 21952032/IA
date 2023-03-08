using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private bool isWalkable;
    private Vector3 worldPosition;

    public PathfindingNode(bool _walkable, Vector3 _worldPosition)
    {
        isWalkable = _walkable;
        worldPosition = _worldPosition;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public Vector3 GetWorldPosition()
    {
        return worldPosition;
    }
}
