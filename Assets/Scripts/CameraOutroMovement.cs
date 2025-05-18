using UnityEngine;
using TMPro;
using System.Collections;

public class CameraOutroMovement : MonoBehaviour
{
    [Header("Caméra")]
    public Transform[] points;                // Positions successives de la caméra
    public float transitionDuration = 5f;    // Durée entre deux points

    [Header("Textes")]
    public TextMeshProUGUI[] texts;           // Textes liés à chaque point
    public float textFadeDuration = 1f;       // Durée du fade in/out du texte

    [Header("UI Finale")]
    public GameObject finalUI;                 // UI finale à activer à la fin
    public float finalUIFadeDuration = 1.5f;  // Durée du fade in de l'UI finale
    public float delayBeforeFinalUI = 1f; // Nouveau délai avant apparition de l'UI finale

    private int currentIndex = 0;
    private float timer = 0f;
    private bool isMoving = true;

    private CanvasGroup finalUICanvasGroup;

    void Start()
    {
        // Masquer tous les textes au départ
        foreach (var t in texts)
            t.alpha = 0f;

        // Préparer l'UI finale
        if (finalUI != null)
        {
            finalUI.SetActive(false);
            finalUICanvasGroup = finalUI.GetComponent<CanvasGroup>();
            if (finalUICanvasGroup == null)
                finalUICanvasGroup = finalUI.AddComponent<CanvasGroup>();
            finalUICanvasGroup.alpha = 0f;
        }

        if (texts.Length > 0)
            StartCoroutine(ShowTextRoutine(currentIndex));
    }

    void Update()
    {
        if (!isMoving || currentIndex >= points.Length - 1)
            return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / transitionDuration);
        t = t * t * (3f - 2f * t);  // easing smoothstep

        // Déplacer la caméra
        transform.position = Vector3.Lerp(points[currentIndex].position, points[currentIndex + 1].position, t);

        if (t >= 1f)
        {
            currentIndex++;
            timer = 0f;

            if (currentIndex >= points.Length - 1)
            {
                isMoving = false;
                ActivateFinalUI();
            }
            else
            {
                StartCoroutine(ShowTextRoutine(currentIndex));
            }
        }
    }

    IEnumerator ShowTextRoutine(int index)
    {
        // Fade in du texte
        yield return StartCoroutine(FadeTextAlpha(texts[index], 0f, 1f, textFadeDuration));

        // Texte visible la majeure partie de la transition (moins fade in/out)
        yield return new WaitForSeconds(transitionDuration - 2 * textFadeDuration);

        // Fade out du texte
        yield return StartCoroutine(FadeTextAlpha(texts[index], 1f, 0f, textFadeDuration));
    }

    IEnumerator FadeTextAlpha(TextMeshProUGUI text, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            text.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
        text.alpha = to;
    }

    void ActivateFinalUI()
    {
        StartCoroutine(DelayedActivateFinalUI());
    }

    IEnumerator DelayedActivateFinalUI()
    {
        yield return new WaitForSeconds(delayBeforeFinalUI);

        if (finalUI != null)
        {
            finalUI.SetActive(true);
            StartCoroutine(FadeCanvasGroup(finalUICanvasGroup, 0f, 1f, finalUIFadeDuration));
        }
    }


    IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
        cg.alpha = to;
    }
}
