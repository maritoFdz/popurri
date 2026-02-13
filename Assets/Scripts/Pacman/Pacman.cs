using UnityEngine;

public class Pacman : MovingEntity
{
    public Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        HandleInput();
        base.Update();
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetDirection(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) SetDirection(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) SetDirection(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) SetDirection(Vector2.up);
    }

    protected override void AlterSprite(Vector2 direction)
        => rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
}
