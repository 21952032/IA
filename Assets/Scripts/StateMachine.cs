using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private MonoBehaviour wanderingState, alertState, chasingState;

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
}
