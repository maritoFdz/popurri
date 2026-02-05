using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    private Pacman pacman;

    private void Awake()
    {
        pacman = GetComponentInParent<Pacman>();
    }

    public void DeathAnimationEnd()
        => GameManager.instance.PacmanDeathEnd();
}
