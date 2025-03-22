using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class GridPuzzle : MonoBehaviour
{
    [Header("Grille")]
    public int rows = 3;
    public int columns = 3;
    public float tileSize = 1f;

    [Header("Prefab")]
    public GameObject emptySlotPrefab;

    [Header("Pièces")]
    public List<Transform> puzzlePieces;
    public List<Vector2Int> initialTilePositions;
    public List<Vector2Int> emptySlots;

    private Transform[,] grid;
    public float moveDuration = 0.2f;
    private Dictionary<Vector2Int, GameObject> slotVisuals = new();
    private Transform selectedPiece;

    void Start()
    {
        SetupGrid();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 pos = transform.position + new Vector3(x * tileSize, -y * tileSize, 0);
                Gizmos.DrawWireCube(pos, new Vector3(tileSize, tileSize, 0));
            }
        }
    }

    IEnumerator MovePieceSmooth(Transform piece, Vector2Int targetPos)
    {
        Vector3 startPos = piece.position;
        Vector3 endPos = GetWorldPosition(targetPos);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            piece.position = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.position = endPos;
    }

    void SetupGrid()
    {
        grid = new Transform[rows, columns];

        for (int i = 0; i < puzzlePieces.Count && i < initialTilePositions.Count; i++)
        {
            Vector2Int pos = initialTilePositions[i];
            Transform piece = puzzlePieces[i];
            grid[pos.y, pos.x] = piece;
            piece.position = GetWorldPosition(pos);

            if (!piece.GetComponent<BoxCollider2D>())
                piece.gameObject.AddComponent<BoxCollider2D>();

            if (!piece.GetComponent<GridPieceClick>())
                piece.gameObject.AddComponent<GridPieceClick>();

            piece.GetComponent<GridPieceClick>().puzzle = this;
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                GameObject slot = Instantiate(emptySlotPrefab, GetWorldPosition(gridPos), Quaternion.identity, transform);
                slot.name = "Slot_" + x + "_" + y;

                if (!slot.GetComponent<BoxCollider2D>())
                    slot.AddComponent<BoxCollider2D>();

                if (!slot.GetComponent<GridSlotClick>())
                    slot.AddComponent<GridSlotClick>();

                GridSlotClick slotClick = slot.GetComponent<GridSlotClick>();
                slotClick.puzzle = this;
                slotClick.slotPosition = gridPos;

                slotVisuals[gridPos] = slot;
            }
        }
    }

    public void OnPieceClicked(Transform piece)
    {
        selectedPiece = piece;
    }

    public void OnSlotClicked(Vector2Int slotPos)
    {
        if (selectedPiece == null || !emptySlots.Contains(slotPos)) return;

        Vector2Int piecePos = GetGridPosition(selectedPiece);

        grid[slotPos.y, slotPos.x] = selectedPiece;
        grid[piecePos.y, piecePos.x] = null;

        StartCoroutine(MovePieceSmooth(selectedPiece, slotPos));

        emptySlots.Remove(slotPos);
        emptySlots.Add(piecePos);

        selectedPiece = null;
    }

    Vector2Int GetGridPosition(Transform piece)
    {
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < columns; x++)
                if (grid[y, x] == piece)
                    return new Vector2Int(x, y);

        return new Vector2Int(-1, -1);
    }

    Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return transform.position + new Vector3(gridPos.x * tileSize, -gridPos.y * tileSize, 0);
    }
}

public class GridPieceClick : MonoBehaviour, IPointerDownHandler
{
    public GridPuzzle puzzle;

    public void OnPointerDown(PointerEventData eventData)
    {
        puzzle.OnPieceClicked(transform);
    }
}

public class GridSlotClick : MonoBehaviour, IPointerDownHandler
{
    public GridPuzzle puzzle;
    public Vector2Int slotPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        puzzle.OnSlotClicked(slotPosition);
    }
}
