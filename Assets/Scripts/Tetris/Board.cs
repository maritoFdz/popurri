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




    [SerializeField] private float fallTime;
    private float fallMultiplier;
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
        fallMultiplier = 1;
    }

    private void Start()
    {
        TetraminoSpawner.instance.SpawnTetra();
        DrawBoard();
    }

    private void Update()
    {
        if (currentTetra == null) return;
        HandleInput();
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallTime * fallMultiplier)
        {
            fallTimer = 0;
            TryMoveDown(currentTetra);
        }
    }

    public void SetCurrentTetra(Tetramino currentTetra)
    {
        this.currentTetra = currentTetra;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMoveHorizontal(currentTetra, -1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMoveHorizontal(currentTetra, 1);

        fallMultiplier = Input.GetKey(KeyCode.DownArrow) ? 0.1f : 1;
    }

    private void TryMoveDown(Tetramino currentTetra)
    {
        if (CanPlace(currentTetra, currentTetra.pos.x, currentTetra.pos.y - 1))
            currentTetra.pos.y--;
        else
            PlaceTetramino(currentTetra, currentTetra.pos.x, currentTetra.pos.y);
        DrawBoard();
    }

    private void TryMoveHorizontal(Tetramino currentTetra, int direction)
    {
        if (CanPlace(currentTetra, currentTetra.pos.x + direction, currentTetra.pos.y))
        {
            currentTetra.pos.x += direction;
            DrawBoard();
        }
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
            if (x >= 0 && x < width && y >= 0 && y < height)
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
        TetraminoSpawner.instance.SpawnTetra();
    }

    public Vector2Int GetBoardDimensions()
    {
        return new(width / 2, height - 1);
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
