using UnityEngine;

public class ChapterMusicCrossfader : MonoBehaviour
{
    public Transform cameraTransform;

    //private float fadeSpeed = 2f;
    public float volume1 = 1f;
    public float volume2 = 0f;
    public float volume3 = 0f;

    private AudioSource ch1;
    private AudioSource ch2;
    private AudioSource ch3;

    private float EaseIn(float t) => t * t;
    private float EaseOut(float t) => 1f - Mathf.Pow(1f - t, 2);


    void Start()
    {
        ch1 = GlobalSoundManager.Instance.Chapter1MusicSource;
        ch2 = GlobalSoundManager.Instance.Chapter2MusicSource;
        ch3 = GlobalSoundManager.Instance.Chapter3MusicSource;
        GlobalSoundManager.Instance.chapter1MusicTargetVolume = volume1;
        GlobalSoundManager.Instance.chapter2MusicTargetVolume = volume2;
        GlobalSoundManager.Instance.chapter3MusicTargetVolume = volume3;

    }

    void Update()
    {
        UpdateMusicVolumes();
    }

    private void UpdateMusicVolumes()
    {
        float camX = cameraTransform.position.x;

        float target1 = 0f;
        float target2 = 0f;
        float target3 = 0f;

        if (camX < 200f)
        {
            target1 = 1f;
        }
        else if (camX >= 200f && camX < 400f)
        {
            target2 = 1f;
        }
        else if (camX >= 400f)
        {
            target3 = 1f;
        }

        // Smoothly interpolate current volumes toward target values
        float speed = 1.5f; // ajustable

        volume1 = Mathf.MoveTowards(volume1, target1, Time.deltaTime * speed);
        volume2 = Mathf.MoveTowards(volume2, target2, Time.deltaTime * speed);
        volume3 = Mathf.MoveTowards(volume3, target3, Time.deltaTime * speed);

        GlobalSoundManager.Instance.chapter1MusicTargetVolume = volume1;
        GlobalSoundManager.Instance.chapter2MusicTargetVolume = volume2;
        GlobalSoundManager.Instance.chapter3MusicTargetVolume = volume3;

        float globalVolume = GlobalSoundManager.Instance != null ? GlobalSoundManager.Instance.GetMusicVolume() : 1f;

        if (ch1 != null) ch1.volume = globalVolume * volume1;
        if (ch2 != null) ch2.volume = globalVolume * volume2;
        if (ch3 != null) ch3.volume = globalVolume * volume3;

        GlobalSoundManager.SetMusicVolume(GlobalSoundManager.Instance.GetCurrentMusicVolume());
    }

}
