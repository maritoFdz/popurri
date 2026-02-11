using System.Collections.Generic;
using UnityEngine;

public enum GhostType { Blinky, Pinky, Inky, Clyde};

public class Ghost : MovingEntity
{
    // states
    public OnHomeState homeState = new();
    public EatenState eatenState = new();
    public FrightenedState frightenedState = new();
    public ScatterState scatterState = new();
    public ChaseState chaseState = new();

    private List<Vector2> posDirections;
    protected Vector3 targetTile;
    private bool canChangeDir;
    private IState currentState;
    [Header("Parameters")]
    [SerializeField] private int ScorePoints;
    [SerializeField] private GhostEyesAnimator ghostEyes;
    public readonly GhostType type;

    protected override void Awake()
    {

        currentState = homeState;
        base.Awake();
        ghostEyes = GetComponentInChildren<GhostEyesAnimator>();
    }

    protected override void Update()
    {
        currentState.Update(this);
        if (canChangeDir && targetTile != null)
            HandleInput();
        base.Update();
    }

    public int GetPoints()
    {
        return ScorePoints;
    }

    private void HandleInput()
    {
        Vector2 newDir = ChoseDirection();
        if (newDir != -1 * direction)
        {
            canChangeDir = false;
            SetDirection(newDir);
        }
    }

    private Vector2 ChoseDirection()
    {
        Vector2 bestDirection = new();
        float distance = float.MaxValue;
        foreach (Vector2 direction in posDirections)
        {
            float distanceInDir = Vector3.Distance(transform.position + (Vector3) direction, targetTile);
            if (distanceInDir < distance)
            {
                distance = distanceInDir;
                bestDirection = direction;
            }
        }
        return bestDirection;
    }

    protected override void AlterSprite(Vector2 direction)
        => ghostEyes.LookInDirection(direction);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Crossroad>(out var cross))
        {
            posDirections = cross.availableDir;
            canChangeDir = true;
        }
        else
            currentState.OnColission2DEnter(this, collision);
    }
}
