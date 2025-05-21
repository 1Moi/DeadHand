using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DragWithLimits : MonoBehaviour, IPointerDownHandler
{
    private bool isDragging = false;
    private Camera mainCamera;

    // Limites du bureau (en coordonnées monde)
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 18f; // distance entre cam -18 et ton sprite
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = transform.position.z;

            // Si la position est dans les limites autorisées, on déplace
            if (IsWithinBounds(worldPos))
            {
                transform.position = worldPos;
            }
            else
            {
                Debug.Log("Sorti de la zone -> Drop forcé !");
                isDragging = false; // Forcer le lâcher
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    private bool IsWithinBounds(Vector3 pos)
    {
        return pos.x >= minX && pos.x <= maxX && pos.y >= minY && pos.y <= maxY;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f), new Vector3(maxX - minX, maxY - minY, 0.1f));
    }

}
