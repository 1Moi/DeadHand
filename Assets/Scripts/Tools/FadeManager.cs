using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image fadeImage;
    public TMP_Text mainText;
    public TMP_Text continueText;

    [Header("Default Durations")]
    public float defaultFadeIn = 1f;
    public float defaultHold = 1f;
    public float defaultFadeOut = 1f;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] audioType;

    private bool waitingForClick = false;

    /// <summary>
    /// Lance un écran de transition avec option de texte affiché lettre par lettre.
    /// </summary>
    public void StartFadeIn(bool hasText, string message = "", float fadeIn = -1, float hold = -1, float fadeOut = -1)
    {
        float fi = fadeIn >= 0 ? fadeIn : defaultFadeIn;
        float ho = hold >= 0 ? hold : defaultHold;
        float fo = fadeOut >= 0 ? fadeOut : defaultFadeOut;

        StopAllCoroutines();
        StartCoroutine(FadeSequence(hasText, message, fi, ho, fo));
    }

    private IEnumerator FadeSequence(bool hasText, string message, float fadeIn, float hold, float fadeOut)
    {
        // Setup
        if (fadeImage == null) yield break;

        fadeImage.enabled = true;
        fadeImage.color = new Color(0, 0, 0, 0);

        if (mainText != null) mainText.gameObject.SetActive(false);
        if (continueText != null) continueText.gameObject.SetActive(false);
        waitingForClick = false;

        // Fade In
        float t = 0f;
        while (t < fadeIn)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeIn);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        if (hasText && mainText != null)
        {
            mainText.text = "";
            mainText.color = Color.white;
            mainText.gameObject.SetActive(true);

            // Typing effet lettre par lettre
            foreach (char c in message)
            {
                mainText.text += c;
                if (audioType != null && audioType.Length > 0)
                {
                    int indexAudio = Random.Range(0, audioType.Length);
                    GlobalSoundManager.PlaySFX(audioType[indexAudio]);
                }
                yield return new WaitForSeconds(typingSpeed);               
            }

            if (continueText != null)
            {
                continueText.color = Color.white;
                continueText.gameObject.SetActive(true);
                StartCoroutine(BlinkContinueText());
            }

            waitingForClick = true;
            while (!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space))
                yield return null;

            waitingForClick = false;

            if (continueText != null)
                continueText.gameObject.SetActive(false);
            if (mainText != null)
                StartCoroutine(FadeOutText(mainText));
        }
        else
        {
            yield return new WaitForSeconds(hold);
        }

        // Fade Out
        t = 0f;
        while (t < fadeOut)
        {
            t += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(t / fadeOut);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.enabled = false;
    }

    private IEnumerator BlinkContinueText()
    {
        float blinkSpeed = 0.5f;
        Color baseColor = continueText.color;

        while (waitingForClick && continueText != null)
        {
            continueText.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
            yield return new WaitForSeconds(blinkSpeed);
            continueText.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
            yield return new WaitForSeconds(blinkSpeed);
        }

        if (continueText != null)
            continueText.color = baseColor;
    }

    private IEnumerator FadeOutText(TMP_Text text)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color originalColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / duration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        text.text = "";
        text.color = originalColor;
        text.gameObject.SetActive(false);
    }
}
