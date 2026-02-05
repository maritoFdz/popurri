using System.Collections.Generic;
using UnityEngine;

public class Ghost : MovingEntity
{
    private const float scatterTimeChange = 2f;
    private List<Vector2> posDirections;
    protected Vector3 targetTile;
    private bool canChangeDir;
    [Header("Parameters")]
    [SerializeField] private int ScorePoints;
    [SerializeField] private GhostEyesAnimator ghostEyes;
    [SerializeField] private float scatterTime;

    protected override void Awake()
    {
        base.Awake();
        ghostEyes = GetComponentInChildren<GhostEyesAnimator>();
    }

    protected override void Update()
    {
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
    }

    public void ChangeScatterTime()
        => scatterTime -= scatterTimeChange;
}
