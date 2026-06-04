using UnityEngine;

/// <summary>
/// Represents a node in the grid-based pathfinding system.
/// Stores state information about walkability, exploration, and pathfinding.
/// </summary>
[System.Serializable]
public class Node 
{
    /// <summary>
    /// The grid coordinates (x, y) representing this node's position in the pathfinding grid.
    /// </summary>
    public Vector2Int Coordinates;

    /// <summary>
    /// Indicates whether this node can be walked through by enemies.
    /// Set to false when blocked by towers or obstacles.
    /// </summary>
    public bool IsWalkable;

    /// <summary>
    /// Indicates whether this node has been explored during the Breadth-First Search pathfinding algorithm.
    /// Used for visualization and algorithm validation.
    /// </summary>
    public bool IsExplored;

    /// <summary>
    /// Indicates whether this node is part of the final calculated path from start to destination.
    /// Used for visualization and debugging the pathfinding route.
    /// </summary>
    public bool IsPath;

    /// <summary>
    /// Reference to the previous node in the path during BFS traversal.
    /// Used to reconstruct the complete path from destination back to start.
    /// </summary>
    public Node ConnectedTo;

    /// <summary>
    /// Initializes a new Node with the specified coordinates and walkability state.
    /// </summary>
    /// <param name="coordinates">The grid coordinates for this node.</param>
    /// <param name="isWalkable">Whether this node can be traversed (default: true).</param>
    public Node(Vector2Int coordinates, bool isWalkable)
    {
        Coordinates = coordinates;
        IsWalkable = isWalkable;
    }
}
