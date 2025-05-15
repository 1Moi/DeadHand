using UnityEngine;

public class CameraOutroMovement : MonoBehaviour
{
    public Transform[] points;
    public float transitionDuration = 5f;
    public GameObject outroUI; // UI à activer
    public Vector3 slideOffset = new Vector3(0, -200f, 0); // Slide depuis le bas
    public float uiFadeDuration = 1.5f;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool isMoving = true;

    private CanvasGroup uiGroup;
    private RectTransform uiRect;
    private Vector3 uiStartPos;
    private Vector3 uiTargetPos;
    private float uiTimer = 0f;
    private bool uiAppearing = false;

    void Start()
    {
        if (outroUI != null)
        {
            outroUI.SetActive(false);
            uiGroup = outroUI.GetComponent<CanvasGroup>();
            uiRect = outroUI.GetComponent<RectTransform>();

            if (uiGroup != null && uiRect != null)
            {
                uiTargetPos = uiRect.anchoredPosition;
                uiStartPos = uiTargetPos + slideOffset;
                uiRect.anchoredPosition = uiStartPos;
                uiGroup.alpha = 0f;
            }
        }
    }

    void Update()
    {
        // Mouvement de la caméra
        if (isMoving && currentIndex < points.Length - 1)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionDuration);
            t = t * t * (3f - 2f * t); // easing

            transform.position = Vector3.Lerp(points[currentIndex].position, points[currentIndex + 1].position, t);

            if (t >= 1f)
            {
                currentIndex++;
                timer = 0f;

                if (currentIndex >= points.Length - 1)
                {
                    isMoving = false;
                    StartUIAnimation();
                }
            }
        }

        // Animation UI
        if (uiAppearing && uiGroup != null && uiRect != null)
        {
            uiTimer += Time.deltaTime;
            float t = Mathf.Clamp01(uiTimer / uiFadeDuration);
            t = t * t * (3f - 2f * t); // easing

            uiGroup.alpha = t;
            uiRect.anchoredPosition = Vector3.Lerp(uiStartPos, uiTargetPos, t);

            if (t >= 1f)
                uiAppearing = false;
        }
    }

    void StartUIAnimation()
    {
        if (outroUI != null)
        {
            outroUI.SetActive(true);
            uiAppearing = true;
            uiTimer = 0f;
        }
    }
}
