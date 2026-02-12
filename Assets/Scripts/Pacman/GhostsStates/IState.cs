using UnityEngine;

public interface IState
{
    public void EnterState(Ghost ghost);
    public void Update(Ghost ghost);
    public void OnColission2DEnter(Ghost ghost, Collider2D other);
}
