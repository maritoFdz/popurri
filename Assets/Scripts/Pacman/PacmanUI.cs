using System.Collections;
using TMPro;
using UnityEngine;

public class PacmanUI : MonoBehaviour
{
    private const float flashTime = 0.5f;
    public static PacmanUI instance;

    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
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

    public void UpdateLevel(int level)
    {
        StopCoroutine(FlashTextCo(playerLevelText));
        playerLevelText.text = level.ToString() + "UP";
        StartCoroutine(FlashTextCo(playerLevelText));
    }

    public void ShowReadyText(bool show)
        => readyText.gameObject.SetActive(show);

    public void ShowRestartText(bool show)
        => restartText.gameObject.SetActive(show);

    public IEnumerator FlashTextCo(TextMeshProUGUI text) 
    {
        while (true)
        {
            text.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashTime);
            text.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashTime);
        }
    }
}
