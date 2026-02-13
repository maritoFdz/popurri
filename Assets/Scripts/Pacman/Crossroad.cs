using System.Collections.Generic;
using UnityEngine;

public class Crossroad : MonoBehaviour
{
    public List<Vector2> availableDir = new();

    [Header("Walls Detection")]
    [SerializeField] private float boxSizeFactor;
    [SerializeField] private LayerMask wallsLayer;
    [SerializeField] private float castDistance;
    private Vector2 boxSize;

    private void Start()
        => SetAvailableDir();

    private void SetAvailableDir()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        for (int i = 0; i < directions.Length; i++)
            if (IsDirAvailable(directions[i])) availableDir.Add(directions[i]);
    }

    private bool IsDirAvailable(Vector2 direction)
    {
        RaycastHit2D inFront = Physics2D.Raycast(transform.position, direction, castDistance, wallsLayer);
        return inFront.collider == null;
    }

    private void OnDrawGizmos()
    {
        DrawDirection(Vector2.up);
        DrawDirection(Vector2.down);
        DrawDirection(Vector2.left);
        DrawDirection(Vector2.right);
    }

    private void DrawDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, castDistance, wallsLayer);
        Gizmos.color = (hit.collider != null) ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3) (castDistance * direction));
    }
}
