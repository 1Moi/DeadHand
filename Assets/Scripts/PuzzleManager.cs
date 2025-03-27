using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleStep
    {
        public string stepName;

        [Header("Puzzle Taquin")]
        public SlidingPuzzle slidingPuzzle;
        public List<Vector2Int> correctTilePositions;

        [Header("Puzzle Languettes")]
        public List<LanguetteCrantee> languettePuzzles;
        public List<int> cransCorrects;

        [Header("�tat du puzzle")]
        public bool isUnlocked = false;
        public bool isSolved = false;
        public bool canCheckIfSolved = true;

        [Header("Contr�le visuel (ex: cadenas)")]
        public GameObject lockVisual;

        [Header("R�compense (cl�)")]
        public string unlockKey;


        public void UpdateLockState()
        {
            if (lockVisual != null)
                lockVisual.SetActive(!isUnlocked);

            if (slidingPuzzle != null)
                slidingPuzzle.canMovePieces = isUnlocked;

            foreach (var languettePuzzle in languettePuzzles)
            {
                if (languettePuzzle != null)
                    languettePuzzle.enabled = isUnlocked;
            }
        }

        public bool CheckIfSolved()
        {
            if (!canCheckIfSolved || isSolved)
                return false;

            bool isCurrentlySolved = true;

            if (slidingPuzzle != null)
                isCurrentlySolved &= slidingPuzzle.IsPuzzleSolved(correctTilePositions);

            for (int i = 0; i < languettePuzzles.Count; i++)
            {
                if (languettePuzzles[i].GetCranIndex() != cransCorrects[i])
                {
                    isCurrentlySolved = false;
                    break;
                }
            }

            if (isCurrentlySolved)
            {
                isSolved = true;
                Debug.Log("Enigme r�solue : " + stepName);
            }

            return isCurrentlySolved;
        }
    }

    [Header("Liste des enigmes")]
    public List<PuzzleStep> puzzleSteps;

    [Header("Liste des cles obtenues")]
    public Dictionary<string, bool> keysObtained = new Dictionary<string, bool>();
    public GameObject RewardAnimation;

    [Header("Cheat Mode (Playtest seulement)")]
    public bool cheatMode = false;
    public List<string> keysToCheatUnlock;
    

    void Start()
    {
        foreach (var step in puzzleSteps)
            step.UpdateLockState();

        if (cheatMode)
        {
            foreach (var key in keysToCheatUnlock)
            {
                if (!keysObtained.ContainsKey(key))
                {
                    keysObtained[key] = true;
                    Debug.LogWarning("Cl� d�bloqu�e par cheat : " + key);
                }
            }
        }
    }

    public void CheckSinglePuzzle(int puzzleStepIndex)
    {

        if (puzzleStepIndex < 0 || puzzleStepIndex >= puzzleSteps.Count)
        {
            Debug.LogError("Index du puzzle step invalide.");
            return;
        }

        PuzzleStep step = puzzleSteps[puzzleStepIndex];
        Debug.Log("Vérification de l'énigme " + puzzleStepIndex);

        if (step.isUnlocked && !step.isSolved && step.CheckIfSolved())
        {
            //UnlockNextPuzzle(step);
            step.UpdateLockState();

            Debug.LogWarning("énigme résolue : " + step.stepName);

            if (!string.IsNullOrEmpty(step.unlockKey))
            {
                keysObtained[step.unlockKey] = true;
                RewardAnimation.SetActive(true);
                Debug.LogWarning("Clé obtenue : " + step.unlockKey);
            }
        }
        else if(step.isUnlocked && !step.isSolved)
        {
            Debug.LogWarning("énigme non résolue : " + step.stepName);
        }
    }

    /*
    void UnlockNextPuzzle(PuzzleStep solvedStep)
    {
        int currentIndex = puzzleSteps.IndexOf(solvedStep);
        if (currentIndex + 1 < puzzleSteps.Count)
        {
            puzzleSteps[currentIndex + 1].isUnlocked = true;
            puzzleSteps[currentIndex + 1].UpdateLockState();
        }
    }
    */

    public bool HasKey(string key)
    {
        return keysObtained.ContainsKey(key) && keysObtained[key];
    }
}
