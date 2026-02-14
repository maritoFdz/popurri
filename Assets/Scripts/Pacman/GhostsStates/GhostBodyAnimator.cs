using System.Collections;
using UnityEngine;

public class GhostBodyAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private float animSpeed;
    private Coroutine currentAnim;

    [Header("Idle Sprites")]
    [SerializeField] private Color ghostColor;
    [SerializeField] private Sprite[] Idle;

    [Header("Frightened Sprites")]
    [SerializeField] private Sprite[] Frightened;

    private void Awake()
        => spriteRender = GetComponent<SpriteRenderer>();

    public void SetChaseAnim()
    {
        spriteRender.color = ghostColor;
        if (currentAnim != null)
            StopCoroutine(currentAnim);

        currentAnim = StartCoroutine(PlayAnimCo(Idle));
    }
    public void SetFrightenedAnim()
    {
        spriteRender.color = Color.white;
        if (currentAnim != null) StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(PlayAnimCo(Frightened));
    }

    private IEnumerator PlayAnimCo(Sprite[] sprites)
    {
        while (true)
        {
            foreach (var sprite in sprites)
            {
                spriteRender.sprite = sprite;
                yield return new WaitForSeconds(animSpeed);
            }
        }
    }

    public Color GetColor()
    {
        return ghostColor;
    }

    public void TurnOff()
        => spriteRender.enabled = false;

    public void TurnOn()
        => spriteRender.enabled = true;
}
