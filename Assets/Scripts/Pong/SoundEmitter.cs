using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [Header("Audio")]
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip audioClip;

    protected virtual void Awake()
        => audioSource = GetComponent<AudioSource>();

    public void MakeSound()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(audioClip);
    }
}
