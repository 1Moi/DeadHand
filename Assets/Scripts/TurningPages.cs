using UnityEngine;
using UnityEngine.EventSystems;

public class TurningPages : MonoBehaviour, IPointerDownHandler
{
    [Header("Can turn the page")]
    public bool canTurnPage = false;

    [Header("Camera")]
    public bool NextOrPrevious = false;
    public Camera pageCamera;
    public float pageTurnDistance = 100f; // Distance du déplacement

    private int direction = -1;

    void Start()
    {
        direction = NextOrPrevious ? 1 : -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canTurnPage)
        {
            pageCamera.transform.position += new Vector3(pageTurnDistance * direction, 0, 0);
            Debug.Log("Page tournée.");
        }
        else
        {
            Debug.LogWarning("Impossible de tourner la page.");
        }
    }
}
