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

    private TetraminoData[,] grid;
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
    }

    private void DrawBoard()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                Tile tile = grid[x, y] == null ? empty : grid[x, y].Tile;
                tilemap.SetTile(new Vector3Int(x + offsetX, y + offsetY, 0), tile);
            }
    }
}
