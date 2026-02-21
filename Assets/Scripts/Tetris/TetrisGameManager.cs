using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    public static TetrisGameManager instance;

    private const int levelCap = 10;
    private const int rowBaseScore = 100;
    private const int extraRowScore = 200;
    private const string tetrisHighScoreKey = "TetrisHighScore";

    private bool isGameOver;
    private int highestScore;
    private int score;
    private int level;
    private int totalRowsCleared;
    private int rowsCleared;
    private int TTetraAmount;
    private int LInvTetraAmount;
    private int ZTetraAmount;
    private int OTetraAmount;
    private int ZInvTetraAmount;
    private int LTetraAmount;
    private int ITetraAmount;
    private Tetramino nextTetra;

    [Header("Game Elements")]
    [SerializeField] private TetrisUI ui;
    [SerializeField] private TetrisAudioManager audioManager;
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
        highestScore = PlayerPrefs.GetInt(tetrisHighScoreKey, 0);
        ui.SetHighScore(highestScore);
        StartGame();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
            StartGame();
    }

    private void StartGame()
    {
        audioManager.PlayMusic();
        SetStartingValues();
        SetBoard();
        SetController();
        SetSpawner();
        spawner.SpawnTetra();
    }

    private void SetStartingValues()
    {
        score = 0;
        level = 0;
        totalRowsCleared = 0;
        TTetraAmount = 0;
        LInvTetraAmount = 0;
        ZTetraAmount = 0;
        OTetraAmount = 0;
        ZInvTetraAmount = 0;
        LTetraAmount = 0;
        ITetraAmount = 0;
        rowsCleared = 0;
        isGameOver = false;
        nextTetra = null;
        ui.SetScore(score);
        ui.SetLevel(level);
        ui.SetLines(totalRowsCleared);
        ui.SetStatistics(TTetraAmount, LInvTetraAmount, ZTetraAmount, OTetraAmount, ZInvTetraAmount, LTetraAmount, ITetraAmount);
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

    private void AddStatistics(TetraminoType type)
    {
        switch (type)
        {
            case TetraminoType.T:
                TTetraAmount++;
                break;
            case TetraminoType.LInverted:
                LInvTetraAmount++;
                break;
            case TetraminoType.Z:
                ZTetraAmount++;
                break;
            case TetraminoType.O:
                OTetraAmount++;
                break;
            case TetraminoType.ZInverted:
                ZInvTetraAmount++;
                break;
            case TetraminoType.L:
                LTetraAmount++;
                break;
            case TetraminoType.I:
                ITetraAmount++;
                break;
        }
        ui.SetStatistics(TTetraAmount, LInvTetraAmount, ZTetraAmount, OTetraAmount, ZInvTetraAmount, LTetraAmount, ITetraAmount);
    }

    // public in game events
    public void GameOver()
    {
        audioManager.StopPlayingMusic();
        audioManager.PlaySound(TetrisSoundType.gameOver);
        controller.gameObject.SetActive(false);
        isGameOver = true;
        if (score > highestScore)
        {
            highestScore = score;
            PlayerPrefs.SetInt(tetrisHighScoreKey, highestScore);
            PlayerPrefs.Save();
        }
        ui.SetHighScore(highestScore);
    }

    public void TetraPlaced(Tetramino tetra)
    {
        AddStatistics(tetra.data.type);
        audioManager.PlaySound(TetrisSoundType.tetraPlaced);
    }

    public void RowsCleared(List<int> rowsClearedIndex)
    {
        if (rowsClearedIndex.Count == 0)
            spawner.SpawnTetra();
        else
            StartCoroutine(RowsClearedCo(rowsClearedIndex));
    }

    private IEnumerator RowsClearedCo(List<int> rowsClearedIndex)
    {
        int amount = rowsClearedIndex.Count;
        score += rowBaseScore + (amount - 1) * extraRowScore;
        rowsCleared += amount;
        totalRowsCleared += amount;
        audioManager.PlayRowsSound(amount);

        controller.enabled = false;

        yield return StartCoroutine(board.ClearRowsCo(rowsClearedIndex));

        if (rowsCleared >= levelCap)
        {
            audioManager.PlaySound(TetrisSoundType.levelUp);
            level++;
            rowsCleared = 0;
        }

        ui.SetScore(score);
        ui.SetLevel(level);
        ui.SetLines(totalRowsCleared);

        spawner.SpawnTetra();
        controller.enabled = true;
    }

    public void SetNextTetra(Tetramino tetra)
    {
        this.nextTetra = tetra;
        ui.SetNextTetra(tetra.data.ShapeVisuals);
    }
}