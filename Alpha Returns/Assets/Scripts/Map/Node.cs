using UnityEngine;

public class Node
{
    public Vector3Int cellPos;
    public Node parent;
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

    public Node(Vector3Int pos)
    {
        cellPos = pos;
    }
}
