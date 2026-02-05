using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    private int playerScore;
    private int aiScore;

    public void UpdatePlayerScore()
        => playerScoreText.text = (++playerScore).ToString();

    public void UpdateAiScore()
        => aiScoreText.text = (++aiScore).ToString();

    public void ResetScores()
    {
        playerScore = aiScore = -1;
        UpdateAiScore();
        UpdatePlayerScore();
    }
}
