using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCardEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Effets visuels")]
    public float hoverScale = 1.05f;
    public float pivotIntensity = 10f;
    public float transitionSpeed = 10f;

    [Header("Options")]
    public bool zoomable = true;
    public GameObject outlineSprite;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private SpriteRenderer mainRenderer;
    private SpriteRenderer outlineRenderer;
    private int baseSortingOrder;
    private int baseOutlineOrder;

    private bool hovering = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;

        mainRenderer = GetComponent<SpriteRenderer>();
        if (mainRenderer != null)
            baseSortingOrder = mainRenderer.sortingOrder;

        if (outlineSprite != null)
        {
            outlineRenderer = outlineSprite.GetComponent<SpriteRenderer>();
            if (outlineRenderer != null)
                baseOutlineOrder = outlineRenderer.sortingOrder;

            outlineSprite.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;

        if (mainRenderer != null)
            mainRenderer.sortingOrder = 50;

        if (zoomable && outlineSprite != null && outlineRenderer != null)
        {
            outlineSprite.SetActive(true);
            outlineRenderer.sortingOrder = 45;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        if (mainRenderer != null)
            mainRenderer.sortingOrder = baseSortingOrder;

        if (outlineSprite != null && outlineRenderer != null)
        {
            outlineSprite.SetActive(false);
            outlineRenderer.sortingOrder = baseOutlineOrder;
        }
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
