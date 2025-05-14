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

    public void SetMusicVolume0()
    {
        musicVolume = 0f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume1()
    {
        musicVolume = 0.1f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume2()
    {
        musicVolume = 0.2f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume3()
    {
        musicVolume = 0.3f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume4()
    {
        musicVolume = 0.4f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume5()
    {
        musicVolume = 0.5f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume6()
    {
        musicVolume = 0.6f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume7()
    {
        musicVolume = 0.7f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume8()
    {
        musicVolume = 0.8f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume9()
    {
        musicVolume = 0.9f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetMusicVolume10()
    {
        musicVolume = 1f;
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

    public void SetSFXVolume0()
    {
        sfxVolume = 0f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume1()
    {
        sfxVolume = 0.1f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume2()
    {
        sfxVolume = 0.2f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume3()
    {
        sfxVolume = 0.3f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume4()
    {
        sfxVolume = 0.4f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume5()
    {
        sfxVolume = 0.5f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume6()
    {
        sfxVolume = 0.6f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume7()
    {
        sfxVolume = 0.7f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume8()
    {
        sfxVolume = 0.8f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume9()
    {
        sfxVolume = 0.9f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetSFXVolume10()
    {
        sfxVolume = 1f;
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

    public void SetAmbianceVolume0()
    {
        ambianceVolume = 0f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume1()
    {
        ambianceVolume = 0.1f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume2()
    {
        ambianceVolume = 0.2f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume3()
    {
        ambianceVolume = 0.3f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume4()
    {
        ambianceVolume = 0.4f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume5()
    {
        ambianceVolume = 0.5f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume6()
    {
        ambianceVolume = 0.6f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume7()
    {
        ambianceVolume = 0.7f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume8()
    {
        ambianceVolume = 0.8f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume9()
    {
        ambianceVolume = 0.9f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetAmbianceVolume10()
    {
        ambianceVolume = 1f;
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

    public void SetUIVolume0()
    {
        UIVolume = 0f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume1()
    {
        UIVolume = 0.1f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume2()
    {
        UIVolume = 0.2f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume3()
    {
        UIVolume = 0.3f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume4()
    {
        UIVolume = 0.4f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume5()
    {
        UIVolume = 0.5f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume6()
    {
        UIVolume = 0.6f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume7()
    {
        UIVolume = 0.7f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume8()
    {
        UIVolume = 0.8f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume9()
    {
        UIVolume = 0.9f;
        ApplyVolumes();
        UpdateUI();
    }

    public void SetUIVolume10()
    {
        UIVolume = 1f;
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