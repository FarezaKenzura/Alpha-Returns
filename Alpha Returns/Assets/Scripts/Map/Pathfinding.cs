using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    private Tilemap _floor;
    private Tilemap _obstacles;

    public Pathfinding(Tilemap floor, Tilemap obstacles)
    {
        _floor = floor;
        _obstacles = obstacles;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();

        Node startNode = new Node(start);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost) currentNode = openList[i];
            }

            if (currentNode.cellPos == target) return RetracePath(startNode, currentNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode.cellPos);

            foreach (Vector3Int neighborPos in GetNeighbors(currentNode.cellPos))
            {
                if (closedList.Contains(neighborPos) || _obstacles.HasTile(neighborPos) || !_floor.HasTile(neighborPos))
                    continue;

                Node neighborNode = new Node(neighborPos);
                neighborNode.parent = currentNode;
                neighborNode.gCost = currentNode.gCost + 1;
                neighborNode.hCost = Mathf.Abs(neighborPos.x - target.x) + Mathf.Abs(neighborPos.y - target.y);

                bool skip = false;
                foreach (Node openNode in openList)
                {
                    if (openNode.cellPos == neighborPos && neighborNode.gCost >= openNode.gCost)
                    {
                        skip = true; break;
                    }
                }
                if (!skip) openList.Add(neighborNode);
            }
        }
        return null;
    }

    private List<Vector3Int> RetracePath(Node start, Node end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node curr = end;
        while (curr != start)
        {
            path.Add(curr.cellPos);
            curr = curr.parent;
        }
        path.Reverse();
        return path;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        return new List<Vector3Int> {
            pos + Vector3Int.up, pos + Vector3Int.down, pos + Vector3Int.left, pos + Vector3Int.right
        };
    }
}
