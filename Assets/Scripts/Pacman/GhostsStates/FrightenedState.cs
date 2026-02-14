using UnityEngine;

public class FrightenedState : IState
{
    private const float timeToFrightened = 7f;
    private float time;

    public void EnterState(Ghost ghost)
    {
        ghost.SetDirection(-1 * ghost.direction);
        ghost.targetTile = GameManager.instance.Door.transform.position;
        ghost.ghostBody.SetFrightenedAnim();
        time = 0f;
        ghost.SetSpeedBost(1f);
    }

    public void Update(Ghost ghost)
    {
        if (!ghost.audioSource.isPlaying)
            ghost.audioSource.PlayOneShot(ghost.frightenedClip);
        time += Time.deltaTime;
        if (time >= timeToFrightened)
        {
            ghost.audioSource.Stop();
            ghost.SwitchState(ghost.scatterState);
        }
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D collision)
    {
        if (collision.GetComponent<Pacman>() != null)
        {
            GameManager.instance.PacmanEatsGhost(ghost);
            ghost.SwitchState(ghost.eatenState);
        }
    }
}
