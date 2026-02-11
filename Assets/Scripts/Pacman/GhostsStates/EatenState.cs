using UnityEngine;

public class EatenState : IState
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

    }
}
