using UnityEngine;

public class OnHomeState : IState
{
    public void EnterState(Ghost ghost)
    {
        ghost.ghostBody.TurnOn();
        ghost.ghostBody.SetChaseAnim();
        ghost.SetSpeedBost(0.00001f);
    }

    public void Update(Ghost ghost)
        => ghost.targetTile = GameManager.instance.Door.transform.position;

    public void OnColission2DEnter(Ghost ghost, Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Crossroads"))
            ghost.SetDirection(Vector2.up);
        else
            ghost.SwitchState(ghost.scatterState);
    }
}