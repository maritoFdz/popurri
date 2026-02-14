using System;
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
        // create method that plays the starting game music and shows ready text
        SetGame(defaultScore, defaultLifes);
    }

    private void Update()
    {
        lastPelletTime += Time.deltaTime;
        if (isGameOver && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isGameOver = false;
            SetGame(defaultScore, defaultLifes);
        }
    }

    private void SetGame(int score, int lifes)
    {
        PacmanUI.instance.UpdateLevel(++Level);
        isGameOver = false;
        Score = score;
        Lifes = lifes;
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
        // show game over press enter to restart text
        SetPellets();
        SetGhosts(false);
        SetPacman();
    }

    private void CheckWin()
    {
        foreach (Transform pellet in pellets)
            if (pellet.gameObject.activeSelf) return;
        // freeze all for a moment
        audioSource.PlayOneShot(intermissionMusic);
        // freeze all until music stops and show ready text
        SetGame(Score, Lifes);
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
        CheckWin();
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
}
