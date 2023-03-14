using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public Vector3 target;
    private StateMachine stateMachine;
    private AStarAlgorithm AStar;
    private List<PathfindingNode> path;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        AStar = GetComponent<AStarAlgorithm>();
    }

    private void Update()
    {
        if (transform.position != target)
        {
            Vector3.Lerp(transform.position, target, speed);
        }
        else
        {
            Vector3 tempTarget = stateMachine.GetNewTarget();
            if (tempTarget == target && path.Count > 1)
            {
                target = path[1].GetWorldPosition();
                path.RemoveAt(0);
            }
            else
            {
                target = tempTarget;
            }
        }
    }
}
