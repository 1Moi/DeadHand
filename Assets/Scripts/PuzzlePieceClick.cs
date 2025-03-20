using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePieceClick : MonoBehaviour, IPointerDownHandler
{
    public SlidingPuzzle puzzleManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        puzzleManager.OnPieceClicked(transform);
    }

    void Start()
    {
        puzzleManager = FindObjectOfType<SlidingPuzzle>();
    }
}
