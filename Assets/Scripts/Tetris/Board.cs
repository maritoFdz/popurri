using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [Header("Board Parameters")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("Tilemap info")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile empty;

    private TetraminoData[,] grid;
    // this way is more intuitive to keep track of actual blocks positions taking bottom left corner as [0,0] instead of the center
    private int offsetX;
    private int offsetY;

    private void Awake()
    {
        grid = new TetraminoData[width, height];
        offsetX = -width / 2;
        offsetY = -height / 2;
    }

    private void Start()
    {
        DrawBoard();
    }

    public void ResetBoard()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[x, y] = null;
        DrawBoard();
    }

    public Vector2Int GetBoardDimensions()
    {
        return new(width, height);
    }

    public void TryMoveDown(Tetramino currentTetra)
    {
        if (CanPlace(currentTetra, currentTetra.pos.x, currentTetra.pos.y - 1))
            currentTetra.pos.y--;
        else
            PlaceTetramino(currentTetra, currentTetra.pos.x, currentTetra.pos.y);
        DrawBoard();
        DrawTetra(currentTetra);
    }

    public void TryMoveHorizontal(Tetramino currentTetra, int direction)
    {
        if (CanPlace(currentTetra, currentTetra.pos.x + direction, currentTetra.pos.y))
        {
            currentTetra.pos.x += direction;
            DrawBoard();
            DrawTetra(currentTetra);
        }
    }

    public void TryRotate(Tetramino currentTetra)
    {
        Vector2Int[] original = new Vector2Int[currentTetra.Rotation.Length];
        Array.Copy(currentTetra.Rotation, original, original.Length);
        currentTetra.Rotate();
        if (!CanPlace(currentTetra, currentTetra.pos.x, currentTetra.pos.y))
        {
            currentTetra.Rotation = original;
            return;
        }
        DrawBoard();
        DrawTetra(currentTetra);
    }

    private void PlaceTetramino(Tetramino currentTetra, int xPos, int yPos)
    {
        foreach (var block in currentTetra.Rotation)
        {
            int x = xPos + block.x;
            int y = yPos + block.y;
            grid[x, y] = currentTetra.data;
        }
        ClearRows();
        DrawBoard();
        TetrisGameManager.instance.TetraPlaced(currentTetra);
    }

    private void ClearRows()
    {
        for (int y = 0; y < height; y++)
            if (IsLineFull(y)) DropRowsAbove(y--);
        DrawBoard();
    }

    private bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
            if (grid[x, y] == null) return false;
        return true;
    }

    private void DropRowsAbove(int row)
    {
        for (int y = row; y < height - 1; y++)
            for (int x = 0; x < width; x++)
                grid[x, y] = grid[x, y + 1];

        // Clean last row
        for (int x = 0; x < width; x++)
            grid[x, height - 1] = null;
    }

    private void DrawBoard()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Tile tile = grid[x, y] == null ? empty : grid[x, y].Tile;
                SetTileInGrid(x, y, tile);
            }
    }

    public void DrawTetra(Tetramino currentTetra)
    {
        foreach (var block in currentTetra.Rotation)
        {
            int x = currentTetra.pos.x + block.x;
            int y = currentTetra.pos.y + block.y;
            if (x >= 0 && x < width && y >= 0 && y < height)
                SetTileInGrid(x, y, currentTetra.data.Tile);
        }
    }

    public bool CanPlace(Tetramino tetra, int xPos, int yPos)
    {
        foreach (var block in tetra.Rotation)
        {
            int x = xPos + block.x;
            int y = yPos + block.y;
            if (x < 0 || x >= width || y < 0 || y >= height) return false; // out of bounds
            if (grid[x, y] != null) return false; // blocking tetramino
        }
        return true;
    }

    private void SetTileInGrid(int x, int y, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(x + offsetX, y + offsetY, 0), tile);
    }
}
