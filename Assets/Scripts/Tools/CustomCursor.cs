using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    private Camera mainCamera;

    [Tooltip("Distance entre la caméra et le curseur dans la scène")]
    public float cursorZ = 0f;

    [Tooltip("Décalage optionnel pour affiner le point actif")]
    public Vector2 offset = Vector2.zero;

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z) + cursorZ;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(
            worldPos.x + offset.x,
            worldPos.y + offset.y,
            cursorZ
        );
    }
}
