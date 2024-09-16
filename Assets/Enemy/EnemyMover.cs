using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0f,7f)] float _Speed = 0.7f;

    List<Node> _Path = new List<Node>();

    Enemy _Enemy;
    GridManager gridManager;
    Pathfinder pathFinder;

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
    }
    private void Awake()
    {
        _Enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinder>();
    }
    void RecalculatePath(bool ResetPath)
    {
        Vector2Int _Coordinates = new Vector2Int();
        if (ResetPath)
        {
            _Coordinates = pathFinder.StartCoords;
        }
        else 
        {
            _Coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        _Path.Clear();
        _Path = pathFinder.GetNewPath(_Coordinates);
        StartCoroutine(FollowPath());
    }
    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoords);
    }
    IEnumerator FollowPath()
    {
        for(int i = 1; i < _Path.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(_Path[i]._Coordinates);
            float travelPercent = 0f;
            transform.LookAt(endPosition);
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * _Speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        FinishPath();
    }

    private void FinishPath()
    {
        _Enemy.StealGold();
        gameObject.SetActive(false);
    }
}
