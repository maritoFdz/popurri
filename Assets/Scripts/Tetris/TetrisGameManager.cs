using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    public static TetrisGameManager instance;

    private const int levelCap = 10;
    private const int rowBaseScore = 100;
    private const int extraRowScore = 200;

    private bool isGameOver;
    private int score;
    private int level;
    private int rowsCleared;

    [Header("Game Elements")]
    [SerializeField] private TetraminoController controller;
    [SerializeField] private TetraminoSpawner spawner;
    [SerializeField] private Board board;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
            StartGame();
    }

    private void StartGame()
    {
        score = 0;
        level = 0;
        rowsCleared = 0;
        SetBoard();
        SetController();
        SetSpawner();
        spawner.SpawnTetra();
        isGameOver = false;
    }

    private void SetSpawner()
    {
        spawner.ResetSpawner();
    }

    private void SetBoard()
    {
        board.ResetBoard();
    }

    private void SetController()
    {
        controller.ResetController();
        controller.gameObject.SetActive(true);
    }

    // public in game events
    public void GameOver()
    {
        controller.gameObject.SetActive(false);
        isGameOver = true;
    }

    public void TetraPlaced(Tetramino tetra)
    {
        // TODO stadistics
        spawner.SpawnTetra();
    }

    public void RowsCleared(int amount)
    {
        score += rowBaseScore + (amount - 1) * extraRowScore;
        rowsCleared += amount;
        if (rowsCleared >= levelCap)
        {
            level++;
            rowsCleared = 0;
        }
    }
}