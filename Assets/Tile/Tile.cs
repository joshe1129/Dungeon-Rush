using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Tower towerWizard;
    [SerializeField] bool isPlaceable;
    [SerializeField] bool isWalkable;
    public bool IsPlaceable { get => isPlaceable; set => isPlaceable = value; }
    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }

    GridManager gridManager;
    Pathfinder pathFinder;
    Vector2Int _Coordinates = new Vector2Int();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinder>();
    }

    private void Start()
    {
        if (gridManager != null)
        {
            _Coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isWalkable)
            {
                gridManager.BlockNode(_Coordinates);
            }
        }
    }

    private void OnMouseDown()
    {
        // Permitir colocar torre solo en tiles donde se pueda colocar (isPlaceable sea true)
        if (isPlaceable && gridManager.GetNode(_Coordinates).isWalkable && !pathFinder.WillBlockPath(_Coordinates))
        {
            bool isSuccesful = towerWizard.CreateTower(towerWizard, transform.position);
            if (isSuccesful)
            {
                // Una vez que la torre se ha colocado, el tile ya no es transitable ni colocable
                gridManager.BlockNode(_Coordinates);
                isPlaceable = false;
                isWalkable = false;
                pathFinder.NotifyRecivers();
            }
        }
    }
}