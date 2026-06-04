using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the grid structure used for pathfinding.
/// Handles creation, storage, and manipulation of grid nodes, coordinate conversion, and node state management.
/// </summary>
public class GridManager : MonoBehaviour
{
    /// <summary>
    /// The size of the grid (width x height in grid units).
    /// </summary>
    [SerializeField] private Vector2Int gridSize;

    /// <summary>
    /// The scale factor that converts grid coordinates to world positions.
    /// For example, a grid coordinate (1,1) with unityGridSize=2 becomes world position (2,2).
    /// </summary>
    [SerializeField] private int unityGridSize = 2;
    public int UnityGridSize => unityGridSize;

    /// <summary>
    /// Dictionary storing all nodes mapped by their grid coordinates.
    /// Used for fast node lookups during pathfinding and movement validation.
    /// </summary>
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid => grid;

    /// <summary>
    /// Initializes the grid by creating all nodes.
    /// Called before Start().
    /// </summary>
    private void Awake()
    {
        grid.Clear();
        CreateGrid();
    }

    /// <summary>
    /// Retrieves a specific node from the grid by its coordinates.
    /// </summary>
    /// <param name="coordinates">The grid coordinates to lookup.</param>
    /// <returns>The node at the specified coordinates, or null if not found.</returns>
    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }

    /// <summary>
    /// Marks a node as not walkable (blocked by a tower or obstacle).
    /// </summary>
    /// <param name="coordinates">The coordinates of the node to block.</param>
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].IsWalkable = false;
        }
    }

    /// <summary>
    /// Resets the pathfinding state of all nodes.
    /// Called before each new pathfinding calculation to clear previous search data.
    /// </summary>
    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.ConnectedTo = null;
            entry.Value.IsExplored = false;
            entry.Value.IsPath = false;
        }
    }

    /// <summary>
    /// Converts a world position to grid coordinates.
    /// </summary>
    /// <param name="position">The world position to convert.</param>
    /// <returns>The grid coordinates corresponding to the world position.</returns>
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    }

    /// <summary>
    /// Converts grid coordinates to a world position.
    /// Note: Y coordinate in grid space maps to Z coordinate in world space.
    /// </summary>
    /// <param name="coordinates">The grid coordinates to convert.</param>
    /// <returns>The world position corresponding to the grid coordinates.</returns>
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        return position;
    }

    /// <summary>
    /// Creates all nodes in the grid based on the gridSize.
    /// Each node is initialized as walkable by default.
    /// </summary>
    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}
