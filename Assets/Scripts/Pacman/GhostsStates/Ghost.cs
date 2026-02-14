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

    // properties
    private List<Vector2> posDirections;
    public Vector3 targetTile;
    public bool canChangeDir;
    private IState currentState;

    [Header("Score")]
    public int ScorePoints;

    [Header("Animations")]
    [SerializeField] private GhostEyesAnimator ghostEyes;
    public GhostBodyAnimator ghostBody;
    public GhostType type;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip frightenedClip;

    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentState = homeState;
        currentState.EnterState(this);
        ghostEyes = GetComponentInChildren<GhostEyesAnimator>();
        ghostBody = GetComponentInChildren<GhostBodyAnimator>();
        base.Awake();
    }

    protected override void Update()
    {
        currentState.Update(this);
        if (canChangeDir && targetTile != null)
            HandleInput();
        base.Update();
    }

    protected override void HandleInput()
    {
        Vector2 newDir = ChoseDirection();
        if (newDir != Vector2.zero)
        {
            SetDirection(newDir);
            canChangeDir = false;
        }
    }

    protected override void AlterSprite(Vector2 direction)
        => ghostEyes.LookInDirection(direction);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Crossroad>(out var cross))
        {
            posDirections = cross.availableDir;
            canChangeDir = true;
        }
        currentState.OnColission2DEnter(this, collision);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = ghostBody.GetColor();
        Gizmos.DrawWireSphere(targetTile, 0.2f);
        Gizmos.DrawLine(transform.position, targetTile);
    }
}
