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
    public GameObject validationPrefab; // Ce que tu veux instancier
    public Transform validationContainer; // Optionnel : pour organiser la hiérarchie

    public void OnPointerDown(PointerEventData eventData)
    {
        if (spriteRenderer == null) return;

        StopAllCoroutines();
        StartCoroutine(LeverAnimation());

        Vector3 pos = gridManager.GetCurrentCellWorldPosition();
        Instantiate(validationPrefab, pos, Quaternion.identity, validationContainer);
    }

    private IEnumerator LeverAnimation()
    {
        spriteRenderer.sprite = leverDown;
        yield return new WaitForSeconds(downDuration);
        spriteRenderer.sprite = leverUp;
    }
}
