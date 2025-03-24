using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CharacterSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("Sprites métiers (ordre défini)")]
    public List<Sprite> jobSprites;

    [Header("Références")]
    public SpriteRenderer characterRenderer;  // sprite visible
    public SpriteRenderer outlineRenderer;    // contour pour la sélection
    public float switchThreshold = 50f;       // distance en pixels pour déclencher un changement
    public float transitionSpeed = 10f;

    [Header("État")]
    public bool isSelectable = true;
    public bool isSelectedByDefault = false;

    private int currentJobIndex = 0;
    private Vector2 dragStart;
    private bool isSelected = false;

    void Start()
    {
        if (characterRenderer != null && jobSprites.Count > 0)
        {
            characterRenderer.sprite = jobSprites[currentJobIndex];
        }

        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = isSelectedByDefault;
        }

        isSelected = isSelectedByDefault;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelectable) return;

        // Sélection
        isSelected = true;
        if (outlineRenderer != null)
            outlineRenderer.enabled = true;

        dragStart = eventData.position;

        // TODO : informer les autres CharacterSlot de se désélectionner si besoin
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isSelected) return;

        float dragDistance = eventData.position.y - dragStart.y;

        if (Mathf.Abs(dragDistance) > switchThreshold)
        {
            int direction = dragDistance > 0 ? 1 : -1;
            SwitchJob(direction);
            dragStart = eventData.position; // reset pour ne pas faire défiler plusieurs fois d’un coup
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // rien de spécial ici pour l’instant
    }

    void SwitchJob(int direction)
    {
        currentJobIndex = (currentJobIndex + direction + jobSprites.Count) % jobSprites.Count;

        if (characterRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothSpriteChange(jobSprites[currentJobIndex]));
        }
    }

    IEnumerator SmoothSpriteChange(Sprite newSprite)
    {
        // Simple effet de fade out / fade in
        float t = 0f;
        while (t < 1f)
        {
            characterRenderer.color = new Color(1f, 1f, 1f, 1f - t);
            t += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        characterRenderer.sprite = newSprite;

        t = 0f;
        while (t < 1f)
        {
            characterRenderer.color = new Color(1f, 1f, 1f, t);
            t += Time.deltaTime * transitionSpeed;
            yield return null;
        }

        characterRenderer.color = Color.white;
    }

    public int GetCurrentJobIndex()
    {
        return currentJobIndex;
    }

    public void Deselect()
    {
        isSelected = false;
        if (outlineRenderer != null)
            outlineRenderer.enabled = false;
    }
}
