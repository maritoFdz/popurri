using System;
using UnityEngine;

public class Tetramino
{
    public TetraminoData data;
    public Vector2Int[] Rotation { get; private set; }
    public Vector2Int pos;

    public Tetramino(TetraminoData newData, Vector2Int pos)
    {
        data = newData;
        this.pos = pos;
        Rotation = new Vector2Int[data.TetraminoShape.Length];
        Array.Copy(data.TetraminoShape, Rotation, data.TetraminoShape.Length);
    }

    public void Rotate() // always right to left rotation
    {
        for (int i = 0; i < Rotation.Length; i++)
        {
            int x = Rotation[i].x;
            int y = Rotation[i].y;
            Rotation[i] = new Vector2Int(-y, x);
        }
    }
}
