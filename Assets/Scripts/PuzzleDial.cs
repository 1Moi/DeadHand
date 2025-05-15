using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class PuzzleDial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Sprites de la molette")]
    public List<Sprite> rotationSprites;
    private int currentIndex = 0;

    [Header("Feedback visuel")]
    public GameObject outlineObject; // Un GameObject (sprite) activé/désactivé au survol
    public Color clickFeedbackColor = Color.yellow;
    public float feedbackDuration = 0.1f;

    [Header("Références")]
    public PuzzleDialManager manager; // Script parent qui vérifie la combinaison
    private SpriteRenderer sr;
    private Color defaultColor;

    [Header("Audio")]
    [SerializeField] private AudioClip[] AudioClick;
    [SerializeField] private float volume;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;

        if (rotationSprites.Count > 0)
            sr.sprite = rotationSprites[0];

        if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outlineObject != null)
            outlineObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outlineObject != null)
            outlineObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = Random.Range(0, AudioClick.Length);
        GlobalSoundManager.PlaySFX(AudioClick[index]);

        RotateDial();
        StartCoroutine(ClickFeedback());

        if (manager != null)
            manager.CheckCombination();
    }

    private void RotateDial()
    {
        if (rotationSprites.Count == 0) return;

        currentIndex = (currentIndex + 1) % rotationSprites.Count;
        sr.sprite = rotationSprites[currentIndex];
    }

    private IEnumerator ClickFeedback()
    {
        sr.color = clickFeedbackColor;
        yield return new WaitForSeconds(feedbackDuration);
        sr.color = defaultColor;
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}