using UnityEngine;

public class SmallBolita : MonoBehaviour
{
    public int ScorePoints { get; private set; } = 10;

    private void OnTriggerEnter2D(Collider2D collision)
        => GameManager.instance.PacmanEatsSmallBolita(this);
}
