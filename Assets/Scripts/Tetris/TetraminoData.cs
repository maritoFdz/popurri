using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tetramino Data", menuName = "Scriptable Objects/Tetramino Data")]
public class TetraminoData : ScriptableObject
{
    public Tile Tile;
    public Vector2Int[] TetraminoShape; // (0,0) will always be the shape pivot for rotation
}
