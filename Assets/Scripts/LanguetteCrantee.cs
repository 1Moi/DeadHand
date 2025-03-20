using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LanguetteCrantee : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    [Header("Liste des crans")]
    public List<Vector3> cransLanguette;  // Liste des positions de la languette
    public List<Vector3> cransImage;      // Liste des positions de l’image attachée
    private Vector3 offset;

    [Header("Options")]
    public int cranDeDepart = 0;          // Index du cran où commence la languette
    public bool isHorizontal = true;      // Déplacement horizontal (Z) ou vertical (X)
    public bool isOnRightPage = true;     // La languette est-elle sur la page de droite ?
    public float vitesseDeplacement = 0.005f; // Sensibilité du mouvement
    public float vitesseRetour = 10f;     // Vitesse du snap vers un cran
    [Tooltip("Facteur de résistance (entre 0 et 1) lorsque la languette dépasse les limites")]
    public float resistanceFactor = 0.2f; // 0 = très forte résistance, 1 = pas de résistance

    [Header("Image attachée ?")]
    public bool hasAttachedImage = false; // La languette déplace-t-elle une image ?
    public Transform imageAttachée;       // Référence vers l'image à déplacer (optionnel)

    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;   // Référence vers le PuzzleManager
    public int puzzleStepIndex; // Référence vers l'étape du puzzle

    private void Start()
    {
        if (cransLanguette.Count == 0)
        {
            //Debug.LogError("La liste des crans de la languette est vide !");
            return;
        }

        if (hasAttachedImage)
        {
            if (cransImage.Count == 0)
            {
                //Debug.LogError("La liste des cransImage est vide alors que hasAttachedImage est true !");
                return;
            }
            if (cransLanguette.Count != cransImage.Count)
            {
                //Debug.LogError("Les listes cransLanguette et cransImage ne sont pas de la même taille !");
                return;
            }
        }

        if (transform.parent != null)
        {
            //Debug.Log("La languette est un enfant d'un autre objet ! Utilisation de localPosition.");
        }

        cranDeDepart = Mathf.Clamp(cranDeDepart, 0, cransLanguette.Count - 1);
        transform.localPosition = cransLanguette[cranDeDepart];

        if (hasAttachedImage && imageAttachée != null)
        {
            imageAttachée.localPosition = cransImage[cranDeDepart];
        }
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

        //Debug.Log("Languette cliquée à : " + transform.position);
    }


    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.forward, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 newPosition = ray.GetPoint(distance) + offset;
            newPosition.z = transform.position.z; // Empêche tout déplacement en profondeur

            if (isHorizontal) // Déplacement uniquement sur X
            {
                float minX = Mathf.Min(cransLanguette[0].x, cransLanguette[cransLanguette.Count - 1].x);
                float maxX = Mathf.Max(cransLanguette[0].x, cransLanguette[cransLanguette.Count - 1].x);
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = transform.position.y; // Bloque Y pour éviter un mouvement parasite
            }
            else // Déplacement uniquement sur Y
            {
                float minY = Mathf.Min(cransLanguette[0].y, cransLanguette[cransLanguette.Count - 1].y);
                float maxY = Mathf.Max(cransLanguette[0].y, cransLanguette[cransLanguette.Count - 1].y);
                newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
                newPosition.x = transform.position.x; // Bloque X pour éviter un mouvement parasite
            }

            transform.position = newPosition;
            //Debug.Log("Nouvelle position de la languette : " + transform.position);
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
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
