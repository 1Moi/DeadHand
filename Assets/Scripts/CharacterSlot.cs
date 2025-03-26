using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CharacterSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("Sprites métiers (ordre défini)")]
    public List<Sprite> jobSprites;

    [Header("Références")]
    public SpriteRenderer characterRenderer;  // sprite visible
    public SpriteRenderer outlineRendererHover;    // contour pour le hover
    public SpriteRenderer outlineRendererSelected; // contour pour la sélection

    [Header("Effets visuels")]
    public float switchThreshold = 50f;
    public float hoverScale = 1.05f;
    public float transitionSpeed = 10f;
    public float pivotIntensity = 10f;

    [Header("État")]
    public bool isSelectable = true;
    public bool isSelectedByDefault = false;

    private static List<CharacterSlot> selectedSlots = new List<CharacterSlot>();

    private int currentJobIndex = 0;
    private Vector2 dragStart;
    private bool isSelected = false;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private bool hovering = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;

        if (characterRenderer != null && jobSprites.Count > 0)
        {
            characterRenderer.sprite = jobSprites[currentJobIndex];
        }

        outlineRendererHover.enabled = false;
        outlineRendererSelected.enabled = false;

        if (isSelectedByDefault)
        {
            SelectThisSlot(forceKeep: true);
        }
    }

    void Update()
    {
        Vector3 targetScale = (hovering || isSelected) ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (hovering || isSelected)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 localMouse = transform.InverseTransformPoint(mouseWorld);
            float x = Mathf.Clamp(localMouse.x, -1f, 1f);
            float angleZ = -x * pivotIntensity;
            Quaternion targetRot = Quaternion.Euler(0, 0, angleZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * transitionSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        if (!isSelected && outlineRendererHover != null)
            outlineRendererHover.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        if (!isSelected && outlineRendererHover != null)
            outlineRendererHover.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelectable || isSelectedByDefault || isSelected) return;
        SelectThisSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelectable || !isSelected) return;
        dragStart = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isSelected) return;
        float dragDistance = eventData.position.y - dragStart.y;

        if (Mathf.Abs(dragDistance) > switchThreshold)
        {
            int direction = dragDistance > 0 ? 1 : -1;
            SwitchJob(direction);
            dragStart = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // rien pour le moment
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

    void SelectThisSlot(bool forceKeep = false)
    {
        if (!isSelected)
        {
            isSelected = true;
            outlineRendererHover.enabled = false;
            outlineRendererSelected.enabled = true;
            if (!selectedSlots.Contains(this))
                selectedSlots.Add(this);
        }

        if (!forceKeep)
        {
            foreach (var slot in selectedSlots.ToArray())
            {
                if (slot != this && !slot.isSelectedByDefault)
                {
                    slot.Deselect();
                }
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        if (outlineRendererHover != null) outlineRendererHover.enabled = false;
        if (outlineRendererSelected != null) outlineRendererSelected.enabled = false;
        selectedSlots.Remove(this);
    }

    public bool IsSelected() => isSelected;

    public int GetCurrentJobIndex() => currentJobIndex;
}
