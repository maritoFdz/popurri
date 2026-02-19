using TMPro;
using UnityEngine;

public class TetrisUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI linesText;
    private int score;
    private int level;
    private int lines;

    public void SetLines(int lines)
    {
        this.lines = lines;
        linesText.text = this.lines.ToString(); 
    }

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = this.score.ToString();
    }

    public void SetLevel(int level)
    {
        this.level = level;
        levelText.text = this.level.ToString();
    }

    public void ResetAll()
    {
        score = 0;
        level = 1;
        lines = 0;
    }
}
