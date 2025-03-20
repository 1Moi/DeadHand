using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlidingPuzzle : MonoBehaviour
{
    [Header("Paramètres de la grille")]
    public int rows = 3;
    public int columns = 3;
    public float tileSize = 1f;

    [Header("Paramètres d'interaction")]
    public bool canMovePieces = true;

    [Header("Paramètres visuels")]
    public float moveDuration = 0.2f;

    [Header("Pièces du puzzle")]
    public List<Transform> puzzlePieces;

    [Header("Positions initiales des tuiles")]
    public List<Vector2Int> initialTilePositions;

    [Header("Cases vides initiales")]
    public List<Vector2Int> emptySlots;

    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;
    public int puzzleStepIndex;

    private Transform[,] grid;

    void Start()
    {
        SetupGrid();
    }

    void SetupGrid()
    {
        grid = new Transform[rows, columns];

        for (int i = 0; i < puzzlePieces.Count && i < initialTilePositions.Count; i++)
        {
            Vector2Int position = initialTilePositions[i];
            Transform piece = puzzlePieces[i];

            if (!emptySlots.Contains(position))
            {
                grid[position.y, position.x] = piece;
                piece.position = GetWorldPosition(position);

                if (!piece.GetComponent<BoxCollider2D>())
                    piece.gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

    public void OnPieceClicked(Transform piece)
    {
        if (!canMovePieces) return;

        Vector2Int pieceGridPos = GetGridPosition(piece);
        List<Vector2Int> emptyAdjacents = GetAllAdjacentEmptySlots(pieceGridPos);

        if (emptyAdjacents.Count > 0)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int targetPos = GetClosestEmptySlot(mouseWorldPos, emptyAdjacents);

            StartCoroutine(MovePieceSmooth(pieceGridPos, piece, targetPos));
        }
    }

    IEnumerator MovePieceSmooth(Vector2Int startPos, Transform piece, Vector2Int targetPos)
    {
        grid[targetPos.y, targetPos.x] = piece;
        grid[startPos.y, startPos.x] = null;

        Vector3 startWorldPos = piece.position;
        Vector3 endWorldPos = GetWorldPosition(targetPos);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            piece.position = Vector3.Lerp(startWorldPos, endWorldPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.position = endWorldPos;

        emptySlots.Remove(targetPos);
        emptySlots.Add(startPos);

        if (puzzleManager != null)
            puzzleManager.CheckSinglePuzzle(puzzleStepIndex);
    }

    public Vector2Int GetGridPosition(Transform piece)
    {
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < columns; x++)
                if (grid[y, x] == piece)
                    return new Vector2Int(x, y);

        return new Vector2Int(-1, -1);
    }

    public List<Vector2Int> GetAllAdjacentEmptySlots(Vector2Int pos)
    {
        List<Vector2Int> emptyAdjacents = new List<Vector2Int>();

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPos = pos + dir;
            if (emptySlots.Contains(adjacentPos))
                emptyAdjacents.Add(adjacentPos);
        }

        return emptyAdjacents;
    }

    public Vector2Int GetClosestEmptySlot(Vector2 worldPosition, List<Vector2Int> candidates)
    {
        Vector2Int closestSlot = candidates[0];
        float minDist = Vector2.Distance(worldPosition, GetWorldPosition(closestSlot));

        foreach (var slot in candidates)
        {
            float dist = Vector2.Distance(worldPosition, GetWorldPosition(slot));
            if (dist < minDist)
            {
                minDist = dist;
                closestSlot = slot;
            }
        }
        return closestSlot;
    }

    public bool IsPuzzleSolved(List<Vector2Int> correctPositions)
    {
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            Vector2Int currentGridPos = GetGridPosition(puzzlePieces[i]);

            if (currentGridPos != correctPositions[i])
                return false;
        }
        return true;
    }

    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        return transform.position + new Vector3(gridPos.x * tileSize, -gridPos.y * tileSize, 0);
    }
}
