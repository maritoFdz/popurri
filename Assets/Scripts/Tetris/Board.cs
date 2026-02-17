using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    private Transform[,] grid;

    void Awake()
    {
        grid = new Transform[width, height];
    }
}
