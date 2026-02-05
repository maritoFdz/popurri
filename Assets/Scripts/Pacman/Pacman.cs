using UnityEngine;

public class Pacman : MovingEntity
{
    public Animator anim;
    private bool canMove = true;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        HandleInput();
        if (!canMove) return;
        base.Update();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetDirection(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) SetDirection(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) SetDirection(Vector2.right);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) SetDirection(Vector2.up);
    }

    protected override void AlterSprite(Vector2 direction)
    {
        float dirAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = dirAngle;
    }

    public override void ResetEntity()
    {
        canMove = true;
        base.ResetEntity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            canMove = false;
            StartCoroutine(GameManager.instance.PacmanDies());
        }
    }
}
