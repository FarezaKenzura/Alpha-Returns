using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour {
    [Header("Setting Map")]
    [SerializeField] private float waterLevel = .4f;
    [SerializeField] private float scale = .1f;
    [SerializeField] private int size = 100;

    [Header("Tile Map")]
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile landTile; 

    [Header("Tile Base")]
    [SerializeField] private Tilemap tilemap;

    Cell[,] grid;

    public void GenerateGrid()
    {
        float[,] noiseMap = GenerateNoiseMap();
        float[,] fallOffMap = GenerateFallOffMap();

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= fallOffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }

        DrawTilemap(grid);
    }

    public void ClearGrid()
    {
        tilemap.ClearAllTiles();
    }

    private float[,] GenerateNoiseMap() {
        float[,] noiseMap = new float[size, size];
        (float xOffSet, float yOffSet) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffSet, y * scale + yOffSet);
                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }

    private float[,] GenerateFallOffMap() {
        float[,] fallOffMap = new float[size, size];
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                fallOffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }
        return fallOffMap;
    }

    private void DrawTilemap(Cell[,] grid) {
        if (grid == null) {
            Debug.LogError("Grid is null.");
            return;
        }

        tilemap.ClearAllTiles();
        
        float offSet = size / 2f;
        for (int y = 0; y < size; y++) {
            for (int x = 0; x < size; x++) {
                Cell cell = grid[x, y];
                if (cell == null) {
                    Debug.LogWarning($"Cell at {x},{y} is null.");
                    continue;
                }
                
                Vector3Int tilePosition = new Vector3Int((int)(x - offSet), (int)(y - offSet), 0);
                Tile tile = cell.isWater ? waterTile : landTile;
                tilemap.SetTile(tilePosition, tile);
            }
        }
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