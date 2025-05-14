using UnityEngine;

public class MenuToChapterMusicManager : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Menu Music")]
    public AudioClip menuMusicClip;

    [Header("Chapter Music Clips")]
    public AudioClip chapter1Clip;
    public AudioClip chapter2Clip;
    public AudioClip chapter3Clip;

    private bool chaptersStarted = false;

    void Start()
    {
        // Démarre la musique du menu dès le début
        GlobalSoundManager.PlayMusic(menuMusicClip);
    }

    void Update()
    {
        // Dès que la caméra quitte la zone du menu, on lance les musiques des chapitres
        if (!chaptersStarted && cameraTransform.position.x > -50f)
        {
            chaptersStarted = true;

            GlobalSoundManager.FadeOutMenuMusic(1f); // 1 secondes de fondu

            // Lance les 3 musiques des chapitres
            GlobalSoundManager.PlayChapterMusics(chapter1Clip, chapter2Clip, chapter3Clip);
        }
    }
}
