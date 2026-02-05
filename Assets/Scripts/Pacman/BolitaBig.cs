using UnityEngine;

public class BolitaBig : MonoBehaviour
{
    public int ScorePoints { get; private set; } = 50;
    [SerializeField] private float powerTime;

    private void OnTriggerEnter2D(Collider2D collision)
        => GameManager.instance.PacmanEatsBigBolita(this);
}