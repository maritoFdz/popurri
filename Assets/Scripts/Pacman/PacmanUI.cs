using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PacmanUI : MonoBehaviour
{
    private const float flashTime = 0.5f;
    private const float distanceLifes = 0.48f;
    private readonly List<GameObject> currentLifes = new();

    public static PacmanUI instance;

    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private Vector3 pacmanLifesSpot;
    [SerializeField] private GameObject pacmanLife;

    private void Awake()
        => instance = this;

    public void UpdateScore(int score)
        => playerScore.text = score.ToString();

    public void UpdateLifes(int lifes)
    {
        foreach (GameObject life in currentLifes)
            Destroy(life);

        Vector3 pos = pacmanLifesSpot;
        for (int i = 0; i < lifes; i++)
        {
            GameObject life = Instantiate(pacmanLife, pos, Quaternion.identity);
            currentLifes.Add(life);
            pos.x += distanceLifes;
        }
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
