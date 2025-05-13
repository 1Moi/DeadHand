using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Crank : MonoBehaviour, IPointerDownHandler
{
    public enum CrankType { Horizontal, Vertical }
    public CrankType crankDirection;

    [Header("Logique")]
    public bool inverse = false;            // Inverse le déplacement logique

    [Header("Visuel")]
    public bool inverseVisual = false;      // Inverse uniquement la rotation de la manivelle
    public float rotationAmount = 15f;
    public float rotationDuration = 0.2f;

    public GridManager gridManager;

    private bool isRotating = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRotating) return;

        int deltaX = 0, deltaY = 0;

        if (crankDirection == CrankType.Horizontal)
            deltaX = inverse ? -1 : 1;
        else
            deltaY = inverse ? -1 : 1;

        gridManager.MoveSelector(deltaX, deltaY);

        float visualDirection = inverseVisual ? -1f : 1f;
        StartCoroutine(SmoothRotate(visualDirection * rotationAmount));
    }

    private IEnumerator SmoothRotate(float angle)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, angle);
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
    }
}
