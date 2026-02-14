using TMPro;
using UnityEngine;

public class PacmanUI : MonoBehaviour
{
    public static PacmanUI instance;

    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private SpriteRenderer[] pacmanLifes;

    private void Awake()
        => instance = this;

    public void UpdateScore(int score)
        => playerScore.text = score.ToString();

    public void UpdateLifes(int lifes)
    {
        for (int i = 0; i < lifes; i++)
            pacmanLifes[i].gameObject.SetActive(true);
        for (int i = lifes; i < pacmanLifes.Length; i++)
            pacmanLifes[i].gameObject.SetActive(false);
    }
}
