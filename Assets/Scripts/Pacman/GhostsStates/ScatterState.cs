using UnityEngine;

public class ScatterState : IState
{
    public void EnterState(Ghost ghost)
    {
        ghost.speedBoost = 1f;
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Crossroad>(out var cross))
        {
            ghost.posDirections = cross.availableDir;
            ghost.canChangeDir = true;
        }
    }

    public void Update(Ghost ghost)
    {
        switch (ghost.type)
        {
            case GhostType.Blinky:
                ghost.targetTile = GameManager.instance.TopRight.transform.position;
                break;
            case GhostType.Pinky:
                ghost.targetTile = GameManager.instance.TopLeft.transform.position;
                break;
            case GhostType.Inky:
                ghost.targetTile = GameManager.instance.BottomRight.transform.position;
                break;
            case GhostType.Clyde:
                ghost.targetTile = GameManager.instance.BottomLeft.transform.position;
                break;
        }
    }
}
