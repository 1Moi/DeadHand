using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridPattern
{
    public string patternName;
    public List<Vector2Int> cells;
    public bool isActivated = false;    // Le pattern peut-il être détecté ?
    public bool isSolved = false;       // A-t-il déjà été détecté ?
    public bool parfaitement = false;   // Doit-il correspondre parfaitement (pas de cases en trop) ?
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

    [Header("Fade Settings")]
    public FadeManager fadeManager = null;
    public string TexteVika = "Oui Oui";

    public LanguetteCrantee LanguetteCrantee;

    public TurningPages TurningPages;
    public GameObject TurningPageObject;
    public GameObject CLICKBLOCKER;

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

        //Debug.Log("Cellule activée : " + coord);
        CheckPatterns();
        return validators.ContainsKey(coord);
    }

    private void CheckPatterns()
    {
        foreach (var pattern in patterns)
        {
            if (!pattern.isActivated || pattern.isSolved)
                continue;

            bool match = true;

            // 1. Toutes les cellules du pattern doivent être actives
            foreach (var cell in pattern.cells)
            {
                if (!validators.ContainsKey(cell))
                {
                    match = false;
                    break;
                }
            }

            // 2. Si le mode "parfaitement" est activé : il ne doit pas y avoir d'autres cellules actives
            if (match && pattern.parfaitement)
            {
                foreach (var activeCell in validators.Keys)
                {
                    if (!pattern.cells.Contains(activeCell))
                    {
                        match = false;
                        break;
                    }
                }
            }

            if (match)
            {
                pattern.isSolved = true;
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
                Debug.LogWarning("LOSANGE");
                PatternIsSolved(name);

                // activation Fade In / Fade Out
                fadeManager.StartFadeIn(true, TexteVika, 1f, 0f, 1f);
                StartCoroutine(Wait2Seconds(1f));
                // Drop la languette


                ActivatePattern("Coeur");
                break;

            case "Coeur":
                Debug.LogWarning("COEUR");
                PatternIsSolved(name);

                // Tourner la page Automatiquement ICI
                StartCoroutine(Chap3AutoTurnPage1to2());

                break;

            case "BLOODBOUND!!!!!!!!":
                Debug.LogWarning("BLOODBOUND!!!!!!!!!");
                break;

            default:
                Debug.LogWarning("Pattern inconnu : " + name);
                break;
        }
    }

    public void ActivatePattern(string name)
    {
        GridPattern pattern = patterns.Find(p => p.patternName == name);
        if (pattern == null) return;

        pattern.isActivated = true;
        //Debug.Log($"Pattern {name} autorisé pour vérification.");
    }

    public void PatternIsSolved(string name)
    {
        GridPattern pattern = patterns.Find(p => p.patternName == name);
        if (pattern == null) return;

        pattern.isSolved = true;
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

    public void ClearGrid()
    {
        foreach (var kvp in validators)
        {
            if (kvp.Value != null)
                Destroy(kvp.Value);
        }

        validators.Clear();
        Debug.Log(" Grille nettoyée !");
    }

    public IEnumerator Chap3AutoTurnPage1to2()
    {
        CLICKBLOCKER.SetActive(true);
        TurningPageObject.SetActive(true);
        TurningPages.TurningPage();
        yield return new WaitForSeconds(2f);
        CLICKBLOCKER.SetActive(false);
    }

    public IEnumerator Wait2Seconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        LanguetteCrantee.GoToCran(1);
    }
}
