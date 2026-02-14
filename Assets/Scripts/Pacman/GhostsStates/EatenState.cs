using System.Collections;
using UnityEngine;

public class EatenState : IState
{
    bool hasTouchedDoor;

    public void EnterState(Ghost ghost)
    {
        ghost.targetTile = GameManager.instance.Door.transform.position;
        hasTouchedDoor = false;
        ghost.ghostBody.TurnOff();
        ghost.SetSpeedBost(1.2f);
    }

    public void Update(Ghost ghost)
    {
        return;
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DoorFront") && !hasTouchedDoor)
            ghost.StartCoroutine(DoorAnimCo(ghost));
    }

    private IEnumerator DoorAnimCo(Ghost ghost)
    {
        hasTouchedDoor = true;
        ghost.SetDirection(Vector2.down, true);
        yield return new WaitForSeconds(0.1f);
        ghost.ghostBody.TurnOn();
        ghost.SetDirection(Vector2.up, true);
        yield return new WaitForSeconds(0.1f);
        ghost.SetDirection(Vector2.right);
        ghost.SwitchState(ghost.scatterState);
    }
}
