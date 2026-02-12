using UnityEngine;

public class FrightenedState : IState
{
    private const float timeToFrightened = 20f;
    private float time;

    public void EnterState(Ghost ghost)
    {
        ghost.ghostBody.SetFrightenedAnim();
        time = 0f;
        ghost.speedBoost = 1f;
    }

    public void Update(Ghost ghost)
    {

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
