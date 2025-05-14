using UnityEngine;

public class ChapterMusicCrossfader : MonoBehaviour
{
    public Transform cameraTransform;

    private float fadeSpeed = 2f;
    public float volume1 = 1f;
    public float volume2 = 0f;
    public float volume3 = 0f;

    private AudioSource ch1;
    private AudioSource ch2;
    private AudioSource ch3;

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

       
        float volume1 = 0f;
        float volume2 = 0f;
        float volume3 = 0f;

        // Détermination des volumes cibles selon la position X
        if (camX >= 0f && camX <= 100f)
        {
            float t = Mathf.InverseLerp(0f, 100f, camX);
            volume1 = 1f - t;
            volume2 = t;
        }
        else if (camX > 100f && camX <= 200f)
        {
            volume2 = 1f;
        }
        else if (camX > 200f && camX <= 300f)
        {
            float t = Mathf.InverseLerp(200f, 300f, camX);
            volume2 = 1f - t;
            volume3 = t;
        }
        else if (camX > 300f)
        {
            volume3 = 1f;
        }

        // Application des volumes cibles dans GlobalSoundManager
        GlobalSoundManager.Instance.chapter1MusicTargetVolume = volume1;
        GlobalSoundManager.Instance.chapter2MusicTargetVolume = volume2;
        GlobalSoundManager.Instance.chapter3MusicTargetVolume = volume3;
    }



}
