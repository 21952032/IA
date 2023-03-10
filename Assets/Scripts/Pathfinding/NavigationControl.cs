using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationControl : MonoBehaviour
{
    public Transform target;
    private Vector3 destination;
    
    public void UpdateDestination(Vector3 newDestination)
    {
        destination = newDestination;
    }
    public void UpdateDestination()
    {
        destination=target.position;
    }
}
