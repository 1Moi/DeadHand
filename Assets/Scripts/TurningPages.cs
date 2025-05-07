using UnityEngine;
using UnityEngine.EventSystems;


public class TurningPages : MonoBehaviour, IPointerDownHandler
{
    [Header("Can turn the page")]
    public bool canTurnPage = false;
    public AutoFlip autoFlip;

    [Header("Camera")]
    public bool NextOrPrevious = false;

    [Header("Audio")]
    [SerializeField] private AudioClip[] audioPage;
    [SerializeField] private float volume;

    public Camera pageCamera;
    public float pageTurnDistance = 100f; // Distance du d�placement

    private int direction = -1;

    private bool hovering = false;

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
            if (audioPage.Length > 0)
            {
                int index = Random.Range(0, audioPage.Length);
                GlobalSoundManager.PlaySound(audioPage[index], volume);
            }
        }
        else
        {
            Debug.LogWarning("Impossible de tourner la page.");
        }
    }

    void OnMouseOver()
    {
        if (hovering == false)
        {
            hovering = true;
            transform.localScale = transform.localScale + new Vector3(0.25f * direction, 0.25f, 0);
            transform.position = transform.position + new Vector3(-0.65f * direction, 1, 0);
        }

    }
    void OnMouseExit()
    {
        if (hovering == true)
        {
            hovering = false;
            transform.localScale = transform.localScale + new Vector3(-0.25f * direction, -0.25f, 0);
            transform.position = transform.position + new Vector3(0.65f * direction, -1, 0);
            
        }
        
    }
}
