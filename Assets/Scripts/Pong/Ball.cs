using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball ball;
    [SerializeField] private float speed;
    [SerializeField] float cooldown;
    public Rigidbody2D rb { get; private set; }
    private GameUI ui;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ui = FindFirstObjectByType<GameUI>();
    }

    private void Start()
    {
        Vector2 dir = (Vector2.left + SelectVertical()).normalized; // primer tiro
        Throw(dir * speed);
    }

    private void Throw(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    private void ResetBall(bool scoredPlayer)
    {
        if (scoredPlayer) ui.UpdatePlayerScore();
        else ui.UpdateAiScore();
            StartCoroutine(ResetCo(scoredPlayer));
    }

    private IEnumerator ResetCo(bool scoredOnRight)
    {
        rb.linearVelocity = transform.position = Vector2.zero;
        yield return new WaitForSeconds(cooldown);

        Vector2 dir = SelectVertical();
        dir += scoredOnRight ? Vector2.left : Vector2.right;
        Throw(dir.normalized * speed);
    }

    private Vector2 SelectVertical()
        => Random.value < 0.5f ? Vector2.up : Vector2.down;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ScoreZone scoreZone))
        {
            scoreZone.MakeSound();
            ResetBall(scoreZone.name == "RightScore");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Paddle paddle))
            paddle.MakeSound();
        else if (collision.collider.TryGetComponent(out Wall wall))
            wall.MakeSound();
    }
}
