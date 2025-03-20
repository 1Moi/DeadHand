using UnityEngine;
using UnityEngine.EventSystems;

public class LockScript : MonoBehaviour, IPointerDownHandler
{
    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;

    [Header("Cl� n�cessaire pour ouvrir")]
    public string requiredKey;

    [Header("PuzzleStep � d�bloquer")]
    public int puzzleStepToUnlock;

    [Header("Objet � d�bloquer (facultatif)")]
    public GameObject objectToUnlock;

    [Header("Page qui peut maintenant tourner")]
    public GameObject pageToTurn;

    private bool isUnlocked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isUnlocked)
        {
            Debug.Log("Cadenas d�j� ouvert.");
            return;
        }

        if (puzzleManager.HasKey(requiredKey))
        {
            isUnlocked = true;

            if (objectToUnlock != null)
                objectToUnlock.SetActive(true);

            if (pageToTurn != null)
                pageToTurn.GetComponent<TurningPages>().canTurnPage = true;

            if (puzzleStepToUnlock >= 0 && puzzleStepToUnlock < puzzleManager.puzzleSteps.Count)
            {
                puzzleManager.puzzleSteps[puzzleStepToUnlock].isUnlocked = true;
                puzzleManager.puzzleSteps[puzzleStepToUnlock].UpdateLockState();
            }

            Debug.Log("Cadenas ouvert avec la cl� : " + requiredKey);

            //delete the gameobject
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Cl� manquante : " + requiredKey);
        }
    }
}
