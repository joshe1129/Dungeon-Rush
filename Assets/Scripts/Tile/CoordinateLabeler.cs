using UnityEngine;
using TMPro;

/// <summary>
/// Debug utility that displays grid coordinates on tiles and visualizes pathfinding data.
/// Shows coordinate labels and uses color coding to indicate node state:
/// - Blue: Walkable, unexplored
/// - Yellow: Explored during pathfinding
/// - Red: Part of the final path
/// - Gray: Blocked/unwalkable
/// Press 'C' to toggle label visibility.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    /// <summary>
    /// Color for walkable nodes that haven't been explored.
    /// </summary>
    [SerializeField] private Color defaultColor = Color.blue;

    /// <summary>
    /// Color for blocked/unwalkable nodes.
    /// </summary>
    [SerializeField] private Color blockedColor = Color.gray;

    /// <summary>
    /// Color for nodes explored during BFS but not on the final path.
    /// </summary>
    [SerializeField] private Color exploredColor = Color.yellow;

    /// <summary>
    /// Color for nodes that are part of the final enemy path.
    /// </summary>
    [SerializeField] private Color pathColor = Color.red;

    /// <summary>
    /// The grid coordinates of this tile.
    /// </summary>
    private Vector2Int coordinates = new Vector2Int();

    /// <summary>
    /// The TextMeshPro component for displaying coordinates.
    /// </summary>
    private TextMeshPro label;

    /// <summary>
    /// Reference to the GridManager for node state queries.
    /// </summary>
    private GridManager gridManager;

    /// <summary>
    /// Initializes references and calculates initial coordinates.
    /// Runs in both play and edit modes.
    /// </summary>
    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        label = GetComponent<TextMeshPro>();
        
        if (label == null)
        {
            Debug.LogWarning("CoordinateLabeler: TextMeshPro component not found!");
            return;
        }
        
        label.enabled = false;

        if (gridManager != null && transform.parent != null)
        {
            DisplayCoordinates();
        }
    }

    /// <summary>
    /// Updates label display and color based on node state every frame.
    /// Updates coordinates and parent name in editor mode for real-time visualization.
    /// </summary>
    private void Update()
    {
        // Try to find GridManager if not already found
        if (gridManager == null)
        {
            gridManager = FindAnyObjectByType<GridManager>();
        }

        // Skip if GridManager not found
        if (gridManager == null)
            return;

        // In editor mode, continuously update coordinates for better visualization
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            //UpdateObjectName(); // For debugging
            label.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }

    /// <summary>
    /// Updates the label color based on the node's current pathfinding state.
    /// </summary>
    private void SetLabelColor()
    {
        if (gridManager == null)
            return;

        Node node = gridManager.GetNode(coordinates);
        if (node == null)
            return;

        // Color priority: blocked > path > explored > default
        if (!node.IsWalkable)
        {
            label.color = blockedColor;
        }
        else if (node.IsPath)
        {
            label.color = pathColor;
        }
        else if (node.IsExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    /// <summary>
    /// Updates the text label to display current grid coordinates.
    /// </summary>
    private void DisplayCoordinates()
    {
        if (gridManager == null)
        {
            return;
        }

        // Check if this component has a parent transform
        if (transform.parent == null)
        {
            return;
        }

        // Check if label exists
        if (label == null)
        {
            label = GetComponent<TextMeshPro>();
            if (label == null)
                return;
        }

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        label.text = coordinates.x + "," + coordinates.y;
    }

    /// <summary>
    /// Updates the parent tile's name to match its coordinates.
    /// Useful for identifying tiles in the scene hierarchy during development.
    /// </summary>
    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    /// <summary>
    /// Toggles label visibility when the 'C' key is pressed.
    /// Useful for hiding debug information during gameplay.
    /// </summary>
    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive();
        }
    }
}
