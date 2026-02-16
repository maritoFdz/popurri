using UnityEngine;

public class Fruit : MonoBehaviour
{
    private FruitData data;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetFruit(FruitData data)
    {
        this.data = data;
        spriteRenderer.sprite = data.sprite;
    }

    public int GetScore()
    { 
        return data.score;
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
