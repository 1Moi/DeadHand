using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;

public class ResetButton : MonoBehaviour, IPointerDownHandler
{
    public GridManager gridManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        gridManager.ClearGrid();
    }
}
