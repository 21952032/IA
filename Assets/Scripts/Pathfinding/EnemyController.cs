using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private Vector3 target, destination;
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
        //target es el centro del siguiente nodo, si no se ha llegado a ese punto sigue moviendose hacia target
        if (transform.position != target)
        {
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(target, transform.position) > 0.1f)
            {
                transform.forward = (target - transform.position).normalized;
            }
        }
        else
        {//Si se ha llegado a target...
            if (path.Count>1)//Si no se ha llegado al destino, es decir, el último nodo, se recalcula el camino hacia el destino.
            {//Se hace de esta forma porque otro enemigo puede estar ocupando un nodo del camino. 
                SetNewTarget(destination);//Si fuera una implementación de un solo agente se podría eliminar esta línea
                path.RemoveAt(0);
                target = path[0].GetWorldPosition();
                transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            }
            else
            {//Si, por el contrario, hemos llegado al objetivo pedimos un nuevo objetivo. Este script no se encarga de distinguir objetivos, solo mueve al agente
                SetNewTarget(stateMachine.GetNewTarget());
            }
        }
        //Este código marca el nodo en el que se sitúa el agente en este fotograma como no transitable. Desmarca el anterior una vez deja de estar en el nodo
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
        destination = newTarget;
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
