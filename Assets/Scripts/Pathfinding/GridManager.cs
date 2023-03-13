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

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        //Creamos la matriz grid
        grid = new PathfindingNode[gridSizeX, gridSizeY];

        //Calculamos las coordenadas del punto inicial
        Vector3 startingPoint = transform.position - Vector3.right * (gridSize.x / 2) - Vector3.forward * (gridSize.y / 2);//Con esto conseguimos el punto inferior izquierdo del grid

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)//Doble for para recorrer toda la matriz grid
            {
                //Calculamos el punto exacto del mundo correspondiente a las coordenadas x,y del bucle
                Vector3 worldPoint = startingPoint
                                        + Vector3.right * (x * nodeDiameter + nodeRadius)//Con esto conseguimos el punto central del nodo en el eje X
                                        + Vector3.forward * (y * nodeDiameter + nodeRadius);//Repetimos para el eje Z, consiguiendo el punto central del nodo y lo sumamos a startingPoint
                //Calculamos las colisiones del punto
                bool isWalkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableLayerMask));
                //Creamos un nuevo nodo, le damos el punto y la booleana y se lo asignamos a una posici�n de la matriz
                PathfindingNode newNode = new(isWalkable, worldPoint);
                grid[x, y] = newNode;
            }
        }
    }

    public void OnDrawGizmos()
    {
        //Este gizmo nos permite ver el tama�o de la matriz en el editor para ajustarlo c�moda y visualmente
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 0.2f, gridSize.y));

        //Con este gizmo, al mirar la escena mientras se ejecuta el juego, se podr� ver en blanco los nodos que se pueden usar y en rojo los que no
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
    {//Esta funci�n devuelve un nodo si encuentra la posici�n del mundo en un nodo o null si coordinates est� en una posici�n fuera del grid
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

    public void FindNodeInGrid(out int gridx, out int gridy, PathfindingNode node)
    {//Este m�todo devuelve el �ndice en el grid de un nodo. Devuelve -1 si el nodo no est� en el grid
        gridx = -1;
        gridy = -1;
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)//Doble for para recorrer toda la matriz grid
            {
                if(node == grid[x, y])
                {
                    gridx = x;
                    gridy = y;
                }
            }
        }
    }
    private bool CoordinatesInsideNode(Vector3 coordinates, Vector3 nodePosition)
    {
        return (coordinates.x >= nodePosition.x - nodeRadius &&
                coordinates.x < nodePosition.x + nodeRadius &&
                coordinates.y >= nodePosition.y - nodeRadius &&
                coordinates.y < nodePosition.y + nodeRadius);
        //Esta funci�n devuelve true si coordinates est� dentro del nodo.
        //El nodo se ha tratado hasta ahora como un c�rculo, pero si esta funci�n devolviera si coordinates est� dentro del radio alrededor de nodePosition
        //entonces habr�a gran cantidad de espacio que siempre devolver� false al estar entre 4 c�rculos
        //Con esta implementaci�n comprobamos el cuadrado entero alrededor de nodePosition
    }
}
