using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterMovement2D : MonoBehaviour
{
    public System.Func<Vector3, Vector3> OnSnappedPos;
    public System.Action<Vector3> OnMoveCommand;

    [SerializeField] private Tilemap _floor;
    [SerializeField] private Tilemap _obstacles;
    private Pathfinding _pathfinder;
    private Queue<Vector3> _pathQueue = new Queue<Vector3>();

    private void Awake()
    {
        _pathfinder = new Pathfinding(_floor, _obstacles);
    }

    private void Update()
    {
        if (_pathQueue.Count == 0) return;

        Vector3 target = _pathQueue.Peek();
        transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f) _pathQueue.Dequeue();
    }

    private void OnEnable()
    {
        OnMoveCommand += ProcessMoveCommand;
        OnSnappedPos += GetSnappedPosition;
    }

    private void OnDisable()
    {
        OnMoveCommand -= ProcessMoveCommand;
        OnSnappedPos -= GetSnappedPosition;
    }

    public void ProcessMoveCommand(Vector3 worldTarget)
    {
        Vector3Int start = _floor.WorldToCell(transform.position);
        Vector3Int end = _floor.WorldToCell(worldTarget);

        List<Vector3Int> path = _pathfinder.FindPath(start, end);
        if (path != null)
        {
            _pathQueue.Clear();
            foreach (var pos in path)
                _pathQueue.Enqueue(_floor.GetCellCenterWorld(pos));
        }
    }

    public Vector3 GetSnappedPosition(Vector3 worldPos)
    {
        Vector3Int cellPos = _floor.WorldToCell(worldPos);
        return _floor.GetCellCenterWorld(cellPos);
    }
}
