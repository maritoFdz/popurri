using UnityEngine;

public class Player : Paddle
{
    protected override void HandleMovement()
    {
        float dir = 0;
        if (Input.GetKey(KeyCode.DownArrow))
            dir = -1;
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = 1;
        Move(dir);
    }
}
