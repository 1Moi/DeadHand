using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PuzzleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Couleurs")]
    public Color defaultColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.green;
    public Color errorColor = Color.red;

    [Header("Références")]
    public SpriteRenderer spriteRenderer;
    public PuzzleButtonManager manager;

    [Header("Comportement spécial")]
    public bool isResetButton = false;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = defaultColor;
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
        if (manager == null || manager.IsLocked())
            return;

        spriteRenderer.color = pressedColor;

        if (isResetButton)
        {
            manager.ResetAllButtons();
        }
        else
        {
            manager.RegisterButtonPress(this);
        }

        StartCoroutine(ResetAfterDelay(0.1f));
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = defaultColor;
    }

    public void ResetButton()
    {
        spriteRenderer.color = defaultColor;
    }

    public IEnumerator FlashError()
    {
        spriteRenderer.color = errorColor;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = defaultColor;
    }
}