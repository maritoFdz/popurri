using UnityEngine;

public class Pellet : MonoBehaviour
{
    [SerializeField] private int pelletPoints;
    [SerializeField] private int powerPelletPoints;
    public bool isPowerPellet;

    public int GetPoints()
    {
        return (isPowerPellet) ? powerPelletPoints : pelletPoints;
    }

    private void OnTriggerEnter2D(Collider2D collision)
        => GameManager.instance.PacmanEatsPellet(this);
}
