using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingState : MonoBehaviour
{
    public MonoBehaviour alertState;
    public Collider hearingCollider;
    private StateMachine stateMachine;
    private void OnEnable()
    {
        hearingCollider.enabled = true;
        stateMachine = GetComponent<StateMachine>();
    }

    private void OnDisable()
    {
        hearingCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stateMachine.ChangeCurrentStatus(alertState);
        }
    }
}
