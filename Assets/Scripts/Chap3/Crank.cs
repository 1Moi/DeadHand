using UnityEngine;
using UnityEngine.EventSystems;

public class Crank : MonoBehaviour, IPointerDownHandler
{
    public enum CrankType { Horizontal, Vertical }
    public CrankType crankDirection;
    public bool inverse = false;
    public float rotationAmount = 15f;

    public GridManager gridManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        int deltaX = 0, deltaY = 0;

        switch (crankDirection)
        {
            case CrankType.Horizontal:
                deltaX = inverse ? -1 : 1;
                break;
            case CrankType.Vertical:
                deltaY = inverse ? -1 : 1;
                break;
        }

        transform.Rotate(Vector3.forward, rotationAmount * (inverse ? -1 : 1));
        gridManager.MoveSelector(deltaX, deltaY);
    }
}
