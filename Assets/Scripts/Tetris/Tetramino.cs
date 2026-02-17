using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tetramino", menuName = "Scriptable Objects/Tetramino")]
public class Tetramino : ScriptableObject
{
    [SerializeField] private Tile tile;

    public Tile GetTile()
    {
        return tile;
    }
}
