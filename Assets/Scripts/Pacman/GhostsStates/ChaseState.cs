using UnityEngine;

public class ChaseState : IState
{
    private const float clydeDistance = 4f;
    private const float timeToChase = 20f;
    private const int maxStateChanges = 4;
    private float time;
    private int stateChanges;
    private Pacman pacman;
    private Ghost blinky;

    public void EnterState(Ghost ghost)
    {
        ghost.ghostBody.SetChaseAnim();
        pacman = GameManager.instance.pacman;
        blinky = GameManager.instance.Blinky;
        stateChanges++;
        ghost.speedBoost = 1f;
        ghost.direction = -1 * ghost.direction;
        time = 0f;
    }

    public void Update(Ghost ghost)
    {
        Vector3 pacmanPos = pacman.transform.position;
        switch (ghost.type)
        {
            case GhostType.Blinky:
                ghost.targetTile = pacmanPos;
                break;
            case GhostType.Pinky:
                ghost.targetTile = pacmanPos + (Vector3) (2 * pacman.direction);
                break;
            case GhostType.Inky:
                Vector3 blinkyPos = blinky.transform.position; // A
                Vector3 pacmanFront = pacmanPos + (Vector3) pacman.direction; // B
                ghost.targetTile = 2 * pacmanFront - blinkyPos;
                break;
            case GhostType.Clyde:
                if (Vector3.Distance(pacmanPos, ghost.transform.position) >= clydeDistance)
                    ghost.targetTile = pacmanPos;
                else
                    ghost.targetTile = GameManager.instance.BottomLeft.transform.position;
                break;
        }

        time += Time.deltaTime;
        if (stateChanges <= maxStateChanges && time >= timeToChase)
            ghost.SwitchState(ghost.scatterState);
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
