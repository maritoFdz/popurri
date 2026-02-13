using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovingEntity : MonoBehaviour
{
    private float speedBoost;
    public Vector2 direction;
    protected Vector2 nextDirection;
    protected Vector3 startPos;
    private Vector2 boxSize;
    protected Vector2 initialDirection;
    protected bool canMove;

    [Header("Movement Settings")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private float speed;

    [Header("Walls Detection")]
    [SerializeField] private LayerMask wallsLayer;
    [SerializeField] private float castDistance;
    [SerializeField] private float boxSizeFactor;

    protected abstract void HandleInput();
    protected abstract void AlterSprite(Vector2 direction);

    protected virtual void Awake()
    {
        initialDirection = Vector2.right;
        startPos = transform.position;
        boxSize = Vector2.one * boxSizeFactor;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
        => ResetEntity();

    protected virtual void Update()
    {
        if (!canMove) return;
        if (nextDirection != Vector2.zero) SetDirection(nextDirection);
        HandleMovement();
    }

    public virtual void ResetEntity()
    {
        Unfreeze();
        speedBoost = 1f;
        direction = initialDirection;
        AlterSprite(direction);
        nextDirection = Vector2.zero;
        transform.position = startPos;
    }

    private void HandleMovement()
        => rb.MovePosition(rb.position + speed * speedBoost * Time.deltaTime * direction);

    private bool CanTakeDir(Vector2 direction)
    {
        RaycastHit2D inFront = Physics2D.BoxCast(rb.position, boxSize, 0f, direction, castDistance, wallsLayer);
        return inFront.collider == null;
    }

    public void SetDirection(Vector2 newDirection, bool isForced = false)
    {
        if (CanTakeDir(newDirection) || isForced)
        {
            direction = newDirection;
            AlterSprite(direction);
            nextDirection = Vector2.zero;
        }
        else nextDirection = newDirection;
    }

    public void SetSpeedBost(float speedBost)
    => this.speedBoost = speedBost;

    public void Freeze()
        => canMove = false;

    public void Unfreeze()
        => canMove = true;

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position;
        Vector3 end = origin + (Vector3)(direction * castDistance);
        Gizmos.DrawWireCube(origin, boxSize);
        Gizmos.DrawWireCube(end, boxSize);
        Gizmos.DrawLine(origin, end);
    }
}
