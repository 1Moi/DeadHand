using UnityEngine;

public class GlobalSoundManager : MonoBehaviour
{
    private static GlobalSoundManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource ambianceAudioSource;
    [SerializeField] private AudioSource uiAudioSource;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float ambianceVolume = 1f;
    private float uiVolume = 1f;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistance entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Musique
    public static void PlayMusic(AudioClip music)
    {
        instance.musicAudioSource.clip = music;
        instance.musicAudioSource.volume = instance.musicVolume;
        instance.musicAudioSource.loop = true;
        instance.musicAudioSource.Play();
    }

    public static void SetMusicVolume(float volume)
    {
        instance.musicVolume = volume;
        instance.musicAudioSource.volume = volume;
    }

    // Effets sonores
    public static void PlaySFX(AudioClip sfxClip)
    {
        instance.sfxAudioSource.PlayOneShot(sfxClip, instance.sfxVolume);
    }

    public static void SetSFXVolume(float volume)
    {
        instance.sfxVolume = volume;
    }

    // Ambiance
    public static void PlayAmbiance(AudioClip ambianceClip)
    {
        instance.ambianceAudioSource.clip = ambianceClip;
        instance.ambianceAudioSource.volume = instance.ambianceVolume;
        instance.ambianceAudioSource.loop = true;
        instance.ambianceAudioSource.Play();
    }

    public static void SetAmbianceVolume(float volume)
    {
        instance.ambianceVolume = volume;
        instance.ambianceAudioSource.volume = volume;
    }

    // UI
    public static void PlayUI(AudioClip uiClip)
    {
        instance.uiAudioSource.PlayOneShot(uiClip, instance.uiVolume);
    }

    public static void SetUIVolume(float volume)
    {
        instance.uiVolume = volume;
        instance.uiAudioSource.volume = volume;
    }
}
