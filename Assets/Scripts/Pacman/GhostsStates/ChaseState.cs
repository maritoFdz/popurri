using UnityEngine;

public class ChaseState : IState
{
    private const float clydeDistance = 3f;
    private const float pinkyDistance = 1.5f;
    private const float timeToChase = 25f;
    private const int maxStateChanges = 4;
    private float time;
    public int stateChanges;
    private Vector3 blinkyPos;
    private Vector3 pacmanPos;
    private Vector2 pacmanDir;

    public void EnterState(Ghost ghost)
    {
        ghost.ghostBody.SetChaseAnim();
        stateChanges++;
        ghost.SetSpeedBost(1f);
        ghost.SetDirection(-1 * ghost.direction);
        time = 0f;
    }

    public void Update(Ghost ghost)
    {
        blinkyPos = GameManager.instance.GetBlinkyPos();
        pacmanPos = GameManager.instance.GetPacmanPos();
        pacmanDir = GameManager.instance.GetPacmanDir();
        switch (ghost.type)
        {
            case GhostType.Blinky:
                ghost.targetTile = pacmanPos;
                break;
            case GhostType.Pinky:
                ghost.targetTile = pacmanPos + (Vector3) (pinkyDistance * pacmanDir);
                break;
            case GhostType.Inky:
                Vector3 pacmanFront = pacmanPos + (Vector3) pacmanDir;
                ghost.targetTile = 2 * pacmanFront - blinkyPos;
                break;
            case GhostType.Clyde:
                if (Vector3.Distance(pacmanPos, ghost.transform.position) >= clydeDistance)
                    ghost.targetTile = pacmanPos;
                else ghost.targetTile = GameManager.instance.BottomLeft.transform.position;
                break;
        }

        time += Time.deltaTime;
        if (stateChanges <= maxStateChanges && time >= timeToChase)
            ghost.SwitchState(ghost.scatterState);
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D collision)
    {
        if (collision.GetComponent<Pacman>() != null)
            GameManager.instance.KillPacman();
    }
}
