using UnityEngine;

public class Ai : Paddle
{
    private Ball ball;
    [Header("AI Dificulty")]
    [SerializeField] private float sensibility;
    [SerializeField] private float minDistance;

    protected override void Awake()
    {
        base.Awake();
        ball = FindFirstObjectByType<Ball>();
    }

    protected override void HandleMovement()
    {
        float dir = 0;
        if (Mathf.Abs(ball.transform.position.x - transform.position.x) > minDistance || ball.rb.linearVelocity.x < 0)
            return;
        if (ball.transform.position.y - sensibility > transform.position.y)
            dir = 1;
        else if (ball.transform.position.y + sensibility < transform.position.y)
            dir = -1;
        Move (dir);
    }
}
