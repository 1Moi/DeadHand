using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PuzzlePieceHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SlidingPuzzle puzzle;
    private LineRenderer lineRenderer;

    [Header("Paramètres Glow")]
    public Color defaultGlowColor = Color.red;
    public Color directionGlowColor = Color.green;
    public float outlineWidth = 0.05f;

    void Start()
    {
        puzzle = FindObjectOfType<SlidingPuzzle>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.Log("Ajout du LineRenderer sur " + gameObject.name);
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        SetupLineRenderer();
        SetOutlineActive(false);
    }


    void SetupLineRenderer()
    {
        lineRenderer.positionCount = 5;  // 4 coins + retour au premier
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = outlineWidth;
        lineRenderer.useWorldSpace = false;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Assurer un matériau visible

        lineRenderer.startColor = defaultGlowColor;
        lineRenderer.endColor = defaultGlowColor;

        UpdateOutlinePositions();
        lineRenderer.enabled = true; // FORCE L'ACTIVATION AU DÉMARRAGE
    }

    void UpdateOutlinePositions()
    {
        float halfSize = 0.5f; // Ajuste en fonction de la taille des tuiles

        Vector3[] positions = new Vector3[]
        {
            new Vector3(-halfSize, halfSize, 0),  // Haut gauche
            new Vector3(halfSize, halfSize, 0),   // Haut droit
            new Vector3(halfSize, -halfSize, 0),  // Bas droit
            new Vector3(-halfSize, -halfSize, 0), // Bas gauche
            new Vector3(-halfSize, halfSize, 0)   // Retour au début
        };

        lineRenderer.SetPositions(positions);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2Int tilePos = puzzle.GetGridPosition(transform);
        List<Vector2Int> emptyAdjacents = puzzle.GetAllAdjacentEmptySlots(tilePos);

        if (emptyAdjacents.Count > 0)
        {
            SetOutlineActive(true);
            HighlightDirection(tilePos, emptyAdjacents);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetOutlineActive(false);
    }

    void SetOutlineActive(bool active)
    {
        lineRenderer.enabled = active;
        if (active)
        {
            lineRenderer.startColor = defaultGlowColor;
            lineRenderer.endColor = defaultGlowColor;
        }
    }

    void HighlightDirection(Vector2Int tilePos, List<Vector2Int> emptyAdjacents)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int closestEmptySlot = puzzle.GetClosestEmptySlot(mouseWorldPos, emptyAdjacents);

        Vector2Int dir = closestEmptySlot - tilePos;

        Color[] colors = new Color[5] { defaultGlowColor, defaultGlowColor, defaultGlowColor, defaultGlowColor, defaultGlowColor };

        if (dir == Vector2Int.up) colors[0] = colors[1] = directionGlowColor;
        else if (dir == Vector2Int.right) colors[1] = colors[2] = directionGlowColor;
        else if (dir == Vector2Int.down) colors[2] = colors[3] = directionGlowColor;
        else if (dir == Vector2Int.left) colors[3] = colors[0] = directionGlowColor;

        lineRenderer.startColor = colors[0];
        lineRenderer.endColor = colors[4]; // Retour à la couleur initiale
    }
}
