using UnityEngine;

public class EatenState : IState
{
    public void EnterState(Ghost ghost)
    {
        ghost.ghostBody.TurnOn();
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
        else if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            ghost.ghostBody.TurnOn();
            ghost.SwitchState(ghost.homeState);
        }
    }
}
