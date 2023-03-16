using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public LayerMask unwalkableLayerMask;
    public Vector2 gridSize;
    public float nodeRadius;
    public PathfindingNode[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    public void CreateGrid()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        //Creamos la matriz grid
        grid = new PathfindingNode[gridSizeX, gridSizeY];

        //Calculamos las coordenadas del punto inicial
        Vector3 startingPoint = transform.position - 
                                Vector3.right * (gridSize.x / 2) - 
                                Vector3.forward * (gridSize.y / 2);
        //Con esto conseguimos el punto inferior izquierdo del grid

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)//Doble for para recorrer toda la matriz grid
            {
                //Calculamos el punto exacto del mundo correspondiente a las coordenadas x,y del bucle
                Vector3 worldPoint = startingPoint
                                        + Vector3.right * (x * nodeDiameter + nodeRadius)
                                        //Con esto conseguimos el punto central del nodo en el eje X
                                        + Vector3.forward * (y * nodeDiameter + nodeRadius);
                //Repetimos para el eje Z, consiguiendo el punto central del nodo y lo sumamos a startingPoint
                //Calculamos las colisiones del punto
                bool isWalkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableLayerMask));
                //Creamos un nuevo nodo, le damos el punto y la booleana y se lo asignamos a una posición de la matriz
                PathfindingNode newNode = new(isWalkable, worldPoint);
                grid[x, y] = newNode;
            }
        }
    }

    public void OnDrawGizmos()
    {
        //Este gizmo nos permite ver el tamaño de la matriz en el editor para ajustarlo cómoda y visualmente
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 0.2f, gridSize.y));

        //Con este gizmo, al mirar la escena mientras se ejecuta el juego, se podrá ver en blanco los nodos que se pueden usar y en rojo los que no
        if (grid != null)
        {
            foreach (PathfindingNode node in grid)
            {
                if (node.IsWalkable())
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(node.GetWorldPosition(), Vector3.one * (nodeRadius));
            }
        }
    }

    public PathfindingNode FindCorrespondingNode(Vector3 coordinates)
    {//Esta función devuelve un nodo si encuentra la posición del mundo en un nodo
     //o null si coordinates está en una posición fuera del grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)//Doble for para recorrer toda la matriz grid
            {
                if (CoordinatesInsideNode(coordinates, grid[x, y].GetWorldPosition()))
                {
                    return grid[x, y];
                }
            }
        }
        return null;
    }

    public Vector2 FindNodeInGrid(PathfindingNode node)
    {//Este método devuelve el índice en el grid de un nodo. Devuelve -1 si el nodo no está en el grid
        if (node == null)
        {
            Debug.Log("this node is null dumbshit");
        }
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)//Doble for para recorrer toda la matriz grid
            {
                if (node.GetWorldPosition() == grid[x, y].GetWorldPosition())
                {
                    return new Vector2(x, y);
                }
            }
        }
        //Si no se encuentra el nodo se devuelve -1 -1
        return new Vector2(-1,-1);
    }
    private bool CoordinatesInsideNode(Vector3 coordinates, Vector3 nodePosition)
    {
        return (coordinates.x >= nodePosition.x - nodeRadius &&
                coordinates.x < nodePosition.x + nodeRadius &&
                coordinates.z >= nodePosition.z - nodeRadius &&
                coordinates.z < nodePosition.z + nodeRadius);
        //Esta función devuelve true si coordinates está dentro del nodo.
        //El nodo se ha tratado hasta ahora como un círculo, pero si esta función comprobara el radio alrededor de nodePosition
        //entonces habría gran cantidad de espacio que siempre devolverá false al estar entre 4 círculos
        //Con esta implementación comprobamos el cuadrado entero alrededor de nodePosition
    }

    public Vector3 GetRandomNodeCoordinates()
    {
        int x = Mathf.RoundToInt(Random.Range(0, gridSizeX));
        int y = Mathf.RoundToInt(Random.Range(0, gridSizeY));

        return grid[x,y].GetWorldPosition();
    }
}
