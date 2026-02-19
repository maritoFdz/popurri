using UnityEngine;

public enum TetrisSoundType { tetraPlaced, levelUp, gameOver};

[RequireComponent (typeof(AudioSource))]
public class TetrisAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Files")]
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip tetraminoPlacedSound;
    [SerializeField] private AudioClip rowClearedSound;
    [SerializeField] private AudioClip multRowsClearedSound;
    [SerializeField] private AudioClip fourRowsClearedSound;
    [SerializeField] private AudioClip levelUpSound;
    [SerializeField] private AudioClip gameOverSound;

    public void PlaySound(TetrisSoundType sound)
    {
        switch (sound)
        {
            case TetrisSoundType.tetraPlaced:
                sfxSource.PlayOneShot(tetraminoPlacedSound);
                break;
            case TetrisSoundType.levelUp:
                sfxSource.PlayOneShot(levelUpSound);
                break;
            case TetrisSoundType.gameOver:
                sfxSource.PlayOneShot(gameOverSound);
                break;
        }
    }

    public void PlayRowsSound(int amount)
    {
        switch (amount)
        {
            case 1:
                sfxSource.PlayOneShot(rowClearedSound);
                break;
            case 2: case 3:
                sfxSource.PlayOneShot(multRowsClearedSound);
                break;
            case 4:
                sfxSource.PlayOneShot(fourRowsClearedSound);
                break;
        }
    }

    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopPlayingMusic()
    {
        musicSource.Stop();
    }
}
