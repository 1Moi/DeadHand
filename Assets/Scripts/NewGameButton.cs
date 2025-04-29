using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Couleurs")]
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.black;

    [Header("Références Visuelles")]
    public SpriteRenderer spriteRenderer;
    public GameObject Livre;
    public GameObject StartGameButton;
    public GameObject OptionButton;
    public GameObject Fils;
    public GameObject Titre;
    public GameObject optionCanvas;

    [Header("Paramètres de Transition")]
    public Vector3 targetPosition;
    public Quaternion targetRotation;
    public float transitionSpeed = 1f;
    public float fadeDuration = 1f;
    public float slideDuration = 0.5f;
    public float slideTargetX = -25f;
    public float delayBeforeLivreMove = 0.2f; // ⬅ Délai avant que le Livre ne commence à bouger

    [Header("Configuration du Bouton")]
    [SerializeField]
    private bool isNewGameOrOption; // true = NewGame, false = Option

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = defaultColor;

        if (optionCanvas != null)
            optionCanvas.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        spriteRenderer.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spriteRenderer.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        spriteRenderer.color = pressedColor;

        if (isNewGameOrOption)
        {
            StartCoroutine(LaunchNewGameSequence());
        }
        else
        {
            OptionMenu();
        }

        StartCoroutine(ResetAfterDelay(0.1f));
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = defaultColor;
    }

    private void OptionMenu()
    {
        if (optionCanvas != null)
        {
            optionCanvas.SetActive(true);
        }
    }

    private IEnumerator LaunchNewGameSequence()
    {
        // 1. Fade out titre et fils
        SpriteRenderer titreSR = Titre.GetComponent<SpriteRenderer>();
        SpriteRenderer filsSR = Fils.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOutSprite(titreSR));
        StartCoroutine(FadeOutSprite(filsSR));

        // 2. Slide les boutons à gauche
        yield return StartCoroutine(SlideOutButton(StartGameButton));
        yield return StartCoroutine(SlideOutButton(OptionButton));

        // 3. Attendre un délai configurable avant de bouger le livre
        yield return new WaitForSeconds(delayBeforeLivreMove);

        // 4. Bouger le livre
        yield return StartCoroutine(MoveLivre());
    }

    private IEnumerator FadeOutSprite(SpriteRenderer sr)
    {
        if (sr == null) yield break;

        float elapsed = 0f;
        Color startColor = sr.color;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            sr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0f, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    private IEnumerator SlideOutButton(GameObject buttonObj)
    {
        if (buttonObj == null) yield break;

        Transform tr = buttonObj.transform;
        float elapsed = 0f;
        Vector3 startPos = tr.localPosition;
        Vector3 targetPos = new Vector3(slideTargetX, startPos.y, startPos.z);

        while (elapsed < slideDuration)
        {
            float t = elapsed / slideDuration;
            tr.localPosition = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tr.localPosition = targetPos;
    }

    private IEnumerator MoveLivre()
    {
        float elapsed = 0f;
        Vector3 startPos = Livre.transform.position;
        Quaternion startRot = Livre.transform.rotation;
        float duration = 1f / transitionSpeed;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Livre.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            Livre.transform.rotation = Quaternion.Slerp(startRot, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Livre.transform.position = targetPosition;
        Livre.transform.rotation = targetRotation;
    }
}
