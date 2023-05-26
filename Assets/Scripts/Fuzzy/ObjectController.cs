using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = new Vector3(Random.Range(-49, 49), 0, Random.Range(-49, 49));
        agent.destination = target;
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null)
        {
            if (target.x != transform.position.x || target.z != transform.position.z)
            {

            }
            else
            {
                target = new Vector3(Random.Range(-49, 49), 0, Random.Range(-49, 49));
                agent.destination = target;
            }
        }
    }
}
