using System.Collections.Generic;
using UnityEngine;

public enum TetraminoType { L, LInverted, Z, ZInverted, I, T, O }

public class TetraminoSpawner : MonoBehaviour
{
    public static TetraminoSpawner instance; 
    [SerializeField] private Board board;
    [SerializeField] private TetraminoController controller;

    [Header("Tetramino Data")]
    [SerializeField] private TetraminoData[] data;

    private List<Tetramino> tetraPool;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
        tetraPool = new();
    }

    private void GeneratePool()
    {
        Vector2Int spawnPoint = board.GetBoardDimensions();
        spawnPoint.x /= 2; // spawn in half
        foreach (var tetraData in data)
            tetraPool.Add(new Tetramino(tetraData, spawnPoint));
    }

    public void SpawnTetra()
    {
        if (tetraPool.Count == 0)
            GeneratePool();
        int index = Random.Range(0, tetraPool.Count);
        Tetramino newTetra = tetraPool[index];
        if (board.CanPlace(newTetra, newTetra.pos.x, newTetra.pos.y - 1))
        {
            controller.SetCurrentTetra(newTetra);
            tetraPool.RemoveAt(index);
        }
        else
            TetrisGameManager.instance.GameOver();
    }
}
