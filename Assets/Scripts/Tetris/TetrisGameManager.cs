using System;
using UnityEngine;

public class TetrisGameManager : MonoBehaviour
{
    public static TetrisGameManager instance;

    private bool isGameOver;

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
        // TODO score thing and stadistics
        spawner.SpawnTetra();
    }
}
