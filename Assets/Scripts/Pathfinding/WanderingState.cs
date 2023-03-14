using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : MonoBehaviour
{
    public GridManager gridManager;
    public Vector3 GetTarget()
    {
        return gridManager.GetRandomNodeCoordinates();
    }
}
