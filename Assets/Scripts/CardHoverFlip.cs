using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class CardHoverFlip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Sprites de la carte")]
    public Sprite backSprite;
    public Sprite faceSprite;

    [Header("Réglages visuels")]
    public float hoverScale = 1.15f;
    public int hoverSortingOrder = 50;
    public float scaleDuration = 0.1f;

    private Vector3 originalScale;
    private int originalSortingOrder;

    private SpriteRenderer sr;
    private bool isFaceUp = false;
    private bool isFlipping = false;
    public float flipDuration = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] AudioClick;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalSortingOrder = sr.sortingOrder;

        // Commencer avec le dos visible
        sr.sprite = backSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * hoverScale));
        sr.sortingOrder = hoverSortingOrder;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
        sr.sortingOrder = originalSortingOrder;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFlipping)
        {
            StartCoroutine(FlipCard());
            if (AudioClick != null && AudioClick.Length > 0)
            {
                int index = Random.Range(0, AudioClick.Length);
                GlobalSoundManager.PlaySFX(AudioClick[index]);
            }
        }
    }

    private IEnumerator FlipCard()
    {
        isFaceUp = !isFaceUp;
        sr.sprite = isFaceUp ? faceSprite : backSprite;
        yield return null;
    }


    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / scaleDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
