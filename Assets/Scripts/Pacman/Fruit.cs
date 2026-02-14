using UnityEngine;

public class Fruit : MonoBehaviour
{
    private int score;
    private SpriteRenderer spriteRenderer;

    private void Awake()
        => spriteRenderer = GetComponent<SpriteRenderer>();

    public void SetFruit(int score, Sprite sprite)
    {
        this.score = score;
        spriteRenderer.sprite = sprite;
    }

    public int GetScore()
    { 
        return score;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            gameObject.SetActive(false);
            GameManager.instance.PacmanEatsFruit(this);
        }
    }
}
