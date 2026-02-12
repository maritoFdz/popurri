using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class MovingEntity : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private float speed;
    public float speedBoost = 1f;
    public Vector2 direction;
    protected Vector2 nextDirection;

    [Header("Walls Detection")]
    [SerializeField] private LayerMask wallsLayer;
    [SerializeField] private float castDistance;
    [SerializeField] private float boxSizeFactor;
    protected Vector3 startPos;
    private Vector2 boxSize;
    protected readonly Vector2 initialDirection = Vector2.right;

    protected virtual void Awake()
    {
        startPos = transform.position;
        boxSize = Vector2.one * boxSizeFactor;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetEntity();
    }

    public virtual void ResetEntity()
    {
        speedBoost = 1f;
        direction = initialDirection;
        AlterSprite(direction);
        nextDirection = Vector2.zero;
        transform.position = startPos;
    }

    protected virtual void Update()
    {
        if (nextDirection != Vector2.zero) SetDirection(nextDirection);
        HandleMovement();
    }


    private void HandleMovement()
    {
        rb.MovePosition(rb.position + speed * speedBoost * Time.deltaTime * direction);
    }

    public void SetDirection(Vector2 newDirection)
    {

        if (CanMove(newDirection))
        {
            direction = newDirection;
            AlterSprite(direction);
            nextDirection = Vector2.zero;
        }
        else nextDirection = newDirection;
    }

    protected abstract void AlterSprite(Vector2 direction);

    private bool CanMove(Vector2 direction)
    {
        RaycastHit2D inFront = Physics2D.BoxCast(rb.position, boxSize, 0f, direction, castDistance, wallsLayer);
        return inFront.collider == null;
    }

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
