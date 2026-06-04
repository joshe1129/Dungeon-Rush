using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements pathfinding for enemies using the Breadth-First Search (BFS) algorithm.
/// Manages the grid-based navigation system and validates whether tower placement blocks the enemy path.
/// </summary>
public class Pathfinder : MonoBehaviour
{
    /// <summary>
    /// The starting coordinates where enemies spawn (portal location).
    /// </summary>
    [SerializeField] private Vector2Int startCoords;
    public Vector2Int StartCoords => startCoords;

    /// <summary>
    /// The destination coordinates where enemies must reach (castle location).
    /// </summary>
    [SerializeField] private Vector2Int endCoords;
    public Vector2Int EndCoords => endCoords;

    /// <summary>
    /// The starting node in the pathfinding grid.
    /// </summary>
    private Node startNode;

    /// <summary>
    /// The ending/destination node in the pathfinding grid.
    /// </summary>
    private Node endNode;

    /// <summary>
    /// The node currently being examined during BFS traversal.
    /// </summary>
    private Node currentSearchNode;

    /// <summary>
    /// Queue of nodes to explore during the BFS algorithm (frontier).
    /// </summary>
    private Queue<Node> frontier = new Queue<Node>();

    /// <summary>
    /// Dictionary tracking all nodes that have been reached during BFS.
    /// </summary>
    private Dictionary<Vector2Int, Node> reachedNodes = new Dictionary<Vector2Int, Node>();

    /// <summary>
    /// Array of four cardinal directions for grid movement (right, left, up, down).
    /// </summary>
    private Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    /// <summary>
    /// Reference to the GridManager that manages all nodes and grid operations.
    /// </summary>
    private GridManager gridManager;

    /// <summary>
    /// Complete grid dictionary mapping coordinates to their corresponding nodes.
    /// </summary>
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    /// <summary>
    /// Initializes the Pathfinder and retrieves the grid from GridManager.
    /// Called before Start().
    /// </summary>
    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
            startNode = grid[startCoords];
            endNode = grid[endCoords];
        }
    }

    /// <summary>
    /// Initiates the initial pathfinding when the game starts.
    /// </summary>
    private void Start()
    {
        GetNewPath();
    }

    /// <summary>
    /// Calculates a new path from the start coordinates to the destination.
    /// </summary>
    /// <returns>A list of nodes representing the path from start to destination.</returns>
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoords);
    }

    /// <summary>
    /// Calculates a new path from the specified coordinates to the destination.
    /// </summary>
    /// <param name="coordinates">The starting coordinates for pathfinding.</param>
    /// <returns>A list of nodes representing the calculated path.</returns>
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    /// <summary>
    /// Explores all valid neighbors of the current search node.
    /// Adds unvisited, walkable neighbors to the frontier for BFS exploration.
    /// </summary>
    private void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();

        // Check all four cardinal directions for neighbors
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.Coordinates + direction;

            // Add neighbor if it exists in the grid
            if (grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(grid[neighborCoords]);
            }
        }

        // Add valid neighbors (not yet reached and walkable) to the frontier
        foreach (Node neighbor in neighbors)
        {
            if (!reachedNodes.ContainsKey(neighbor.Coordinates) && neighbor.IsWalkable)
            {
                // Link neighbor back to current node for path reconstruction
                neighbor.ConnectedTo = currentSearchNode;
                reachedNodes.Add(neighbor.Coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    /// <summary>
    /// Implements the Breadth-First Search (BFS) algorithm to find a path from the specified coordinates to the destination.
    /// BFS guarantees finding the shortest path in an unweighted grid.
    /// Algorithm:
    /// 1. Start from the given coordinates
    /// 2. Explore all neighbors at distance 1, then distance 2, etc.
    /// 3. Track the path by linking each node to its parent (ConnectedTo)
    /// 4. Stop when the destination is reached
    /// </summary>
    /// <param name="coordinates">The starting coordinates for the search.</param>
    private void BreadthFirstSearch(Vector2Int coordinates)
    {
        // Ensure start and end nodes are always walkable
        startNode.IsWalkable = true;
        endNode.IsWalkable = true;

        // Clear previous search data
        frontier.Clear();
        reachedNodes.Clear();

        // Initialize: add start node to frontier and mark as reached
        frontier.Enqueue(grid[coordinates]);
        reachedNodes.Add(coordinates, grid[coordinates]);

        bool isRunning = true;
        while (frontier.Count > 0 && isRunning)
        {
            // Process next node in frontier (FIFO - breadth-first)
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.IsExplored = true;

            // Explore neighbors of current node
            ExploreNeighbors();

            // Stop if destination is reached
            if (currentSearchNode.Coordinates == endCoords)
            {
                isRunning = false;
            }
        }
    }

    /// <summary>
    /// Reconstructs the path from start to destination by following the ConnectedTo links.
    /// This is called after BFS completes and traverses backward from the end node to the start node,
    /// then reverses the list to get the correct order.
    /// </summary>
    /// <returns>A list of nodes representing the complete path from start to destination.</returns>
    private List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        // Start from destination and trace back using ConnectedTo links
        path.Add(currentNode);
        currentNode.IsPath = true;

        // Follow the chain of ConnectedTo references back to start
        while (currentNode.ConnectedTo != null)
        {
            currentNode = currentNode.ConnectedTo;
            path.Add(currentNode);
            currentNode.IsPath = true;
        }

        // Reverse to get path in correct order (start → destination)
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Determines whether placing a tower at the specified coordinates would completely block the enemy path.
    /// Performs a temporary pathfinding check by blocking the node and seeing if an alternate path exists.
    /// </summary>
    /// <param name="coordinates">The coordinates to test for path blocking.</param>
    /// <returns>True if blocking this node would completely prevent enemies from reaching the destination; false otherwise.</returns>
    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (!grid.ContainsKey(coordinates))
            return false;

        // Temporarily block the node
        bool previousState = grid[coordinates].IsWalkable;
        grid[coordinates].IsWalkable = false;

        // Try to find an alternate path without this node
        List<Node> newPath = GetNewPath();

        // Restore the node's original state
        grid[coordinates].IsWalkable = previousState;

        // If no valid path exists (path.Count <= 1 means only start node found), it would block
        if (newPath.Count <= 1)
        {
            GetNewPath();  // Recalculate original path
            return true;
        }

        return false;
    }

    /// <summary>
    /// Notifies all enemy movers in the scene to recalculate their paths.
    /// Called when towers are placed or grid state changes.
    /// Uses BroadcastMessage to invoke "RecalculatePath" on all EnemyMover components.
    /// </summary>
    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
