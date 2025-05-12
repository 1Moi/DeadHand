using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int columns = 12;
    public int rows = 6;
    public float cellWidth = 1f;
    public float cellHeight = 1f;
    public Vector2 origin = Vector2.zero;

    public Transform selector;
    public Transform indicator;

    [Header("Debug")]
    public bool showGridGizmos = true;

    private int currentX = 0;
    private int currentY = 0;

    private void Start()
    {
        UpdateSelectorPosition();
    }

    public void MoveSelector(int deltaX, int deltaY)
    {
        currentX = Mathf.Clamp(currentX + deltaX, 0, columns - 1);
        currentY = Mathf.Clamp(currentY + deltaY, 0, rows - 1);
        UpdateSelectorPosition();
    }

    private void UpdateSelectorPosition()
    {
        Vector3 pos = new Vector3(origin.x + currentX * cellWidth, origin.y + currentY * cellHeight, 0);
        if (selector) selector.position = pos;
        if (indicator) indicator.position = pos;
    }

    public Vector3 GetCurrentCellWorldPosition()
    {
        return new Vector3(origin.x + currentX * cellWidth, origin.y + currentY * cellHeight, 0f);
    }

    private void OnDrawGizmos()
    {
        if (!showGridGizmos) return;

        Gizmos.color = Color.green;
        for (int y = 0; y <= rows; y++)
        {
            Vector3 from = new Vector3(origin.x, origin.y + y * cellHeight, 0);
            Vector3 to = new Vector3(origin.x + columns * cellWidth, origin.y + y * cellHeight, 0);
            Gizmos.DrawLine(from, to);
        }

        for (int x = 0; x <= columns; x++)
        {
            Vector3 from = new Vector3(origin.x + x * cellWidth, origin.y, 0);
            Vector3 to = new Vector3(origin.x + x * cellWidth, origin.y + rows * cellHeight, 0);
            Gizmos.DrawLine(from, to);
        }
    }
}
