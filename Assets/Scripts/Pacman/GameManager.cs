using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    private int Level;
    private int Score;
    private int Lifes;
    private Ghost blinky;

    private const int defaultScore = 0;
    private const int defaultLifes = 3;
    private const int defaultLevel = 1;
    private bool isGameOver;
    private AudioSource audioSource;

    [Header("Sound Effects and Music")]
    [SerializeField] private AudioClip startMusic;
    [SerializeField] private AudioClip pelletEatenSound;
    [SerializeField] private AudioClip ghostEatenSound;
    [SerializeField] private AudioClip pacmanDeathSound;
    [SerializeField] private AudioClip fruitEatenSound;
    [SerializeField] private AudioClip extraPacSound;
    [SerializeField] private AudioClip intermissionMusic;
    private const float pelletSoundCooldown = 0.5f;
    private float lastPelletTime;

    private void Awake()                                    
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetGame(defaultScore, defaultLifes, defaultLevel);
        StartCoroutine(WaitMusicCo(startMusic));
    }

    private IEnumerator WaitMusicCo(AudioClip music)
    {
        pacman.anim.speed = 0;
        PacmanUI.instance.ShowReadyText(true);
        FreezeAll();
        audioSource.PlayOneShot(music);
        yield return new WaitWhile(()=>audioSource.isPlaying);
        PacmanUI.instance.ShowReadyText(false);
        UnfreezeAll();
        pacman.anim.speed = 1;
    }

    private void Update()
    {
        lastPelletTime += Time.deltaTime;
        if (isGameOver && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            PacmanUI.instance.ShowRestartText(false);
            SetGame(defaultScore, defaultLifes, defaultLevel);
        }
    }

    private void SetGame(int score, int lifes, int level)
    {
        Level = level;
        Score = score;
        Lifes = lifes;
        PacmanUI.instance.UpdateLevel(Level);
        isGameOver = false;
        SetPellets();
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
        foreach (Transform pellet in pellets)
            pellet.gameObject.SetActive(!isGameOver);
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
        foreach (Transform pellet in pellets)
            if (pellet.gameObject.activeSelf) return;
        Level++;
        SetGame(Score, Lifes, Level);
        StartCoroutine(WaitMusicCo(intermissionMusic));
    }

    // In game events
    public void PacmanEatsPellet(Pellet pellet)
    {
        Score += pellet.GetPoints();
        PacmanUI.instance.UpdateScore(Score);
        pellet.gameObject.SetActive(false);
        MakePelletSound();
        CheckWin();
        if (pellet.isPowerPellet)
            foreach (Ghost ghost in ghosts)
            {
                ghost.audioSource.Pause();
                ghost.SwitchState(ghost.frightenedState);
            }
    }

    private void MakePelletSound()
    {
        if (lastPelletTime < pelletSoundCooldown)
            return;
        audioSource.PlayOneShot(pelletEatenSound);
        lastPelletTime = 0;
    }

    public void PacmanEatsGhost(Ghost ghost)
    {
        audioSource.PlayOneShot(ghostEatenSound);
        Score += ghost.ScorePoints;
        PacmanUI.instance.UpdateScore(Score);
    }

    public void PacmanEatsFruit()
    {
        // despues
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
        audioSource.PlayOneShot(pacmanDeathSound);
        pacman.anim.speed = 1;
        pacman.anim.SetTrigger("TouchGhost");
    }

    public void PacmanDeathEnd()
    {
        Lifes--;
        PacmanUI.instance.UpdateLifes(Lifes);
        pacman.gameObject.SetActive(false);
        if (Lifes == 0) SetGameOver();
        else StartCoroutine(ResetGameCo());
    }

    private IEnumerator ResetGameCo() // set Ready! text
    {
        yield return new WaitForSeconds(3f);
        SetGhosts(true);
        SetPacman();
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
