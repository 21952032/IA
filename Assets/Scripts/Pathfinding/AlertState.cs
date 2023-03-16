using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : MonoBehaviour
{
    private Vector3 suspiciousLocation;
    private StateMachine stateMachine;
    public GameObject alertFeedback, player;
    public MonoBehaviour wanderingState, chasingState;
    public float timeInAlert;

    private void OnEnable()
    {
        alertFeedback.SetActive(true);
        stateMachine = GetComponent<StateMachine>();
        Invoke(nameof(ReturnToWandering), timeInAlert);
        SetSuspiciusLocation(player.transform.position);
    }

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                stateMachine.ChangeCurrentStatus(chasingState);
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        alertFeedback.SetActive(false);
    }

    public Vector3 GetTarget()
    {
        return suspiciousLocation;
    }

    public void SetSuspiciusLocation(Vector3 vector)
    {
        suspiciousLocation = vector;
        suspiciousLocation.y *= 0;
    }

    private void ReturnToWandering()
    {
        stateMachine.ChangeCurrentStatus(wanderingState);
    }
}
