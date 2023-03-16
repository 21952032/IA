using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    public GridManager gridManager;
    private List<PathfindingNode> openList;
    private HashSet<PathfindingNode> closedList;

    public List<PathfindingNode> FindPath(Vector3 startingPoint, Vector3 endingPoint)
    {
        if (gridManager.grid == null)
        {
            gridManager.CreateGrid();
        }
        //Conseguimos el nodo correspondiente a las coordenadas iniciales y finales
        PathfindingNode startingNode = gridManager.FindCorrespondingNode(startingPoint);
        PathfindingNode endingNode = gridManager.FindCorrespondingNode(endingPoint);

        //Creamos la lista abierta y la lista cerrada
        openList = new List<PathfindingNode> { startingNode };
        closedList = new HashSet<PathfindingNode>();
        //Utilizamos un HashSet en vez de lista como una optimización
        //del código, pues solo necesitaremos saber si un nodo pertenece a la lista cerrada en el futuro

        //Asignamos un coste G infinito a todos los nodos y calculamos el coste F
        foreach (PathfindingNode node in gridManager.grid)
        {
            node.SetGCost(int.MaxValue);
            node.CalculateFCost();
            node.SetPreviousNode(null);
        }
        //Asginamos el coste G del nodo inicial y calculamos el coste H
        startingNode.SetGCost(0);
        startingNode.SetHCost(CalculateHCost(startingNode, endingNode));
        startingNode.CalculateFCost();
        while (openList.Count > 0)
        {
            PathfindingNode node = GetCheapestFCostNode(openList);
            if (node.GetWorldPosition() == endingNode.GetWorldPosition())
            {
                return CalculatePath(endingNode);
            }
            closedList.Add(node);
            openList.Remove(node);

            List<PathfindingNode> neighbouringNodes = GetNeighbouringNodes(node);
            foreach (PathfindingNode neighbourNode in neighbouringNodes)
            {
                if (closedList.Contains(neighbourNode)) continue;
                int estimatedCost = node.GetGCost() + CalculateHCost(node, neighbourNode);
                if (estimatedCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPreviousNode(node);
                    neighbourNode.SetGCost(estimatedCost);
                    neighbourNode.SetHCost(CalculateHCost(neighbourNode, endingNode));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode) && neighbourNode.IsWalkable())
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }//Final de la lista abierta

        //Si hemos llegado a esta línea del método es porque no hay un camino posible

        return null;
    }

    private List<PathfindingNode> CalculatePath(PathfindingNode targetNode)
    {
        List<PathfindingNode> path = new List<PathfindingNode>();
        path.Add(targetNode);
        PathfindingNode node = targetNode;
        while (node.GetPreviousNode() != null)
        {
            path.Add(node.GetPreviousNode());
            node = node.GetPreviousNode();
        }
        path.Reverse();
        return path;
    }

    private int CalculateHCost(PathfindingNode pointA, PathfindingNode pointB)
    {
        int x = Mathf.Abs(Mathf.RoundToInt(pointA.GetWorldPosition().x - pointB.GetWorldPosition().x));
        int y = Mathf.Abs(Mathf.RoundToInt(pointA.GetWorldPosition().z - pointB.GetWorldPosition().z));
        //Calculamos la distancia entre el punto A y punto B, cada eje por separado. El punto en el mundo es en el eje Z, pero para estos cálculos utilizaremos X e Y

        //Ignoramos las diagonales con el método Manhattan para calcular HCost, por lo que simplemente sumar el coste en X y el coste en Y será suficiente
        //Además, multiplicamos por 10 para que los futuros cálculos sean más sencillos de realizar
        return 10 * (x + y);
    }

    private PathfindingNode GetCheapestFCostNode(List<PathfindingNode> nodeList)
    {
        PathfindingNode cheapestNode = nodeList[0];
        for (int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].GetFCost() < cheapestNode.GetFCost())
            {
                cheapestNode = nodeList[i];
            }
        }
        return cheapestNode;
    }

    private List<PathfindingNode> GetNeighbouringNodes(PathfindingNode node)
    {
        float x, y;
        x = node.GetWorldPosition().x;
        y = node.GetWorldPosition().z;
        List<PathfindingNode> nodes = new List<PathfindingNode>();

        //x-1
        PathfindingNode potentialNode = gridManager.FindCorrespondingNode(new Vector3(x - 1, 0, y));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x - 1, 0, y - 1));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x - 1, 0, y + 1));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        //x+1
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x + 1, 0, y));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x + 1, 0, y - 1));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x + 1, 0, y + 1));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        //x
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x, 0, y - 1));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        potentialNode = gridManager.FindCorrespondingNode(new Vector3(x, 0, y + 1));
        if (potentialNode != null)
        {
            nodes.Add(potentialNode);
        }
        return nodes;
    }

    public PathfindingNode GetNode(Vector3 coordinates)
    {
        return gridManager.FindCorrespondingNode(coordinates);
    }
}
