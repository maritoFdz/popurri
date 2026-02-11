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

    public List<Vector2> posDirections;
    public Vector3 targetTile;
    public bool canChangeDir;
    private IState currentState;
    [Header("Parameters")]
    [SerializeField] private int ScorePoints;
    [SerializeField] private GhostEyesAnimator ghostEyes;
    public GhostType type;

    protected override void Awake()
    {
        currentState = homeState;
        currentState.EnterState(this);
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
        if (newDir != Vector2.zero)
        {
            SetDirection(newDir);
            canChangeDir = false;
        }
    }

    private Vector2 ChoseDirection()
    {
        Vector2 bestDirection = new();
        float distance = float.MaxValue;
        foreach (Vector2 posDirection in posDirections)
        {
            if (posDirection == -1 * direction) continue;

            float distanceInDir = Vector3.Distance(transform.position + (Vector3) posDirection, targetTile);
            if (distanceInDir < distance)
            {
                distance = distanceInDir;
                bestDirection = posDirection;
            }
        }
        return bestDirection;
    }

    public void SwitchState(IState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    protected override void AlterSprite(Vector2 direction)
        => ghostEyes.LookInDirection(direction);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnColission2DEnter(this, collision);
    }
}
