using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyesAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private Sprite EyesDown;
    [SerializeField] private Sprite EyesUp;
    [SerializeField] private Sprite EyesRight;
    [SerializeField] private Sprite EyesLeft;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    public void LookInDirection(Vector2 direction)
    {
        if (direction == Vector2.down)
            spriteRender.sprite = EyesDown;
        else if (direction == Vector2.up)
            spriteRender.sprite = EyesUp;
        else if (direction == Vector2.right)
            spriteRender.sprite = EyesRight;
        else if (direction == Vector2.left)
            spriteRender.sprite = EyesLeft;
    }
}
