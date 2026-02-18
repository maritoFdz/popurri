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

    [Header("FOR TESTING ONLY (will be erased)")]
    [SerializeField] private TetraminoData L;
    [SerializeField] private TetraminoData LInverted;
    [SerializeField] private TetraminoData Z;
    [SerializeField] private TetraminoData ZInverted;
    [SerializeField] private TetraminoData I;
    [SerializeField] private TetraminoData T;
    [SerializeField] private TetraminoData O;

    [SerializeField] private float fallTime;
    private float fallTimer;
    private Tetramino currentTetra;
    private TetraminoData[,] grid;

    // this way is more intuitive to keep track of actual blocks positions taking bottom left corner as [0,0] instead of the center
    private int offsetX;
    private int offsetY;

    void Awake()
    {
        grid = new TetraminoData[width, height];
        offsetX = -width / 2;
        offsetY = -height / 2;
    }

    private void Start()
    {
        DrawBoard();
        SpawnPiece(T);
    }

    private void Update()
    {
        if (currentTetra == null) return;
        
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallTime)
        {
            fallTimer = 0;
            TryMoveDown(currentTetra);
        }
    }

    private void TryMoveDown(Tetramino currentTetra)
    {
        if (CanPlace(currentTetra, currentTetra.pos.x, currentTetra.pos.y - 1))
            currentTetra.pos.y--;
        else
            PlaceTetramino(currentTetra, currentTetra.pos.x, currentTetra.pos.y);
        DrawBoard();
    }

    public void SpawnPiece(TetraminoData data)
    {
        currentTetra = new(data, new(width / 2, height - 1));
        DrawBoard();
    }

    private void DrawBoard()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Tile tile = grid[x, y] == null ? empty : grid[x, y].Tile;
                SetTileInGrid(x, y, tile);
            }
        if (currentTetra != null)
            DrawCurrentTetra();
    }

    private void DrawCurrentTetra()
    {
        foreach (var block in currentTetra.Rotation)
        {
            int x = currentTetra.pos.x + block.x;
            int y = currentTetra.pos.y + block.y;
            SetTileInGrid(x, y, currentTetra.data.Tile);
        }
    }

    private void SetTileInGrid(int x, int y, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(x + offsetX, y + offsetY, 0), tile);
    }

    private void PlaceTetramino(Tetramino tetra, int xPos, int yPos)
    {
        // TODO kick wall if tretramino's placement violates one of the two first conditions
        if (CanPlace(tetra, xPos, yPos))
            foreach (var block in tetra.Rotation)
            {
                int x = xPos + block.x;
                int y = yPos + block.y;

                grid[x, y] = tetra.data;
            }
    }

    private bool CanPlace(Tetramino tetra, int xPos, int yPos)
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
}
