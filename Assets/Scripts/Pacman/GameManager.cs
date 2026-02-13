using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Entities")]
    [SerializeField] private Ghost[] ghosts;
    public Pacman pacman;
    [SerializeField] private Transform bolitas;

    [Header("Locations")]
    public GameObject Door;
    public GameObject TopLeft;
    public GameObject TopRight;
    public GameObject BottomLeft;
    public GameObject BottomRight;

    public int Level { get; private set; }
    public int Score { get; private set; }
    public int Lifes { get; private set; }
    public float TimeSeconds { get; private set; }
    public Ghost Blinky { get; private set; }

    private const int defaultScore = 0;
    private const int defaultLifes = 3;
    private bool isGameOver;
    private AudioSource audioSource;

    [Header("Sound Effects and Music")]
    [SerializeField] private AudioClip startMusic;
    [SerializeField] private AudioClip bolitaEatenSound;
    [SerializeField] private AudioClip ghostEatenSound;
    [SerializeField] private AudioClip pacmanDeathSound;
    [SerializeField] private AudioClip fruitEatenSound;
    [SerializeField] private AudioClip extraPacSound;
    [SerializeField] private AudioClip intermissionMusic;
    private const float bolitaCooldown = 0.54f;
    private float lastBolitaTime;

    private void Awake()                                    
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetGame(defaultScore, defaultLifes);
    }

    private void Update()
    {
        TimeSeconds += Time.deltaTime;
        if (isGameOver && Input.GetKeyDown(KeyCode.KeypadEnter))
            SetGame(defaultScore, defaultLifes);
    }

    private void SetGame(int score, int lifes)
    {
        isGameOver = false;
        Score = score;
        PacmanUI.instance.UpdateScore(Score);
        Lifes = lifes;
        SetBolitas();
        SetGhosts();
        SetPacman();
    }

    private void SetGameOver()
    {
        isGameOver = true;
        SetBolitas();
        SetGhosts();
        SetPacman();
    }

    private void SetPacman()
    {
        pacman.gameObject.SetActive(!isGameOver);
        pacman.ResetEntity();
    }

    private void SetGhosts()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            // ghosts[i].transform.position = new Vector3(0f, 0f, 0f);
            if (ghosts[i].type == GhostType.Blinky)
                Blinky = ghosts[i];
            ghosts[i].Unfreeze();
            ghosts[i].gameObject.SetActive(!isGameOver);
        }
    }

    private void SetBolitas()
    {
        foreach (Transform bolita in bolitas)
            bolita.gameObject.SetActive(!isGameOver);
    }

    private IEnumerator ResetGameCo()
    {
        yield return new WaitForSeconds(3f);
        SetGhosts();
        SetPacman();
    }

    private void CheckWin()
    {
        foreach (Transform bolita in bolitas)
            if (bolita.gameObject.activeSelf) return;
        audioSource.PlayOneShot(intermissionMusic);
        SetGame(Score, Lifes);
    }

    // Events (public methods) 
    public void PacmanEatsSmallBolita(SmallBolita bolita) // esto se debe mejorar fula haciendo una clase abstracta o algo deso
    {
        Score += bolita.ScorePoints;
        PacmanUI.instance.UpdateScore(Score);
        bolita.gameObject.SetActive(false);
        MakeBolitaSound();
        CheckWin();
    }

    public void PacmanEatsBigBolita(BolitaBig bolita)
    {
        Score += bolita.ScorePoints;
        bolita.gameObject.SetActive(false);
        MakeBolitaSound();
        CheckWin();
        foreach (Ghost ghost in ghosts)
        {
            ghost.audioSource.Pause();
            ghost.SwitchState(ghost.frightenedState);
        }
    }

    private void MakeBolitaSound()
    {
        if (Time.time - lastBolitaTime < bolitaCooldown)
            return;
        audioSource.PlayOneShot(bolitaEatenSound);
        lastBolitaTime = Time.time;
        CheckWin();
    }

    public void PacmanEatsGhost(Ghost ghost)
    {
        audioSource.PlayOneShot(ghostEatenSound);
        Score += ghost.GetPoints();
    }

    public void PacmanEatsFruit()
    {
        // despues
    }

    public void KillPacman()
    {
        FreezeAll();
        StartCoroutine(PacmanDies());
    }

    public IEnumerator PacmanDies()
    {
        pacman.anim.speed = 0;
        yield return new WaitForSeconds(1.5f);
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

    public void FreezeAll()
    {
        foreach (Ghost ghost in ghosts)
            ghost.Freeze();
        pacman.Freeze();
    }
}
