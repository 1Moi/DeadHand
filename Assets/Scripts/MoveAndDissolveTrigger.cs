using System.Collections;
using UnityEngine;

public class MoveAndDissolveTrigger : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 2f;
    public float rotationSpeed = 180f; // degrés par seconde

    public DissolveEffectSergei dissolveScript;


    private void Update()
    {
        
    }

    public IEnumerator MoveThenDissolve()
    {
        while (
            Vector3.Distance(transform.position, targetPosition.position) > 0.01f ||
            Quaternion.Angle(transform.rotation, targetPosition.rotation) > 0.1f
        )
        {
            // Position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                moveSpeed * Time.deltaTime
            );

            // Rotation progressive
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetPosition.rotation,
                rotationSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Correction finale
        transform.position = targetPosition.position;
        transform.rotation = targetPosition.rotation;

        Debug.Log("Déplacement terminé. Activation du dissolve.");
        dissolveScript.IsDissolvingSergei = true;
    }
}
