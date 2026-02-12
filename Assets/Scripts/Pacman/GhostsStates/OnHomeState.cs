using UnityEngine;

public class OnHomeState : IState
{
    public void EnterState(Ghost ghost)
    {
        ghost.speedBoost = 0.01f;
    }

    public void Update(Ghost ghost)
    {
        ghost.targetTile = GameManager.instance.Door.transform.position;
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crossroads"))
            ghost.SetDirection(Vector2.up);
        else
            ghost.SwitchState(ghost.scatterState);
    }
}
