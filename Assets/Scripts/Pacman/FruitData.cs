using UnityEngine;

[CreateAssetMenu(fileName = "New Fruit Data", menuName = "Scriptable Objects/Fruit Data")]
public class FruitData : ScriptableObject
{
    public Sprite sprite;
    public int score;
}
