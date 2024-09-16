using UnityEngine;
using TMPro;
using System;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.blue;
    [SerializeField] Color blockedColor = Color.gray;    
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = Color.red;

    Vector2Int _Coordinates = new Vector2Int();

    TextMeshPro _Label;
    GridManager _GridManager;

    private void Awake()
    {
        _GridManager = FindAnyObjectByType<GridManager>();
        _Label = GetComponent<TextMeshPro>();
        _Label.enabled = false;

        DisplayCoordinates();
    }
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            _Label.enabled = true;
        }
        SetLabelColor();
        ToggleLabels();
    }

    private void SetLabelColor()
    {
        if (_GridManager == null) return;

        Node _Node = _GridManager.GetNode(_Coordinates);

        if (_Node == null) return;

        if (!_Node.isWalkable)
        {
            _Label.color = blockedColor;
        }
        else if (_Node.isPath)
        {
            _Label.color = pathColor;
        }
        else if ( _Node.isExplored)
        { 
            _Label.color = exploredColor;
        }
        else
        {
            _Label.color= defaultColor;
        }
    }

    public void DisplayCoordinates()
    {
        if (_GridManager == null)
        {
            return;
        }
        _Coordinates.x = Mathf.RoundToInt(transform.parent.position.x / _GridManager.UnityGridSize);
        _Coordinates.y = Mathf.RoundToInt(transform.parent.position.z / _GridManager.UnityGridSize);
        _Label.text = _Coordinates.x + "," + _Coordinates.y;
    }
    private void UpdateObjectName()
    {
        transform.parent.name = _Coordinates.ToString();
    }
    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _Label.enabled = !_Label.IsActive();
        }
    }

}
