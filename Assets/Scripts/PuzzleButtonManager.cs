using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PuzzleButtonSequence
{
    public string sequenceName;
    public bool isActive = true;
    public bool isSolved = false;
    public List<PuzzleButton> buttons = new List<PuzzleButton>();
}

public class PuzzleButtonManager : MonoBehaviour
{
    [Header("Séquences des puzzles")]
    public List<PuzzleButtonSequence> sequencesInspector = new List<PuzzleButtonSequence>();

    private List<List<PuzzleButton>> allSequences = new List<List<PuzzleButton>>();
    private List<PuzzleButtonSequence> sequences = new List<PuzzleButtonSequence>();
    private List<PuzzleButton> currentInput = new List<PuzzleButton>();
    private int currentSequenceIndex = 0;
    private bool lastAttemptWasCorrect = false;
    private bool isLocked = false;

    public GameObject ClickBlocker;
    public LeaveCase LeaveCase;
    public GameObject ButtonPage;
    public TurningPages TurningPages1;
    public TurningPages TurningPages2;
    public GameObject Caneva;

    void Awake()
    {
        allSequences.Clear();
        sequences.Clear();

        foreach (var seq in sequencesInspector)
        {
            foreach (var btn in seq.buttons)
            {
                btn.manager = this;
            }
            allSequences.Add(seq.buttons);
            sequences.Add(seq);
        }

        SetActiveSequence(0); // Initialise avec la première séquence
    }

    public void SetActiveSequence(int index)
    {
        if (index >= 0 && index < sequences.Count)
        {
            if (!sequences[index].isActive || sequences[index].isSolved)
            {
                Debug.LogWarning("La séquence " + sequences[index].sequenceName + " est inactive ou déjà résolue.");
                return;
            }

            ResetAllButtons();
            currentSequenceIndex = index;
            currentInput.Clear();
            lastAttemptWasCorrect = false;
            isLocked = false;
        }
        else
        {
            Debug.LogWarning("Index de séquence invalide : " + index);
        }
    }

    public void RegisterButtonPress(PuzzleButton button)
    {
        if (isLocked || lastAttemptWasCorrect)
            return;

        var currentSequence = sequences[currentSequenceIndex];
        if (!currentSequence.isActive || currentSequence.isSolved)
        {
            Debug.Log("Cette séquence n’est pas active ou déjà résolue.");
            return;
        }

        currentInput.Add(button);
        var correctSequence = currentSequence.buttons;

        if (currentInput.Count == correctSequence.Count)
        {
            bool correct = true;
            for (int i = 0; i < correctSequence.Count; i++)
            {
                if (currentInput[i] != correctSequence[i])
                {
                    correct = false;
                    break;
                }
            }

            if (correct)
            {
                lastAttemptWasCorrect = true;
                currentSequence.isSolved = true;
                Debug.Log("Bonne combinaison pour : " + currentSequence.sequenceName);

                HandleSequenceAction(currentSequence.sequenceName); // Appel de la logique personnalisée

                // Passe automatiquement à la séquence suivante
                if (currentSequenceIndex + 1 < sequences.Count)
                {
                    SetActiveSequence(currentSequenceIndex + 1);
                }
                else
                {
                    Debug.Log("Toutes les séquences ont été résolues !");
                }
            }
            else
            {
                Debug.Log("Mauvaise combinaison !");
                StartCoroutine(FlashAllError());
            }
        }
    }

    private void HandleSequenceAction(string sequenceName)
    {
        switch (sequenceName)
        {
            case "ActivationMachine":
                Debug.Log("Machine Activer");

                SetActiveSequence(1);

                ClickBlocker.SetActive(true);

                // debut de la coroutine de fin de chapitre 2
                StartCoroutine(Chap2EndSequence());

                break;

            case "TentativeDesamorcage":
                Debug.Log("Tentative Desamorcage");

                // Son Alarme
                // Carte qui brule

                break;

            default:
                Debug.Log("Aucune action spécifique pour cette séquence.");
                break;
        }
    }

    private IEnumerator FlashAllError()
    {
        isLocked = true;

        foreach (var b in currentInput)
        {
            StartCoroutine(b.FlashError());
        }

        yield return new WaitForSeconds(0.35f);
        ResetAllButtons();
        currentInput.Clear();
        isLocked = false;
    }

    public void ResetAllButtons()
    {
        foreach (var sequence in allSequences)
        {
            foreach (var b in sequence)
            {
                b.ResetButton();
            }
        }
        currentInput.Clear();
    }

    public bool IsCurrentSequenceCorrect()
    {
        return lastAttemptWasCorrect;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public IEnumerator Chap2EndSequence()
    {

        // Activer l'animation du trou noir
        // Son Activation Machine

        yield return StartCoroutine(LeaveCase.FadeAndTeleport());

        // Tourner les pages pour aller au chapitre 3
        // Activer le caneva si il ne l'est pas deja

        ButtonPage.SetActive(true);

        // AUTO FLIP (vers la double page 2 du chap 2)
        TurningPages1.TurningPage();
        yield return new WaitForSeconds(2f);
        // AUTO FLIP (vers la double page 1 du chap 3)
        TurningPages2.TurningPage();
        yield return new WaitForSeconds(1f);
        
        ClickBlocker.SetActive(false);
    }

}
