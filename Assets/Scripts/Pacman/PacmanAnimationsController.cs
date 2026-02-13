using UnityEngine;

public class PacmanAnimationsController : MonoBehaviour
{
    private Pacman pacman;

    private void Awake()
        => pacman = GetComponentInParent<Pacman>();

    public void DeathAnimationEnd()
        => GameManager.instance.PacmanDeathEnd();
}
