using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private MonoBehaviour wanderingState, alertState, chasingState;
    private AlertState alertScript;
    public GameObject player;
    public GridManager gridManager;
    private EnemyController controller;
    private MonoBehaviour currentState;

    private void Start()
    {
        InitializeMachine();
    }
    private void InitializeMachine()
    {
        controller =GetComponent<EnemyController>();
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
        controller.SetNewTarget(GetNewTarget());
    }

    public Vector3 GetNewTarget()
    {
        Vector3 target = new();
        if(currentState != null)
        {
            Debug.Log("Getting new target...\n Current state is: "+currentState.ToString());
        }
        else
        {
            InitializeMachine();
        }
        if (currentState == wanderingState)
        {
            target = gridManager.GetRandomNodeCoordinates();
        }
        if(currentState == alertState)
        {
            alertScript=GetComponent<AlertState>();
            target=alertScript.GetTarget();
            //Esta recursión es una medida de seguridad por si se da el caso
            //de que el jugador sea detectado en un nodo imposible de alcanzar
            if (!gridManager.FindCorrespondingNode(target).IsWalkable())
            {
                ChangeCurrentStatus(wanderingState);
                Debug.Log("Player in unwalkable node");
                target = GetNewTarget();
            }
        }
        if (currentState == chasingState)
        {
            target = player.transform.position;
        }
        return target;
    }
}
