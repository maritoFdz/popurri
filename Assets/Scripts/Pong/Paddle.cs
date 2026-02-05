using UnityEngine;

public abstract class Paddle : SoundEmitter
{
    [SerializeField] protected float speed;
    protected float minY;
    protected float maxY;
    protected Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        float cameraHeight = Camera.main.orthographicSize;
        float paddleCenter = GetComponent<SpriteRenderer>().bounds.extents.y;
        minY = -cameraHeight + paddleCenter;
        maxY = cameraHeight - paddleCenter;
    }

    protected void Update()
    {
        HandleMovement();
    }

    protected abstract void HandleMovement();

    protected void Move(float dir)
    {
        Vector2 direction = dir * speed * Time.deltaTime * Vector2.up + rb.position;
        direction.y = Mathf.Clamp(direction.y, minY, maxY);
        rb.MovePosition(direction);
    }
}
