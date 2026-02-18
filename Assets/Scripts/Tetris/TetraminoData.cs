using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tetramino Data", menuName = "Scriptable Objects/Tetramino Data")]
public class TetraminoData : ScriptableObject
{
    public Tile Tile { get; private set; }
    public Vector2Int[] TetraminoShape { get; private set; } // (0,0) will always be the shape pivot for rotation
}
