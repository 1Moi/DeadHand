using UnityEngine;
using System.Collections;


public class GlobalSoundManager : MonoBehaviour
{
    private static GlobalSoundManager instance;
    public static GlobalSoundManager Instance => instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource ambianceAudioSource;
    [SerializeField] private AudioSource uiAudioSource;

    [Header("Chapter Music Sources")]
    public AudioSource chapter1MusicSource;
    public AudioSource chapter2MusicSource;
    public AudioSource chapter3MusicSource;

    [HideInInspector] public float chapter1MusicTargetVolume = 1f;
    [HideInInspector] public float chapter2MusicTargetVolume = 0f;
    [HideInInspector] public float chapter3MusicTargetVolume = 0f;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambianceVolume = 1f;
    public float uiVolume = 1f;

    public float GetCurrentMusicVolume() => musicVolume;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ----- Menu Music -----
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
        instance.musicAudioSource.volume = volume; // Volume de la musique du menu

        // Appliquer le volume global * target volume pour les musiques des chapitres
        instance.chapter1MusicSource.volume = volume * instance.chapter1MusicTargetVolume;
        instance.chapter2MusicSource.volume = volume * instance.chapter2MusicTargetVolume;
        instance.chapter3MusicSource.volume = volume * instance.chapter3MusicTargetVolume;
    }


    public static void StopMusic()
    {
        instance.musicAudioSource.Stop();
    }

    // ----- SFX -----
    public static void PlaySFX(AudioClip sfxClip)
    {
        instance.sfxAudioSource.PlayOneShot(sfxClip, instance.sfxVolume);
    }

    public static void SetSFXVolume(float volume)
    {
        instance.sfxVolume = volume;
    }

    // ----- Ambiance -----
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

    // ----- UI -----
    public static void PlayUI(AudioClip uiClip)
    {
        instance.uiAudioSource.PlayOneShot(uiClip, instance.uiVolume);
    }

    public static void SetUIVolume(float volume)
    {
        instance.uiVolume = volume;
        instance.uiAudioSource.volume = volume;
    }

    // ----- Chapter Music -----
    public static void PlayChapterMusics(AudioClip ch1, AudioClip ch2, AudioClip ch3)
    {
        instance.chapter1MusicSource.clip = ch1;
        instance.chapter2MusicSource.clip = ch2;
        instance.chapter3MusicSource.clip = ch3;

        instance.chapter1MusicSource.volume = 1f;
        instance.chapter2MusicSource.volume = 0f;
        instance.chapter3MusicSource.volume = 0f;

        instance.chapter1MusicSource.loop = true;
        instance.chapter2MusicSource.loop = true;
        instance.chapter3MusicSource.loop = true;

        instance.chapter1MusicSource.Play();
        instance.chapter2MusicSource.Play();
        instance.chapter3MusicSource.Play();
    }

    // ----- Getters for ChapterMusicCrossfader -----
    public AudioSource Chapter1MusicSource => chapter1MusicSource;
    public AudioSource Chapter2MusicSource => chapter2MusicSource;
    public AudioSource Chapter3MusicSource => chapter3MusicSource;

    public static void FadeOutMenuMusic(float duration)
    {
        instance.StartCoroutine(instance.FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = musicAudioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        musicAudioSource.Stop();
        musicAudioSource.volume = musicVolume; // Reset pour la prochaine fois
    }

    public static void SetAllMusicVolumes(float volume)
    {
        instance.musicVolume = volume;

        if (instance.musicAudioSource != null)
            instance.musicAudioSource.volume = volume;

        if (instance.chapter1MusicSource != null)
            instance.chapter1MusicSource.volume = volume * instance.chapter1MusicTargetVolume;

        if (instance.chapter2MusicSource != null)
            instance.chapter2MusicSource.volume = volume * instance.chapter2MusicTargetVolume;

        if (instance.chapter3MusicSource != null)
            instance.chapter3MusicSource.volume = volume * instance.chapter3MusicTargetVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

}
