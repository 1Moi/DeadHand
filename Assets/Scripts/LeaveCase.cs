using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class LeaveCase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Effet visuel de survol")]
    public float hoverScale = 1.05f;
    public float transitionSpeed = 10f;

    [Header("Transition vers la case")]
    public Image fadePanel; // Panel noir avec alpha 0 au début
    public float fadeInDuration = 0.8f;
    public float fadeOutDuration = 1.2f;
    public float fadeHoldDuration = 0.3f;
    public Vector3 cameraTargetPosition;
    public GameObject Caneva;

    private bool hovering = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (fadePanel != null)
            fadePanel.enabled = false;
    }

    void Update()
    {
        // Effet de zoom progressif sans rotation
        Vector3 targetScale = hovering ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    public void LEAVE()
    {
        StartCoroutine(FadeAndTeleport());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(FadeAndTeleport());
    }

    public IEnumerator FadeAndTeleport()
    {
        if (fadePanel == null)
        {
            Debug.LogWarning("fadePanel non assigné !");
            yield break;
        }

        fadePanel.enabled = true;

        // Fade in (fondu au noir)
        float t = 0f;
        Color c = fadePanel.color;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeInDuration);
            fadePanel.color = c;
            yield return null;
        }

        // Activation Caneva + déplacement caméra
        if (Caneva != null)
            Caneva.SetActive(true);

        Camera.main.transform.position = new Vector3(
            cameraTargetPosition.x,
            cameraTargetPosition.y,
            Camera.main.transform.position.z);

        yield return new WaitForSeconds(fadeHoldDuration);

        // Fade out (fondu retour)
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(t / fadeOutDuration);
            fadePanel.color = c;
            yield return null;
        }

        fadePanel.enabled = false;
    }
}
