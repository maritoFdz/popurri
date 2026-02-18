using System;
using UnityEngine;

public class Tetramino : MonoBehaviour
{
    public TetraminoData data;
    private Vector2Int[] rotation;

    private void Awake()
    {
        rotation = new Vector2Int[data.TetraminoShape.Length];
        Array.Copy(data.TetraminoShape, rotation, data.TetraminoShape.Length);
    }

    public void Rotate() // always right to left rotation
    {
        for (int i = 0; i < rotation.Length; i++)
        {
            int x = rotation[i].x;
            int y = rotation[i].y;
            rotation[i] = new Vector2Int(-y, x);
        }
    }
}
