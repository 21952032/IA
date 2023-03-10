using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private MonoBehaviour wanderingState, alertState, chasingState;
    private AlertState alertScript;
    public GameObject player;
    public GridManager gridManager;

    private MonoBehaviour currentState;

    private void Awake()
    {
        currentState = wanderingState;
        currentState.enabled = true;
    }

    public void ChangeCurrentStatus(MonoBehaviour newState)
    {
        if (currentState != null)
        {
            currentState.enabled = false;
        }
        currentState = newState;
        currentState.enabled = true;
    }

    public Vector3 GetNewTarget()
    {
        Vector3 target = new Vector3();
        if (currentState == wanderingState)
        {
            target = gridManager.GetRandomNodeCoordinates();
        }
        if(currentState == alertState)
        {
            alertScript=GetComponent<AlertState>();
            target=alertScript.GetTarget();
        }
        if (currentState == chasingState)
        {
            target = player.transform.position;
        }
        return target;
    }
}
