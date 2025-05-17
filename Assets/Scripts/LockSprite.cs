using Unity.VisualScripting;
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
    [Tooltip("true : active, false : desactive")]
    public bool setActiveFalseOrTrue = true;

    [Header("Page qui peut maintenant tourner")]
    public GameObject pageToTurn;

    [Header("Audio")]
    [SerializeField] private AudioClip AudioLock;
    [SerializeField] private AudioClip AudioOpen;

    private bool isUnlocked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Cadenas touché : " + gameObject.name);
        if (isUnlocked)
        {
            Debug.Log("Cadenas déjà ouvert.");
            return;
        }

        if (puzzleManager.HasKey(requiredKey))
        {
            isUnlocked = true;
            if (AudioOpen != null)
                GlobalSoundManager.PlaySFX(AudioOpen);

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

            if (objectToUnlock != null)
                objectToUnlock.SetActive(false);

            //delete the gameobject
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Clé manquante : " + requiredKey);
            if (AudioLock != null)
                GlobalSoundManager.PlaySFX(AudioLock);
        }
    }
}
