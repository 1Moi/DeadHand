using UnityEngine;
using UnityEngine.EventSystems;

public class LockScript : MonoBehaviour, IPointerDownHandler
{
    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;

    [Header("Clé nécessaire pour ouvrir")]
    public string requiredKey;

    [Header("PuzzleStep à débloquer")]
    public int puzzleStepToUnlock;

    [Header("Objet à débloquer (facultatif)")]
    public GameObject objectToUnlock;

    [Header("Page qui peut maintenant tourner")]
    public GameObject pageToTurn;

    private bool isUnlocked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isUnlocked)
        {
            Debug.Log("Cadenas déjà ouvert.");
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

            Debug.Log("Cadenas ouvert avec la clé : " + requiredKey);

            //delete the gameobject
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Clé manquante : " + requiredKey);
        }
    }
}
