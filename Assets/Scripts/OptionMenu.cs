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
    [Range(0f, 1f)] public float UIVolume = 1f;

    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;
    public TextMeshProUGUI ambianceVolumeText;
    public TextMeshProUGUI UIVolumeText;

    private void Start()
    {
        UpdateUI();
        ApplyVolumes();
    }

    public void SetSound(int valeur, string Name)
    {
        float volume = valeur / 10;
        musicVolume = volume;

        switch (Name)
            {
            case "Musique":
                musicVolume = volume;
                break;
            case "SFX":
                sfxVolume = volume;
                break;
            case "Ambiance":
                ambianceVolume = volume;
                break;
            case "UI":
                UIVolume = volume;
                break;
            }
        
        ApplyVolumes();
        UpdateUI();
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

    public void SetSFXVolume(int valeur)
    {
        float volume = valeur / 10;
        sfxVolume = volume;
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

    public void SetAmbianceVolume(int valeur)
    {
        float volume = valeur / 10;
        ambianceVolume = volume;
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

    public void SetUIVolume0(int valeur)
    {
        float volume = valeur / 10;
        UIVolume = volume;
        ApplyVolumes();
        UpdateUI();
    }

    public void IncreaseUIVolume()
    {
        UIVolume = Mathf.Clamp01(UIVolume + 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    public void DecreaseUIVolume()
    {
        UIVolume = Mathf.Clamp01(UIVolume - 0.1f);
        ApplyVolumes();
        UpdateUI();
    }

    private void ApplyVolumes()
    {
        GlobalSoundManager.SetMusicVolume(musicVolume);
        GlobalSoundManager.SetSFXVolume(sfxVolume);
        GlobalSoundManager.SetAmbianceVolume(ambianceVolume);
        GlobalSoundManager.SetUIVolume(UIVolume);
        GlobalSoundManager.PlayUI(Clickage);
    }

    private void UpdateUI()
    {
        if (musicVolumeText) musicVolumeText.text = $"Volume Musique : {Mathf.RoundToInt(musicVolume * 10)}";
        if (sfxVolumeText) sfxVolumeText.text = $"Volume SFX : {Mathf.RoundToInt(sfxVolume * 10)}";
        if (ambianceVolumeText) ambianceVolumeText.text = $"Volume Ambiance : {Mathf.RoundToInt(ambianceVolume * 10)}";
        if (UIVolumeText) UIVolumeText.text = $"Volume UI : {Mathf.RoundToInt(UIVolume * 10)}";
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