using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour {
    [Header("Setting Map")]
    [SerializeField] private int landSizeX = 6;
    [SerializeField] private int landSizeY = 6;
    [SerializeField] private int sizeX = 20;
    [SerializeField] private int sizeY = 20;

    [Header("Tile Map")]
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile landTile;
    [SerializeField] private Tile obstacleTile;

    [Header("Tile Base")]
    [SerializeField] private Tilemap tilemap;

    Cell[,] grid;

    public void GenerateGrid()
    {
        grid = new Cell[sizeX, sizeY];

        GenerateRectangularLandGrid();
        DrawTilemap(grid);
    }

    private void GenerateRectangularLandGrid()
    {
        int landStartX = (sizeX - landSizeX) / 2;
        int landStartY = (sizeY - landSizeY) / 2;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                bool isWater = !(x >= landStartX && x < landStartX + landSizeX && 
                                 y >= landStartY && y < landStartY + landSizeY);

                Cell cell = new Cell(isWater);
                grid[x, y] = cell;

                if (!isWater && Random.Range(0f, 1f) > 0.85f)
                {
                    cell.hasObstacle = true;
                }
            }
        }
    }

    private void DrawTilemap(Cell[,] grid)
    {
        if (grid == null) {
            Debug.LogError("Grid is null.");
            return;
        }

        tilemap.ClearAllTiles();

        int offsetX = (sizeX / 2);
        int offsetY = (sizeY / 2);

        for (int y = 0; y < sizeY; y++) {
            for (int x = 0; x < sizeX; x++) {
                Cell cell = grid[x, y];
                if (cell == null) {
                    continue;
                }

                Vector3Int tilePosition = new Vector3Int(x - offsetX, y - offsetY, 0);

                Tile tile = cell.isWater ? waterTile : landTile;
                tilemap.SetTile(tilePosition, tile);

                if (!cell.isWater && cell.hasObstacle)
                {
                    tilemap.SetTile(tilePosition, obstacleTile);
                }
            }
        }
    }

    public void ClearGrid()
    {
        tilemap.ClearAllTiles();
    }
}

#if UNITY_EDITOR
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Grid grid = (Grid)target;

            if (GUILayout.Button("Generate Random Grid"))
            {
                grid.GenerateGrid();
                EditorUtility.SetDirty(grid);
            }

            if (GUILayout.Button("Clear Grid"))
            {
                grid.ClearGrid();
                EditorUtility.SetDirty(grid);
            }
        }
    }
#endif
