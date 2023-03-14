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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            suspiciousLocation = collision.transform.position;
            suspiciousLocation.y *= 0;
        }
    }

    public Vector3 GetTarget()
    {
        return suspiciousLocation;
    }
}
