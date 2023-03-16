using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private Vector3 target;
    private StateMachine stateMachine;
    private AStarAlgorithm AStar;
    private List<PathfindingNode> path;
    private PathfindingNode currentNode, previousNode;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        AStar = GetComponent<AStarAlgorithm>();
        SetNewTarget(new Vector3(Random.Range(-10f,10f),0, Random.Range(-10f, 10f)));
        currentNode = AStar.GetNode(transform.position);
        currentNode.SetWalkable(false);
        previousNode = currentNode;
    }

    private void Update()
    {
        if (transform.position != target)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if (path.Count>1)
            {
                path.RemoveAt(0);
                target = path[0].GetWorldPosition();
                transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                SetNewTarget(stateMachine.GetNewTarget());
            }
        }

        currentNode = AStar.GetNode(transform.position);
        if (currentNode != previousNode)
        {
            previousNode.SetWalkable(true);
            currentNode.SetWalkable(false);
            previousNode = currentNode;
        }
    }

    public void SetNewTarget(Vector3 newTarget)
    {
        Debug.Log("Next target is:"+newTarget);
        List<PathfindingNode> tempList= AStar.FindPath(transform.position, newTarget);
        if(tempList!= null)
        {
            path = tempList;
            target = path[0].GetWorldPosition();
        }
        else
        {
            Debug.Log("No path found");
            SetNewTarget(stateMachine.GetNewTarget());
        }
    }
}
