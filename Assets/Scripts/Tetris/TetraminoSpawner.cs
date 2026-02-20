using System.Collections.Generic;
using UnityEngine;

public enum TetraminoType { L, LInverted, Z, ZInverted, I, T, O }

public class TetraminoSpawner : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private TetraminoController controller;

    [Header("Tetramino Data")]
    [SerializeField] private TetraminoData[] data;

    private List<Tetramino> tetraPool;
    private Tetramino nextTetra;

    private void Awake()
    {
        tetraPool = new();
    }

    public void ResetSpawner()
    {
        tetraPool.Clear();
    }

    private void GeneratePool()
    {
        Vector2Int spawnPoint = board.GetBoardDimensions();
        spawnPoint.x /= 2; // spawn in middle
        foreach (var tetraData in data)
            tetraPool.Add(new Tetramino(tetraData, spawnPoint));
    }

    public void SpawnTetra()
    {
        if (tetraPool.Count == 0) GeneratePool();

        // if there is no tetra created
        if (nextTetra == null)
        {
            nextTetra = GetRandomTetra();
            if (tetraPool.Count == 0) GeneratePool();
        }

        Tetramino newTetra = nextTetra;
        nextTetra = GetRandomTetra();
        TetrisGameManager.instance.SetNextTetra(nextTetra);
        if (board.CanPlace(newTetra, newTetra.pos.x, newTetra.pos.y - 1))
            controller.SetCurrentTetra(newTetra);
        else TetrisGameManager.instance.GameOver();
    }

    private Tetramino GetRandomTetra()
    {
        int index = Random.Range(0, tetraPool.Count);
        Tetramino newTetra = tetraPool[index];
        tetraPool.RemoveAt(index);
        return newTetra;
    }
}
