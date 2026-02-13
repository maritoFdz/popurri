using System.Collections;
using UnityEngine;

public class EatenState : IState
{
    bool hasTouchedDoor;

    public void EnterState(Ghost ghost)
    {
        hasTouchedDoor = false;
        ghost.ghostBody.TurnOff();
        ghost.speedBoost = 1.2f;
    }

    public void Update(Ghost ghost)
    {
        ghost.targetTile = GameManager.instance.Door.transform.position;
    }

    public void OnColission2DEnter(Ghost ghost, Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Crossroad>(out var cross))
        {
            ghost.posDirections = cross.availableDir;
            ghost.canChangeDir = true;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("DoorFront") && !hasTouchedDoor)
            ghost.StartCoroutine(DoorAnimCo(ghost));
    }

    private IEnumerator DoorAnimCo(Ghost ghost)
    {
        hasTouchedDoor = true;
        ghost.direction = Vector2.down;
        yield return new WaitForSeconds(0.1f);
        ghost.ghostBody.TurnOn();
        ghost.direction = Vector2.up;
        yield return new WaitForSeconds(0.1f);
        ghost.SetDirection(Vector2.right);
        ghost.SwitchState(ghost.scatterState);
    }
}
