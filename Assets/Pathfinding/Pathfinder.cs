using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    // Coordenadas iniciales y finales del camino (definidas en el Inspector)
    [SerializeField] Vector2Int startCoords;  // Coordenadas de inicio
    public Vector2Int StartCoords { get { return startCoords; } }
    [SerializeField] Vector2Int endCoords;    // Coordenadas de destino
    public Vector2Int EndCoords { get { return endCoords; } }

    // Nodos que representan el punto de inicio, el final y el nodo actual en la búsqueda
    Node startNode;  
    Node endNode;    
    Node currentNode; 
    Node currentSearchNode; // Nodo actual en la búsqueda BFS

    // Cola para la frontera de búsqueda y diccionario para almacenar los nodos alcanzados
    Queue<Node> _Frontier = new Queue<Node>();  
    Dictionary<Vector2Int, Node> reachedNode = new Dictionary<Vector2Int, Node>();

    // Direcciones posibles para moverse en el grid (arriba, abajo, izquierda, derecha)
    Vector2Int[] _Directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    
    // Referencia al GridManager que gestiona los nodos del mapa
    GridManager gridManager; 
    Dictionary<Vector2Int, Node> _Grid = new Dictionary<Vector2Int, Node>(); // El grid con todos los nodos

    // Inicializa el Pathfinder y configura los nodos de inicio y fin
    private void Awake()
    {
        gridManager = FindFirstObjectByType <GridManager>(); // Busca el GridManager en la escena
        if (gridManager != null)
        {
            _Grid = gridManager.Grid;               // Obtiene el grid de nodos
            startNode = _Grid[startCoords];         // Define el nodo de inicio
            endNode = _Grid[endCoords];             // Define el nodo de fin
        }
    }

    // Inicia la búsqueda del camino cuando el juego comienza
    void Start()
    {
        GetNewPath();  // Genera un nuevo camino al iniciar
    }

    // Método público para obtener un nuevo camino desde el nodo de inicio
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoords);
    }

    // Sobrecarga para obtener un nuevo camino desde coordenadas específicas
    public List<Node> GetNewPath(Vector2Int _Coordinates)
    {
        gridManager.ResetNodes();  // Resetea el estado de todos los nodos en el grid
        BreadthFirstSearch(_Coordinates);  // Realiza la búsqueda en anchura desde las coordenadas proporcionadas
        return BuildPath();  // Construye y devuelve el camino encontrado
    }

    // Explora los nodos vecinos del nodo actual
    private void ExploreNeighbors()
    {
        List<Node> _Neighbors = new List<Node>();

        // Verifica los vecinos en las cuatro direcciones cardinales
        foreach (Vector2Int _Direction in _Directions)
        {
            Vector2Int neighborCoords = currentSearchNode._Coordinates + _Direction;

            // Si el vecino existe en el grid, se agrega a la lista de vecinos
            if (_Grid.ContainsKey(neighborCoords))
            {
                _Neighbors.Add(_Grid[neighborCoords]);
            }
        }

        // Añade vecinos válidos (que no han sido alcanzados y son transitables) a la frontera
        foreach (Node neighbor in _Neighbors)
        {
            if (!reachedNode.ContainsKey(neighbor._Coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;  // Conecta el vecino al nodo actual
                reachedNode.Add(neighbor._Coordinates, neighbor); // Marca el vecino como alcanzado
                _Frontier.Enqueue(neighbor);  // Añade el vecino a la frontera para explorarlo luego
            }
        }
    }

    // Algoritmo de Búsqueda en Anchura (BFS)
    void BreadthFirstSearch(Vector2Int _Coordinates)
    {
        // Asegura que los nodos de inicio y fin sean transitables
        startNode.isWalkable = true;
        endNode.isWalkable = true;

        // Limpia la frontera y los nodos alcanzados antes de iniciar la búsqueda
        _Frontier.Clear();
        reachedNode.Clear();

        // Empieza la búsqueda desde las coordenadas de inicio
        bool isRunning = true;
        _Frontier.Enqueue(_Grid[_Coordinates]);  // Añade el nodo de inicio a la frontera
        reachedNode.Add(_Coordinates, _Grid[_Coordinates]);  // Marca el nodo de inicio como alcanzado

        // Mientras haya nodos por explorar y la búsqueda siga en curso
        while (_Frontier.Count > 0 && isRunning)
        {
            currentSearchNode = _Frontier.Dequeue();  // Toma el nodo actual de la frontera
            currentSearchNode.isExplored = true;      // Marca el nodo como explorado

            // Explora los vecinos del nodo actual
            ExploreNeighbors();

            // Si el nodo actual es el nodo final, detiene la búsqueda
            if (currentSearchNode._Coordinates == endCoords)
            {
                isRunning = false;
            }
        }
    }

    // Construye el camino desde el nodo final hasta el nodo de inicio
    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;  // Empieza desde el nodo final

        // Añade el nodo final al camino
        path.Add(currentNode);
        currentNode.isPath = true;

        // Retrocede por los nodos conectados hasta llegar al nodo de inicio
        while (currentNode.connectedTo != null) 
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;  // Marca los nodos del camino
        }

        path.Reverse();  // Invierte el camino para que esté en el orden correcto (desde inicio a fin)

        return path;  // Devuelve el camino completo
    }

    // Verifica si bloquear un nodo específico impide que haya un camino disponible
    public bool WillBlockPath(Vector2Int _Coordinates)
    {
        if (_Grid.ContainsKey(_Coordinates))
        {
            // Guarda el estado previo del nodo y lo marca como no transitable temporalmente
            bool previusState = _Grid[_Coordinates].isWalkable;
            _Grid[_Coordinates].isWalkable = false;

            // Genera un nuevo camino sin este nodo
            List<Node> newPath = GetNewPath();

            // Restaura el estado del nodo
            _Grid[_Coordinates].isWalkable = previusState;

            // Si no se encuentra un camino válido, el nodo bloquea el camino
            if (newPath.Count <= 1)
            {
                GetNewPath();  // Vuelve a calcular el camino original
                return true;
            }
        }

        return false;  // Si hay un camino válido, no se bloquea
    }

    // Notifica a otros objetos en la escena para que recalculen su camino
    public void NotifyRecivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
