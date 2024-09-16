using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoords;
    public Vector2Int StartCoords { get { return startCoords; } }
    [SerializeField] Vector2Int endCoords;
    public Vector2Int EndCoords { get { return endCoords; } }

    Node startNode;
    Node endNode;
    Node currentNode;
    Node currentSearchNode;

    Queue<Node> _Frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reachedNode = new Dictionary<Vector2Int, Node>();


    Vector2Int[] _Directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> _Grid = new Dictionary<Vector2Int, Node>();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            _Grid = gridManager.Grid;
            startNode = _Grid[startCoords];
            endNode = _Grid[endCoords];
        }
    }
    void Start()
    {
        GetNewPath();
    }
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoords);
    }
    public List<Node> GetNewPath(Vector2Int _Coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(_Coordinates);
        return BuildPath();
    }
    private void ExploreNeighbors()
    {
        List<Node> _Neighbors = new List<Node>();

        foreach (Vector2Int _Direction in _Directions)
        {
            Vector2Int neighborCoords = currentSearchNode._Coordinates + _Direction;

            if (_Grid.ContainsKey(neighborCoords))
            {
                _Neighbors.Add(_Grid[neighborCoords]);
            }
        }

        foreach (Node neighbor in _Neighbors)
        {
            if (!reachedNode.ContainsKey(neighbor._Coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = currentSearchNode;
                reachedNode.Add(neighbor._Coordinates, neighbor);
                _Frontier.Enqueue(neighbor);
            }
        }
    }

    void BreadthFirstSearch(Vector2Int _Coordinates)
    {
        startNode.isWalkable = true;
        endNode.isWalkable = true;

        _Frontier.Clear();
        reachedNode.Clear();

        bool isRunning = true;
        _Frontier.Enqueue(_Grid[_Coordinates]);
        reachedNode.Add(_Coordinates, _Grid[_Coordinates]);

        while (_Frontier.Count > 0 && isRunning)
        {
            currentSearchNode = _Frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (currentSearchNode._Coordinates == endCoords)
            {
                isRunning = false;
            }
        }
    }
    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        path.Add(currentNode);
        currentNode.isPath = true;
        while (currentNode.connectedTo != null) 
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();

        return path;    
    }
    public bool WillBlockPath(Vector2Int _Coordinates)
    {
        if (_Grid.ContainsKey(_Coordinates))
        {
            bool previusState = _Grid[_Coordinates].isWalkable;
            _Grid[_Coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            _Grid[_Coordinates].isWalkable = previusState;

            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyRecivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }

}
