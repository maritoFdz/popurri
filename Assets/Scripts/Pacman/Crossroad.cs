using System.Collections.Generic;
using UnityEngine;

public class Crossroad : MonoBehaviour
{
    public List<Vector2> availableDir = new();
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
        if (!Application.isPlaying) return;

        DrawDirection(Vector2.up);
        DrawDirection(Vector2.down);
        DrawDirection(Vector2.left);
        DrawDirection(Vector2.right);
    }

    private void DrawDirection(Vector2 direction) // gracias por todo, Chatgpt
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            castDistance,
            wallsLayer
        );

        Vector3 endPoint;

        if (hit.collider != null)
        {
            Gizmos.color = Color.red;
            endPoint = hit.point;
        }
        else
        {
            Gizmos.color = Color.green;
            endPoint = transform.position + (Vector3)(direction * castDistance);
        }

        Gizmos.DrawLine(transform.position, endPoint);
        Gizmos.DrawSphere(endPoint, 0.05f);
    }
}
