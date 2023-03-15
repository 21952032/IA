using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : MonoBehaviour
{
    private Vector3 suspiciousLocation;
    public GameObject alertFeedback;

    private void OnEnable()
    {
        alertFeedback.SetActive(true);
    }
    private void OnDisable()
    {
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
}
