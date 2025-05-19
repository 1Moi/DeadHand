using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private AudioClip AudioWinKey;

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

        
        [Header("Puzzle Métiers")]
        public List<CharacterSlot> characterSlots;
        public List<int> correctJobIndexes;
        public List<GameObject> comicPanels;
        public GameObject Enviro;

        [Header("Résolution : récompense ou affichage")]
        public bool unlocksKey = true;

        [Header("état du puzzle")]
        public bool isUnlocked = false;
        public bool isSolved = false;
        public bool canCheckIfSolved = true;

        [Header("Contrôle visuel (ex: cadenas)")]
        public GameObject lockVisual;

        [Header("Récompense (clef)")]
        public string unlockKey;

        [Header("script moche")]
        public LanguetteGoToCranCall GoToCranCall;
        private int LanguetteGoToCall = 0;

        [Header("Audio")]
        [SerializeField] private AudioClip[] AudioWin;
        

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

            foreach (var character in characterSlots)
            {
                if (character != null)
                    return;
            }

            foreach (var panel in comicPanels)
            {
                if (panel != null)
                    panel.SetActive(false);
            }
        }

        public bool CheckIfSolved()
        {
            if (!canCheckIfSolved || isSolved)
                return false;
            

            bool isCurrentlySolved = true;

            if (slidingPuzzle != null)
            {
                isCurrentlySolved &= slidingPuzzle.IsPuzzleSolved(correctTilePositions);
                if (isCurrentlySolved && GoToCranCall != null && LanguetteGoToCall == 0)
                { 
                    GoToCranCall.CallLanguetteGotoCran(0);
                    LanguetteGoToCall++;

                    if (AudioWin != null && AudioWin.Length > 0)
                    {
                        int indexAudio = Random.Range(0, AudioWin.Length);
                        GlobalSoundManager.PlaySFX(AudioWin[indexAudio]);
                    }
                }
            }

            for (int i = 0; i < languettePuzzles.Count; i++)
            {
                if (languettePuzzles[i].GetCranIndex() != cransCorrects[i])
                {
                    isCurrentlySolved = false;
                    break;
                }
            }

            for (int i = 0; i < characterSlots.Count; i++)
            {
                if (characterSlots[i].GetCurrentJobIndex() != correctJobIndexes[i])
                {
                    isCurrentlySolved = false;
                    break;
                }
            }

            if (isCurrentlySolved && Enviro != null)
            {
                if (AudioWin != null && AudioWin.Length > 0)
                {
                    int indexAudio = Random.Range(0, AudioWin.Length);
                    GlobalSoundManager.PlaySFX(AudioWin[indexAudio]);
                }

                isSolved = true;
                Debug.Log("Énigme résolue : " + stepName);

                foreach (var slot in characterSlots)
                {
                    if (slot != null)
                    {
                        slot.DeselectFinal();
                    }
                }

                // Désactiver les enfants nommés Case0BW et activer ceux nommés Case0RGB
                foreach (Transform child in Enviro.transform)
                {
                    if (child.name.Contains("Case0BW"))
                        child.gameObject.SetActive(false);
                    else if (child.name.Contains("Case0RGB"))
                        child.gameObject.SetActive(true);
                }
            }

            return isCurrentlySolved;
        }
    }

    [Header("Liste des énigmes")]
    public List<PuzzleStep> puzzleSteps;

    [Header("Liste des clefs obtenues")]
    public Dictionary<string, bool> keysObtained = new Dictionary<string, bool>();

    [Header("Cheat Mode (Playtest seulement)")]
    public bool cheatMode = false;
    public List<string> keysToCheatUnlock;

    [Header("animation de recompense")]
    public RewardAnimator rewardAnimator;

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
                    Debug.LogWarning("Clef débloquée par cheat : " + key);
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
            step.UpdateLockState();

            Debug.LogWarning("Énigme résolue : " + step.stepName);

            if (step.unlocksKey && !string.IsNullOrEmpty(step.unlockKey))
            {
                GlobalSoundManager.PlaySFX(AudioWinKey);

                keysObtained[step.unlockKey] = true;
                Debug.LogWarning("Clef obtenue : " + step.unlockKey);

                if (rewardAnimator != null)
                    rewardAnimator.ShowReward(step.unlockKey);
                else
                    Debug.LogError("RewardAnimator non assigné !");
            }
        }
        else if (step.isUnlocked && !step.isSolved)
        {
            Debug.LogWarning("Énigme non résolue : " + step.stepName);
        }
    }

    public bool HasKey(string key)
    {
        return keysObtained.ContainsKey(key) && keysObtained[key];
    }
}
