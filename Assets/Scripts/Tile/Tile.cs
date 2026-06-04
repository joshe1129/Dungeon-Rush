using UnityEngine;

/// <summary>
/// Represents an individual tile on the game board.
/// Handles tower placement validation, pathfinding integration, and grid state management.
/// Tiles can be placeable (accept towers) and/or walkable (allow enemy passage).
/// </summary>
public class Tile : MonoBehaviour
{
    /// <summary>
    /// The tower prefab to instantiate when this tile is clicked.
    /// </summary>
    [SerializeField] private Tower towerPrefab;

    /// <summary>
    /// Whether towers can be placed on this tile.
    /// </summary>
    [SerializeField] private bool isPlaceable;
    public bool IsPlaceable { get => isPlaceable; set => isPlaceable = value; }

    /// <summary>
    /// Whether enemies can walk through this tile.
    /// Set to false for obstacles or when towers are placed.
    /// </summary>
    [SerializeField] private bool isWalkable;
    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }

    /// <summary>
    /// Reference to the GridManager for coordinate and state management.
    /// </summary>
    private GridManager gridManager;

    /// <summary>
    /// Reference to the Pathfinder for path validation and recalculation.
    /// </summary>
    private Pathfinder pathfinder;

    /// <summary>
    /// The grid coordinates of this tile.
    /// </summary>
    private Vector2Int coordinates = new Vector2Int();

    /// <summary>
    /// Caches references to GridManager and Pathfinder.
    /// Called before Start().
    /// </summary>
    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        pathfinder = FindFirstObjectByType<Pathfinder>();
    }

    /// <summary>
    /// Initializes tile grid coordinates and blocks the node if not walkable.
    /// Called before the first frame update.
    /// </summary>
    private void Start()
    {
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            
            // If this tile is not walkable (e.g., obstacle), mark its node as blocked
            if (!isWalkable)
            {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    /// <summary>
    /// Handles mouse clicks on the tile to place towers.
    /// Validates placement by checking:
    /// 1. Tile is marked as placeable
    /// 2. Node is not already blocked
    /// 3. Tower placement wouldn't block the entire enemy path
    /// If validation passes, creates the tower and notifies enemies to recalculate paths.
    /// </summary>
    private void OnMouseDown()
    {
        // Validate placement conditions
        if (!isPlaceable || !gridManager.GetNode(coordinates).IsWalkable || pathfinder.WillBlockPath(coordinates))
        {
            return;
        }

        // Attempt to create the tower and deduct cost
        bool isSuccessful = towerPrefab.CreateTower(towerPrefab, transform.position);
        
        if (isSuccessful)
        {
            // Update grid state: block node and prevent future placement on this tile
            gridManager.BlockNode(coordinates);
            isPlaceable = false;
            isWalkable = false;
            
            // Notify all enemies to recalculate their paths
            pathfinder.NotifyReceivers();
        }
    }
}