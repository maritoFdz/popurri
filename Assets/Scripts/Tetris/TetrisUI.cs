using System;
using TMPro;
using UnityEngine;

public class TetrisUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI linesText;

    [Header("Statistics")]
    [SerializeField] private TextMeshProUGUI TTetraAmount;
    [SerializeField] private TextMeshProUGUI LInvTetraAmount;
    [SerializeField] private TextMeshProUGUI ZTetraAmount;
    [SerializeField] private TextMeshProUGUI OTetraAmount;
    [SerializeField] private TextMeshProUGUI ZInvTetraAmount;
    [SerializeField] private TextMeshProUGUI LTetraAmount;
    [SerializeField] private TextMeshProUGUI ITetraAmount;
    [SerializeField] private SpriteRenderer nextTetra;

    public void Awake()
    {
        nextTetra.sprite = null;
    }

    public void SetLines(int lines)
    {
        linesText.text = lines.ToString(); 
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetLevel(int level)
    {
        levelText.text = level.ToString();
    }

    public void SetStatistics(int t, int lInv, int z, int o, int zInv, int l, int i)
    {
        TTetraAmount.text = t.ToString();
        LInvTetraAmount.text = lInv.ToString();
        ZTetraAmount.text = z.ToString();
        OTetraAmount.text = o.ToString();
        ZInvTetraAmount.text = zInv.ToString();
        LTetraAmount.text = l.ToString();
        ITetraAmount.text = i.ToString();
    }

    public void SetNextTetra(Sprite nextTetra)
    {
        this.nextTetra.sprite = nextTetra;
    }
}
