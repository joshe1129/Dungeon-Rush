using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;

    [SerializeField] int unityGridSize = 2;
    public int UnityGridSize {  get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> _Grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return _Grid; } }

    private void Awake()
    {
        _Grid.Clear();
        CreateGrid();
    }
    public Node GetNode(Vector2Int coordinates)
    {
        if (_Grid.ContainsKey(coordinates))
        {
            return _Grid[coordinates];
        }
        return null;
    }
    public void BlockNode(Vector2Int coordinates)
    {
        if (_Grid.ContainsKey(coordinates))
        { 
            _Grid[coordinates].isWalkable = false; 
        }  
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in _Grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }


    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);

        return coordinates;
    }
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;

        return position;
    }
    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int _Coordinates = new Vector2Int(x, y);
                _Grid.Add(_Coordinates, new Node(_Coordinates, true));
            }
        }
    }
}
