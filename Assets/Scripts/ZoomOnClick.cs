using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class ZoomAndCenterWhileHolding : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float zoomFactor = 1.5f;
    public int zoomSortingOrder = 200;

    private SpriteRenderer sr;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private int originalSortingOrder;
    private bool isHeld = false;
    private Camera mainCamera;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalPosition = transform.position;
        originalSortingOrder = sr.sortingOrder;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isHeld)
        {
            // Zoom + centrer au milieu de l'écran
            transform.localScale = originalScale * zoomFactor;

            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 18f); // distance entre cam -18 et z = 0
            Vector3 worldCenter = mainCamera.ScreenToWorldPoint(screenCenter);
            worldCenter.z = originalPosition.z;
            transform.position = worldCenter;

            sr.sortingOrder = zoomSortingOrder;
        }
        else
        {
            // Retour état initial
            transform.localScale = originalScale;
            transform.position = originalPosition;
            sr.sortingOrder = originalSortingOrder;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
    }
}
