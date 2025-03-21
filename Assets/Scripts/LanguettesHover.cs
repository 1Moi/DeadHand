using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LanguetteHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject outlineSprite; // Le sprite qui fait le contour
    public Color baseColor = Color.white;
    public Color hoverColor = Color.green;
    public float colorChangeSpeed = 5f;
    public float fadeOutDelay = 0.2f; // Temps avant que le glow disparaisse

    private SpriteRenderer outlineRenderer;
    private Coroutine colorCoroutine;

    void Start()
    {
        if (outlineSprite != null)
        {
            outlineRenderer = outlineSprite.GetComponent<SpriteRenderer>();
            outlineSprite.SetActive(false); // Cache le contour au départ
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outlineSprite != null)
        {
            outlineSprite.SetActive(true);
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(ChangeColor(hoverColor));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outlineSprite != null)
        {
            if (colorCoroutine != null) StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(ChangeColor(baseColor));
            StartCoroutine(DisableAfterDelay(fadeOutDelay)); // Désactive après le délai défini
        }
    }

    IEnumerator ChangeColor(Color targetColor)
    {
        while (outlineRenderer.color != targetColor)
        {
            outlineRenderer.color = Color.Lerp(outlineRenderer.color, targetColor, Time.deltaTime * colorChangeSpeed);
            yield return null;
        }
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        outlineSprite.SetActive(false);
    }
}
