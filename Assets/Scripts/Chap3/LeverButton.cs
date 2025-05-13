using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LeverButton : MonoBehaviour, IPointerDownHandler
{
    public Sprite leverUp;
    public Sprite leverDown;
    public float downDuration = 0.3f;

    public SpriteRenderer spriteRenderer;
    public GridManager gridManager;

    private bool isAnimating = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isAnimating || spriteRenderer == null || gridManager == null) return;

        StartCoroutine(LeverAnimation());

        bool activated = gridManager.ToggleValidator();
        Debug.Log(activated ? "Case validée" : "Case désactivée");
    }

    private IEnumerator LeverAnimation()
    {
        isAnimating = true;
        spriteRenderer.sprite = leverDown;
        yield return new WaitForSeconds(downDuration);
        spriteRenderer.sprite = leverUp;
        isAnimating = false;
    }
}
