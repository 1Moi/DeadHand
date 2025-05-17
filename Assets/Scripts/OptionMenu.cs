using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;


public class OptionMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip Clickage;
    public AudioClip RetourBouton;
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
        float ValeurBis = valeur;
        float volume = ValeurBis / 10;

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

    public void IncreaseVolume(int val, string Type)
    {
        float Sign = val;

        switch (Type)
        {
            case "Musique":
                musicVolume = Mathf.Clamp01(musicVolume + (Sign * 0.1f));
                break;
            case "SFX":
                sfxVolume = Mathf.Clamp01(sfxVolume + (Sign * 0.1f));
                break;
            case "Ambiance":
                ambianceVolume = Mathf.Clamp01(ambianceVolume + (Sign * 0.1f));
                break;
            case "UI":
                UIVolume = Mathf.Clamp01(UIVolume + (Sign * 0.1f));
                break;
        }

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

    public void RetourSon()
    {
        GlobalSoundManager.PlaySFX(RetourBouton);
    }

    public float GetSFXVolume() => sfxVolume;
    public float GetAmbianceVolume() => ambianceVolume;
}