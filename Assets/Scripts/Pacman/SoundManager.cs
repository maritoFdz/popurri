using UnityEngine;

public enum SoundType { StartMusic, PelletEaten, GhostEaten, PacmanDeath, FruitEaten, ExtraLife, IntermissionMusic }

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource audioSource;
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
        if (instance != null) Destroy(instance);
        else instance = this;
    }

    private void Update()
    {
        lastPelletTime += Time.deltaTime;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.StartMusic:
                audioSource.PlayOneShot(startMusic);
                break;

            case SoundType.PelletEaten:
                MakePelletSound();
                break;

            case SoundType.GhostEaten:
                audioSource.PlayOneShot(ghostEatenSound);
                break;

            case SoundType.PacmanDeath:
                audioSource.PlayOneShot(pacmanDeathSound);
                break;

            case SoundType.FruitEaten:
                audioSource.PlayOneShot(fruitEatenSound);
                break;

            case SoundType.ExtraLife:
                audioSource.PlayOneShot(extraPacSound);
                break;

            case SoundType.IntermissionMusic:
                audioSource.PlayOneShot(intermissionMusic);
                break;
        }
    }

    private void MakePelletSound()
    {
        if (lastPelletTime < pelletSoundCooldown)
            return;
        audioSource.PlayOneShot(pelletEatenSound);
        lastPelletTime = 0;
    }
}
