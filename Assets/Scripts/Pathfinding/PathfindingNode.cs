using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private bool isWalkable;
    private Vector3 worldPosition;
    private int FCost, GCost, HCost;
    private PathfindingNode previousNode;

    public PathfindingNode(bool _walkable, Vector3 _worldPosition)
    {
        isWalkable = _walkable;
        worldPosition = _worldPosition;
    }

    public void CalculateFCost()
    {
        FCost = GCost+HCost;
    }

    public int GetFCost()
    {
        return FCost;
    }
    public void SetGCost(int value)
    {
        GCost = value;
    }
    public int GetGCost()
    {
        return GCost;
    }
    public void SetHCost(int value)
    {
        HCost = value;
    }
    public void SetPreviousNode(PathfindingNode value)
    {
        previousNode = value;
    }
    public PathfindingNode GetPreviousNode()
    {
        return previousNode;
    }
    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetWalkable(bool newWalkableState)
    {
        isWalkable=newWalkableState;
    }

    public Vector3 GetWorldPosition()
    {
        return worldPosition;
    }
}
