using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class HoverCardEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Effets visuels")]
    public float hoverScale = 1.05f;
    public float pivotIntensity = 10f;
    public float transitionSpeed = 10f;

    [Header("Options")]
    public bool zoomable = true;
    public GameObject outlineSprite;

    [Header("Références visuelles")]
    public SpriteRenderer caseBWRenderer;
    public SpriteRenderer caseRGBRenderer;

    [Header("Transition vers la case")]
    public Image fadePanel; // UI Panel noir avec alpha 0 au début
    public float fadeInDuration = 0.8f;
    public float fadeOutDuration = 1.2f;
    public float fadeHoldDuration = 0.3f;
    public Vector3 cameraTargetPosition;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private SpriteRenderer outlineRenderer;
    private int baseRGBSortingOrder;
    private int baseOutlineOrder;

    private bool hovering = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;

        if (caseRGBRenderer != null)
            baseRGBSortingOrder = caseRGBRenderer.sortingOrder;

        if (outlineSprite != null)
        {
            outlineRenderer = outlineSprite.GetComponent<SpriteRenderer>();
            if (outlineRenderer != null)
                baseOutlineOrder = outlineRenderer.sortingOrder;

            outlineSprite.SetActive(false);
        }

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
            fadePanel.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;

        // Toujours afficher le hover visuel, mais outline seulement si RGB actif
        if (caseRGBRenderer != null && caseRGBRenderer.gameObject.activeSelf)
        {
            caseRGBRenderer.sortingOrder = 50;

            if (zoomable && outlineSprite != null && outlineRenderer != null)
            {
                outlineSprite.SetActive(true);
                outlineRenderer.sortingOrder = 45;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        if (caseRGBRenderer != null)
            caseRGBRenderer.sortingOrder = baseRGBSortingOrder;

        if (outlineSprite != null && outlineRenderer != null)
        {
            outlineSprite.SetActive(false);
            outlineRenderer.sortingOrder = baseOutlineOrder;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Déclenche une transition dans la case (fade + TP caméra)
        if (caseRGBRenderer != null && caseRGBRenderer.gameObject.activeSelf)
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        if (fadePanel == null) yield break;

        fadePanel.enabled = true;

        float t = 0f;
        Color c = fadePanel.color;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeInDuration);
            fadePanel.color = c;
            yield return null;
        }

        Camera.main.transform.position = new Vector3(cameraTargetPosition.x, cameraTargetPosition.y, Camera.main.transform.position.z);

        yield return new WaitForSeconds(fadeHoldDuration);

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

    void Update()
    {
        if (hovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, Time.deltaTime * transitionSpeed);

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 localMouse = transform.InverseTransformPoint(mouseWorld);

            float x = Mathf.Clamp(localMouse.x, -1f, 1f);
            float angleZ = -x * pivotIntensity;

            Quaternion targetRot = Quaternion.Euler(0, 0, angleZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * transitionSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * transitionSpeed);
        }
    }
}
