using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Entities")]
    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;

    [Header("Locations")]
    public GameObject Door;
    public GameObject TopLeft;
    public GameObject TopRight;
    public GameObject BottomLeft;
    public GameObject BottomRight;

    private int level;
    private int score;
    private int lifes;
    private int pelletsEaten;
    private int amountOfpellets;
    private Ghost blinky;

    private const int extraLifeCap = 10000;
    private const int fruitCap = 170;
    private const int defaultScore = 0;
    private const int defaultLifes = 3;
    private const int defaultLevel = 1;
    private bool isGameOver;

    private void Awake()                                    
    {
        if (instance != null) Destroy(instance);
        else instance = this;
    }

    private void Start()
    {
        SetGame(defaultScore, defaultLifes, defaultLevel);
        StartCoroutine(WaitMusicCo(SoundType.StartMusic));
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            PacmanUI.instance.ShowRestartText(false);
            SetGame(defaultScore, defaultLifes, defaultLevel);
        }
    }

    private IEnumerator WaitMusicCo(SoundType sound)
    {
        pacman.anim.speed = 0;
        FreezeAll();
        yield return new WaitForSeconds(1);
        PacmanUI.instance.ShowReadyText(true);
        SoundManager.instance.PlaySound(sound);
        yield return new WaitWhile(()=>SoundManager.instance.IsPlaying());
        PacmanUI.instance.ShowReadyText(false);
        UnfreezeAll();
        pacman.anim.speed = 1;
    }

    private void SetGame(int score, int lifes, int level)
    {
        pelletsEaten = 0;
        this.level = level;
        this.score = score;
        this.lifes = lifes;
        PacmanUI.instance.UpdateLifes(this.lifes);
        PacmanUI.instance.UpdateScore(this.score);
        PacmanUI.instance.UpdateLevel(this.level);
        isGameOver = false;
        SetPellets();
        SetGhosts(true);
        SetPacman();
    }

    private IEnumerator ResetGameCo()
    {
        yield return new WaitForSeconds(3);
        SetGhosts(true);
        SetPacman();
    }

    private void SetGhosts(bool isActive)
    {
        foreach (Ghost ghost in ghosts)
        {
            if (ghost.type == GhostType.Blinky)
                blinky = ghost;
            ghost.gameObject.SetActive(isActive);
            ghost.ResetEntity();
        }
    }

    private void SetPellets()
    {
        amountOfpellets = 0;
        foreach (Transform pellet in pellets)
        {
            amountOfpellets++;
            pellet.gameObject.SetActive(!isGameOver);
        }
    }

    private void SetPacman()
    {
        pacman.gameObject.SetActive(!isGameOver);
        pacman.ResetEntity();
    }

    private void SetGameOver()
    {
        isGameOver = true;
        SetPellets();
        SetGhosts(false);
        SetPacman();
        PacmanUI.instance.ShowRestartText(true);
    }

    private void CheckWin()
    {   
        if (pelletsEaten != amountOfpellets) return;
        level++;
        SetGame(score, lifes, level);
        StartCoroutine(WaitMusicCo(SoundType.IntermissionMusic));
    }

    private void CheckExtraLife()
    {
        if (score % extraLifeCap == 0)
        {
            lifes++;
            SoundManager.instance.PlaySound(SoundType.ExtraLife);
            PacmanUI.instance.UpdateLifes(lifes);
        }
    }

    // In game events
    public void PacmanEatsPellet(Pellet pellet)
    {
        pelletsEaten++;
        if (pelletsEaten % fruitCap == 0)
            FruitGenerator.instance.Generate(level);
        score += pellet.GetPoints();
        PacmanUI.instance.UpdateScore(score);
        pellet.gameObject.SetActive(false);
        SoundManager.instance.PlaySound(SoundType.PelletEaten);
        CheckExtraLife();
        CheckWin();
        if (pellet.isPowerPellet)
            foreach (Ghost ghost in ghosts)
            {
                ghost.audioSource.Pause();
                ghost.SwitchState(ghost.frightenedState);
            }
    }

    public void PacmanEatsGhost(Ghost ghost)
    {
        SoundManager.instance.PlaySound(SoundType.GhostEaten);
        score += ghost.ScorePoints;
        CheckExtraLife();
        PacmanUI.instance.UpdateScore(score);
    }

    public void PacmanEatsFruit(Fruit fruit)
    {
        score += fruit.GetScore();
        SoundManager.instance.PlaySound(SoundType.FruitEaten);
        CheckExtraLife();
        PacmanUI.instance.UpdateScore(score);
    }

    public void KillPacman()
    {
        FreezeAll();
        StartCoroutine(PacmanDeathCo());
    }

    public IEnumerator PacmanDeathCo()
    {
        pacman.anim.speed = 0;
        yield return new WaitForSeconds(1.5f);
        foreach (Ghost ghost in ghosts)
            ghost.gameObject.SetActive(false);
        SoundManager.instance.PlaySound(SoundType.PacmanDeath);
        pacman.anim.speed = 1;
        pacman.anim.SetTrigger("TouchGhost");
    }

    public void PacmanDeathEnd()
    {
        lifes--;
        PacmanUI.instance.UpdateLifes(lifes);
        pacman.gameObject.SetActive(false);
        if (lifes == 0) SetGameOver();
        else StartCoroutine(ResetGameCo());
    }

    public Vector3 GetPacmanPos()
    {
        return pacman.transform.position;
    }

    public Vector3 GetPacmanDir()
    {
        return pacman.direction;
    }

    public Vector3 GetBlinkyPos()
    {
        return blinky.transform.position;
    }

    public void FreezeAll()
    {
        foreach (Ghost ghost in ghosts)
            ghost.Freeze();
        pacman.Freeze();
    }

    public void UnfreezeAll()
    {
        foreach (Ghost ghost in ghosts)
            ghost.Unfreeze();
        pacman.Unfreeze();
    }
}
