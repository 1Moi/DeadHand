using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OptionMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip Clickage;
    public float volume;


    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float ambianceVolume = 1f;

    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public TextMeshProUGUI ambianceVolumeText;

    private void Start()
    {
        UpdateUI();
        ApplyVolumes();
    }

    public void IncreaseMusicVolume()
    {
        musicVolume = Mathf.Clamp01(musicVolume + 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    public void DecreaseMusicVolume()
    {
        musicVolume = Mathf.Clamp01(musicVolume - 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    public void IncreaseSFXVolume()
    {
        sfxVolume = Mathf.Clamp01(sfxVolume + 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    public void DecreaseSFXVolume()
    {
        sfxVolume = Mathf.Clamp01(sfxVolume - 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    public void IncreaseAmbianceVolume()
    {
        ambianceVolume = Mathf.Clamp01(ambianceVolume + 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    public void DecreaseAmbianceVolume()
    {
        ambianceVolume = Mathf.Clamp01(ambianceVolume - 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    private void ApplyVolumes()
    {
        GlobalSoundManager.SetMusicVolume(musicVolume);
        GlobalSoundManager.SetSFXVolume(sfxVolume);
        GlobalSoundManager.SetAmbianceVolume(ambianceVolume);
        GlobalSoundManager.PlaySFX(Clickage);
    }

    private void UpdateUI()
    {
        if (musicVolumeText) musicVolumeText.text = $"{Mathf.RoundToInt(musicVolume * 10)}";
        if (sfxVolumeText) sfxVolumeText.text = $"{Mathf.RoundToInt(sfxVolume * 10)}";
        if (ambianceVolumeText) ambianceVolumeText.text = $"{Mathf.RoundToInt(ambianceVolume * 10)}";
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public float GetSFXVolume() => sfxVolume;
    public float GetAmbianceVolume() => ambianceVolume;
}