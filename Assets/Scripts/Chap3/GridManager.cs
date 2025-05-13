using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GridPattern
{
    public string patternName;
    public List<Vector2Int> cells;
    public bool isActivated = false;
}

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 12;
    public int rows = 6;
    public float cellWidth = 1f;
    public float cellHeight = 1f;
    public Vector2 origin = Vector2.zero;

    [Header("Prefabs & Objects")]
    public Transform selectorPrefab;
    public GameObject validationPrefab;
    public Transform validationContainer;

    [Header("Patterns")]
    public List<GridPattern> patterns = new();

    [Header("Debug")]
    public bool showGridGizmos = true;

    private Transform selector;
    private int currentX = 0;
    private int currentY = 0;

    private Dictionary<Vector2Int, GameObject> validators = new();

    private void Start()
    {
        currentX = 0;
        currentY = 0;

        Vector3 startPos = GetCurrentCellWorldPosition();

        if (selectorPrefab != null)
            selector = Instantiate(selectorPrefab, startPos, Quaternion.identity, transform);

        UpdateSelectorPosition();
    }

    public void MoveSelector(int deltaX, int deltaY)
    {
        currentX = (currentX + deltaX + columns) % columns;
        currentY = (currentY + deltaY + rows) % rows;
        UpdateSelectorPosition();
    }

    public void UpdateSelectorPosition()
    {
        if (selector)
            selector.position = GetCurrentCellWorldPosition();
    }

    public Vector3 GetCurrentCellWorldPosition()
    {
        float offsetX = cellWidth / 2f;
        float offsetY = cellHeight / 2f;

        return new Vector3(
            origin.x + currentX * cellWidth + offsetX,
            origin.y + (rows - 1 - currentY) * cellHeight + offsetY,
            0
        );
    }

    public Vector2Int GetCurrentCellCoord() => new(currentX, currentY);

    public bool ToggleValidator()
    {
        Vector2Int coord = GetCurrentCellCoord();

        if (validators.ContainsKey(coord))
        {
            Destroy(validators[coord]);
            validators.Remove(coord);
        }
        else
        {
            Vector3 pos = GetCurrentCellWorldPosition();
            GameObject go = Instantiate(validationPrefab, pos, Quaternion.identity, validationContainer);
            validators[coord] = go;
        }

        CheckPatterns();
        return validators.ContainsKey(coord);
    }

    private void CheckPatterns()
    {
        foreach (var pattern in patterns)
        {
            if (pattern.isActivated) continue;

            bool match = true;
            foreach (var cell in pattern.cells)
            {
                if (!validators.ContainsKey(cell))
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                pattern.isActivated = true;
                HandlePatternActivation(pattern.patternName);
            }
        }
    }

    private void HandlePatternActivation(string name)
    {
        Debug.Log($"Pattern détecté : {name}");

        switch (name)
        {
            case "Losange":
                Debug.Log("Ligne du haut complétée !");
                ActivatePattern("Coeur");
                break;

            case "Coeur":
                Debug.Log("Croix au centre activée !");
                break;

            default:
                Debug.LogWarning("Pattern inconnu : " + name);
                break;
        }
    }

    public void ActivatePattern(string name)
    {
        GridPattern pattern = patterns.Find(p => p.patternName == name);
        if (pattern == null || pattern.isActivated) return;

        foreach (var cell in pattern.cells)
        {
            if (!validators.ContainsKey(cell))
            {
                Vector3 pos = new Vector3(
                    origin.x + cell.x * cellWidth + cellWidth / 2f,
                    origin.y + (rows - 1 - cell.y) * cellHeight + cellHeight / 2f,
                    0
                );

                GameObject go = Instantiate(validationPrefab, pos, Quaternion.identity, validationContainer);
                validators[cell] = go;
            }
        }

        pattern.isActivated = true;
        HandlePatternActivation(name); // peut déclencher une cascade
    }

    private void OnDrawGizmos()
    {
        if (!showGridGizmos) return;

        Gizmos.color = Color.green;

        for (int y = 0; y <= rows; y++)
        {
            float yPos = origin.y + y * cellHeight;
            Vector3 from = new Vector3(origin.x, yPos, 0);
            Vector3 to = new Vector3(origin.x + columns * cellWidth, yPos, 0);
            Gizmos.DrawLine(from, to);
        }

        for (int x = 0; x <= columns; x++)
        {
            float xPos = origin.x + x * cellWidth;
            Vector3 from = new Vector3(xPos, origin.y, 0);
            Vector3 to = new Vector3(xPos, origin.y + rows * cellHeight, 0);
            Gizmos.DrawLine(from, to);
        }
    }
}
