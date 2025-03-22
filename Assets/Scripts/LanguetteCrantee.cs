using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LanguetteCrantee : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("Liste des crans")]
    public List<Vector3> cransLanguette;
    public List<Vector3> cransImage;
    private Vector3 offset;

    [Header("Options")]
    public int cranDeDepart = 0;
    public bool isHorizontal = true;
    public bool isOnRightPage = true;
    public float vitesseDeplacement = 0.005f;
    public float vitesseRetour = 10f;
    [Tooltip("Facteur de résistance (0 = forte résistance, 1 = pas de résistance)")]
    public float resistanceFactor = 0.2f; // Empêche de trop dépasser

    [Header("Image attachée ?")]
    public bool hasAttachedImage = false;
    public Transform imageAttachée;

    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;
    public int puzzleStepIndex;

    private Vector3 startPos;
    //private bool isDragging = false;

    private void Start()
    {
        if (cransLanguette.Count == 0) return;

        if (hasAttachedImage && (cransImage.Count == 0 || cransLanguette.Count != cransImage.Count))
        {
            Debug.LogError("Les listes cransLanguette et cransImage doivent être de la même taille !");
            return;
        }

        cranDeDepart = Mathf.Clamp(cranDeDepart, 0, cransLanguette.Count - 1);
        transform.localPosition = cransLanguette[cranDeDepart];

        if (hasAttachedImage && imageAttachée != null)
        {
            imageAttachée.localPosition = cransImage[cranDeDepart];
        }

        startPos = transform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            offset = transform.position - worldPoint;
        }

        //isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 newPosition = ray.GetPoint(distance) + offset;
            newPosition.z = transform.position.z;

            if (isHorizontal)
            {
                float minX = Mathf.Min(cransLanguette[0].x, cransLanguette[^1].x);
                float maxX = Mathf.Max(cransLanguette[0].x, cransLanguette[^1].x);

                // Appliquer la résistance aux extrêmes
                if (newPosition.x < minX)
                    newPosition.x = Mathf.Lerp(newPosition.x, minX, 1 - resistanceFactor);
                if (newPosition.x > maxX)
                    newPosition.x = Mathf.Lerp(newPosition.x, maxX, 1 - resistanceFactor);

                newPosition.y = transform.position.y;
            }
            else
            {
                float minY = Mathf.Min(cransLanguette[0].y, cransLanguette[^1].y);
                float maxY = Mathf.Max(cransLanguette[0].y, cransLanguette[^1].y);

                if (newPosition.y < minY)
                    newPosition.y = Mathf.Lerp(newPosition.y, minY, 1 - resistanceFactor);
                if (newPosition.y > maxY)
                    newPosition.y = Mathf.Lerp(newPosition.y, maxY, 1 - resistanceFactor);

                newPosition.x = transform.position.x;
            }

            transform.position = newPosition;

            if (hasAttachedImage && imageAttachée != null)
            {
                int cranLePlusProcheIndex = TrouverCranProche(transform.localPosition);
                imageAttachée.localPosition = Vector3.Lerp(imageAttachée.localPosition, cransImage[cranLePlusProcheIndex], Time.deltaTime * vitesseRetour);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //isDragging = false;
        int cranLePlusProcheIndex = TrouverCranProche(transform.localPosition);
        StopAllCoroutines();
        StartCoroutine(SnapToCran(cranLePlusProcheIndex));

        if (puzzleManager != null)
        {
            puzzleManager.CheckSinglePuzzle(puzzleStepIndex);
        }
    }

    private int TrouverCranProche(Vector3 currentPos)
    {
        int cranLePlusProcheIndex = 0;
        float distanceMin = Vector3.Distance(currentPos, cransLanguette[0]);

        for (int i = 1; i < cransLanguette.Count; i++)
        {
            float distance = Vector3.Distance(currentPos, cransLanguette[i]);
            if (distance < distanceMin)
            {
                distanceMin = distance;
                cranLePlusProcheIndex = i;
            }
        }
        return cranLePlusProcheIndex;
    }

    private System.Collections.IEnumerator SnapToCran(int index)
    {
        Vector3 targetLanguette = cransLanguette[index];
        Vector3 targetImage = hasAttachedImage ? cransImage[index] : Vector3.zero;

        while (Vector3.Distance(transform.localPosition, targetLanguette) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetLanguette, Time.deltaTime * vitesseRetour);

            if (hasAttachedImage && imageAttachée != null)
            {
                imageAttachée.localPosition = Vector3.Lerp(imageAttachée.localPosition, targetImage, Time.deltaTime * vitesseRetour);
            }

            yield return null;
        }

        transform.localPosition = targetLanguette;
        if (hasAttachedImage && imageAttachée != null)
        {
            imageAttachée.localPosition = targetImage;
        }
    }

    public int GetCranIndex()
    {
        return TrouverCranProche(transform.localPosition);
    }
}
