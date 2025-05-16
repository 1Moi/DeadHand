using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ButtonType { NewGame, Option, Quit }

    [Header("Type de bouton")]
    public ButtonType buttonType = ButtonType.NewGame;

    [Header("Couleurs")]
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.black;

    [Header("Références Visuelles")]
    public SpriteRenderer spriteRenderer;
    public GameObject Livre;
    public GameObject StartGameButton;
    public GameObject OptionButton;
    public GameObject QuitButton;
    public GameObject Fils;
    public GameObject Titre;
    public GameObject optionCanvas;

    [Header("Paramètres de Transition")]
    public Vector3 targetLocalPosition;
    public Quaternion targetLocalRotation;
    public float transitionSpeed = 1f;
    public float fadeDuration = 1f;
    public float slideDuration = 0.5f;
    public float slideTargetX = -25f;
    public float delayBeforeLivreMove = 0.2f;
    public GameObject ClickBlocker;

    [Header("Colliders à désactiver")]
    public Collider2D[] collidersToDisable;

    [Header("Audio")]
    [SerializeField] private AudioClip[] AudioClick;
    [SerializeField] private float volume;

    private bool isTransitioning = false;

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
        if (isTransitioning) return;
        spriteRenderer.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isTransitioning) return;
        spriteRenderer.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (AudioClick != null && AudioClick.Length > 0)
        {
            int index = Random.Range(0, AudioClick.Length);
            GlobalSoundManager.PlayUI(AudioClick[index]);
        }

        if (isTransitioning) return;
        spriteRenderer.color = pressedColor;

        switch (buttonType)
        {
            case ButtonType.NewGame:
                StartCoroutine(LaunchNewGameSequence());
                break;
            case ButtonType.Option:
                OptionMenu();
                break;
            case ButtonType.Quit:
                QuitGame();
                break;
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

    private void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator LaunchNewGameSequence()
    {
        isTransitioning = true;
        DisableAllColliders();

        SpriteRenderer titreSR = Titre?.GetComponent<SpriteRenderer>();
        SpriteRenderer filsSR = Fils?.GetComponent<SpriteRenderer>();
        if (titreSR) StartCoroutine(FadeOutSprite(titreSR));
        if (filsSR) StartCoroutine(FadeOutSprite(filsSR));

        yield return StartCoroutine(SlideOutButton(StartGameButton));
        yield return StartCoroutine(SlideOutButton(OptionButton));
        yield return StartCoroutine(SlideOutButton(QuitButton)); // ← slide sans fade

        yield return new WaitForSeconds(delayBeforeLivreMove);
        yield return StartCoroutine(MoveLivreLocal());

        isTransitioning = false;
    }

    private void DisableAllColliders()
    {
        foreach (var col in collidersToDisable)
        {
            if (col != null) col.enabled = false;
        }
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

    private IEnumerator MoveLivreLocal()
    {
        float elapsed = 0f;
        Vector3 startPos = Livre.transform.localPosition;
        Quaternion startRot = Livre.transform.localRotation;
        float duration = 1f / transitionSpeed;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Livre.transform.localPosition = Vector3.Lerp(startPos, targetLocalPosition, t);
            Livre.transform.localRotation = Quaternion.Slerp(startRot, targetLocalRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Livre.transform.localPosition = targetLocalPosition;
        Livre.transform.localRotation = targetLocalRotation;

        if (ClickBlocker != null)
            Destroy(ClickBlocker);
    }
}
