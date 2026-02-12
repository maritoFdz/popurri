using UnityEngine;

public class ScatterState : IState
{
    private const float timeToScatter = 5f;
    private float time;

    public void EnterState(Ghost ghost)
    {
        time = 0f;
        ghost.speedBoost = 1f;
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

    public void Update(Ghost ghost)
    {
        time += Time.deltaTime;
        if (time >= timeToScatter)
            ghost.SwitchState(ghost.chaseState);
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Crossroad>(out var cross))
        {
            ghost.posDirections = cross.availableDir;
            ghost.canChangeDir = true;
        }
    }
}
