using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleResetTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SlidingPuzzle targetPuzzle;
    public float hoverScale = 1.1f;
    public float tiltAmount = 10f;
    public float transitionSpeed = 5f;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isHovering)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 localMouse = transform.InverseTransformPoint(mouseWorld);
            float x = Mathf.Clamp(localMouse.x, -1f, 1f);
            float angleZ = -x * tiltAmount;

            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScale, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angleZ), Time.deltaTime * transitionSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (targetPuzzle != null)
        {
            targetPuzzle.ResetPuzzle();
        }
        else
        {
            Debug.LogWarning("Aucun SlidingPuzzle assigné au PuzzleResetTrigger !");
        }
    }
}
