using System.Resources;
using UnityEngine;

public class GlobalSoundManager : MonoBehaviour
{
    private static GlobalSoundManager instance;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource musicAudioSource;

    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void PlaySound(AudioClip audioClip, float volume)
        => instance.audioSource.PlayOneShot(audioClip, volume);
    
    public static void PlayMusic(AudioClip music, float volume)
    {
        instance.musicAudioSource.clip = music;
        instance.musicAudioSource.volume = volume;
        instance.musicAudioSource.Play();
    }
}
