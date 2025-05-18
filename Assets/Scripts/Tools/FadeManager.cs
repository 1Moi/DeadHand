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

    private bool waitingForClick = false;

    /// <summary>
    /// Lance un écran de transition.
    /// </summary>
    /// <param name="hasText">Affiche un texte et attend un clic si true</param>
    /// <param name="message">Texte à afficher</param>
    /// <param name="fadeIn">Durée du fade-in</param>
    /// <param name="hold">Durée du hold si pas de texte</param>
    /// <param name="fadeOut">Durée du fade-out</param>
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
        // Préparation
        if (fadeImage == null) yield break;
        fadeImage.enabled = true;
        fadeImage.color = new Color(0, 0, 0, 0); // transparent

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

        // === Si du texte, on affiche + attend le clic ===
        if (hasText && mainText != null)
        {
            mainText.text = message;
            mainText.gameObject.SetActive(true);

            if (continueText != null)
            {
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
                mainText.gameObject.SetActive(false);
        }
        else
        {
            // === Pas de texte : on attend un temps fixe ===
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
}
