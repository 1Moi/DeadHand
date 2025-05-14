using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CharacterSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Sprites métiers (ordre défini)")]
    public List<Sprite> jobSprites;

    [Header("Références")]
    public SpriteRenderer characterRenderer;
    public SpriteRenderer outlineRendererHover;
    public SpriteRenderer outlineRendererSelected;
    public List<GameObject> arrowIndicators;

    [Header("Effets visuels")]
    public float hoverScale = 1.15f;
    public float transitionSpeed = 10f;
    public float pivotIntensity = 10f;

    [Header("État")]
    [SerializeField] private bool isSelectable = true;
    public bool isSelectedByDefault = false;

    [Header("Audio & Visuels")]
    [SerializeField] private AudioClip audioClick;
    [SerializeField] private float volume;
    public List<GameObject> comicCaseParents;

    private static List<CharacterSlot> selectedSlots = new List<CharacterSlot>();

    private int currentJobIndex = 0;
    private bool isSelected = false;
    private Vector3 originalScale;
    private Quaternion originalRotation;
    private bool hovering = false;

    [Header("Puzzle Manager & Index")]
    public PuzzleManager puzzleManager;
    public int stepIndex = -1;

    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;

        if (characterRenderer != null && jobSprites.Count > 0)
            characterRenderer.sprite = jobSprites[currentJobIndex];

        outlineRendererHover.enabled = false;
        outlineRendererSelected.enabled = false;

        HideAllCases();

        if (isSelectedByDefault)
            SelectThisSlot(forceKeep: true);
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
        if (!isSelectable) return;
        hovering = true;
        if (!isSelected && outlineRendererHover != null)
            outlineRendererHover.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelectable) return;
        hovering = false;
        if (!isSelected && outlineRendererHover != null)
            outlineRendererHover.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelectable) return;

        // Si déjà sélectionné, clic gauche/droit change le job
        if (isSelected)
        {
            int direction = eventData.button == PointerEventData.InputButton.Right ? -1 : 1;
            SwitchJob(direction);

            // 🔄 Relancer clignotement au cas où il aurait été interrompu
            ShowArrows();

            if (puzzleManager != null && stepIndex >= 0 && stepIndex < puzzleManager.puzzleSteps.Count)
            {
                puzzleManager.CheckSinglePuzzle(stepIndex);
                GlobalSoundManager.PlaySFX(audioClick);
            }
        }

        else
        {
            GlobalSoundManager.PlaySFX(audioClick);
            SelectThisSlot();
        }
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
        if (!isSelectable) return;

        if (!isSelected)
        {
            isSelected = true;
            outlineRendererHover.enabled = false;
            outlineRendererSelected.enabled = true;
            if (!selectedSlots.Contains(this))
                selectedSlots.Add(this);

            ShowAllCases();
            ShowArrows();
        }

        if (!forceKeep)
        {
            foreach (var slot in selectedSlots.ToArray())
            {
                if (slot != this && !slot.isSelectedByDefault)
                    slot.Deselect();
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        outlineRendererHover.enabled = false;
        outlineRendererSelected.enabled = false;
        selectedSlots.Remove(this);
        HideAllCases();
        HideArrows();
    }

    private void ShowAllCases()
    {
        foreach (var parent in comicCaseParents)
        {
            if (parent != null)
            {
                parent.SetActive(true);
                Transform bwChild = parent.transform.Find(parent.name + "BW");
                if (bwChild != null)
                    bwChild.gameObject.SetActive(true);
            }
        }
    }

    private void HideAllCases()
    {
        foreach (var parent in comicCaseParents)
        {
            if (parent != null)
            {
                parent.SetActive(false);
                Transform bwChild = parent.transform.Find(parent.name + "BW");
                if (bwChild != null)
                    bwChild.gameObject.SetActive(false);
            }
        }
    }

    private void ShowArrows()
    {
        foreach (var arrow in arrowIndicators)
        {
            if (arrow != null)
                arrow.SetActive(true);
        }

        if (arrowBlinkCoroutine != null)
        {
            StopCoroutine(arrowBlinkCoroutine); // 🔁 Restart propre
            arrowBlinkCoroutine = null;
        }

        arrowBlinkCoroutine = StartCoroutine(BlinkArrows());
    }

    private void HideArrows()
    {
        foreach (var arrow in arrowIndicators)
        {
            if (arrow != null)
            {
                arrow.SetActive(false);
                var sr = arrow.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = 1f; // Reset alpha
                    sr.color = c;
                }
            }
        }

        if (arrowBlinkCoroutine != null)
        {
            StopCoroutine(arrowBlinkCoroutine);
            arrowBlinkCoroutine = null;
        }
    }


    private Coroutine arrowBlinkCoroutine;

    private IEnumerator BlinkArrows()
    {
        float blinkSpeed = 1f; // 1 cycle par seconde
        float alpha = 0f;
        bool increasing = true;

        while (true)
        {
            alpha += (increasing ? 1 : -1) * Time.deltaTime * blinkSpeed;
            alpha = Mathf.Clamp01(alpha);

            foreach (var arrow in arrowIndicators)
            {
                if (arrow != null)
                {
                    var sr = arrow.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Color c = sr.color;
                        c.a = alpha;
                        sr.color = c;
                    }
                }
            }

            if (alpha >= 1f) increasing = false;
            else if (alpha <= 0f) increasing = true;

            yield return null;
        }
    }


    public int GetCurrentJobIndex() => currentJobIndex;

    public void DeselectFinal()
    {
        isSelectable = false;
        isSelected = false;
        hovering = true;

        outlineRendererHover.enabled = false;
        outlineRendererSelected.enabled = false;
        selectedSlots.Remove(this);

        StopAllCoroutines();

        if (characterRenderer != null && jobSprites.Count > currentJobIndex)
        {
            characterRenderer.sprite = jobSprites[currentJobIndex];
            characterRenderer.color = Color.white;
        }

        HideAllCases();
        HideArrows();

        foreach (var parent in comicCaseParents)
        {
            if (parent != null)
            {
                parent.SetActive(true);
                foreach (Transform child in parent.transform)
                {
                    if (child.name.EndsWith("BW"))
                        child.gameObject.SetActive(false);
                    else if (child.name.EndsWith("RGB"))
                        child.gameObject.SetActive(true);
                }
            }
        }
    }
}
