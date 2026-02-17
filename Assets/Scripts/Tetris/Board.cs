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
    [SerializeField] private Tetramino cyan;
    [SerializeField] private Tetramino green;
    [SerializeField] private Tetramino magenta;
    [SerializeField] private Tetramino purple;
    [SerializeField] private Tetramino red;
    [SerializeField] private Tetramino turquoise;
    [SerializeField] private Tetramino yellow;

    private Tetramino[,] grid;
    private int offsetX;
    private int offsetY;

    void Awake()
    {
        grid = new Tetramino[width, height];
        offsetX = -width / 2;
        offsetY = -height / 2;
    }

    private void Start()
    {
        DrawBoard();
    }

    private void DrawBoard()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Tile tile = grid[x, y] == null ? empty : grid[x, y].GetTile();
                tilemap.SetTile(new Vector3Int(x + offsetX, y + offsetY, 0), tile);
            }
    }
}
