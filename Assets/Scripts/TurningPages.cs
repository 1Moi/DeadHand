using UnityEngine;
using UnityEngine.EventSystems;

public class TurningPages : MonoBehaviour, IPointerDownHandler
{
    [Header("Can turn the page")]
    public bool canTurnPage = false;
    public AutoFlip autoFlip;

    [Header("Camera")]
    public bool NextOrPrevious = false;

    [SerializeField] private AudioClip audioPage;
    [SerializeField] private float volume;

    public Camera pageCamera;
    public float pageTurnDistance = 100f; // Distance du d�placement

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
            Debug.Log("Page tourn�e.");

            if (direction > 0)
                autoFlip.FlipRightPage();
            else
                autoFlip.FlipLeftPage();
            GlobalSoundManager.PlaySound(audioPage, volume); 
        }
        else
        {
            Debug.LogWarning("Impossible de tourner la page.");
        }
    }
}
