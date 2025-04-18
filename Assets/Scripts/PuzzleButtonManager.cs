using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PuzzleButtonSequence
{
    public List<PuzzleButton> buttons = new List<PuzzleButton>();
}

public class PuzzleButtonManager : MonoBehaviour
{
    [Header("Séquences des puzzles")]
    public List<PuzzleButtonSequence> sequencesInspector = new List<PuzzleButtonSequence>();

    private List<List<PuzzleButton>> allSequences = new List<List<PuzzleButton>>();
    private List<PuzzleButton> currentInput = new List<PuzzleButton>();
    private int currentSequenceIndex = 0;
    private bool lastAttemptWasCorrect = false;
    private bool isLocked = false;

    void Awake()
    {
        allSequences.Clear();
        foreach (var seq in sequencesInspector)
        {
            foreach (var btn in seq.buttons)
            {
                btn.manager = this;
            }
            allSequences.Add(seq.buttons);
        }

        SetActiveSequence(0); // Initialise avec la première séquence
    }

    public void SetActiveSequence(int index)
    {
        if (index >= 0 && index < allSequences.Count)
        {
            ResetAllButtons();
            currentSequenceIndex = index;
            currentInput.Clear();
            lastAttemptWasCorrect = false;
            isLocked = false;
        }
        else
        {
            Debug.LogWarning("Sequence index invalide : " + index);
        }
    }

    public void RegisterButtonPress(PuzzleButton button)
    {
        if (isLocked || lastAttemptWasCorrect)
            return;

        currentInput.Add(button);
        var correctSequence = allSequences[currentSequenceIndex];

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
                Debug.Log("Bonne combinaison !");

                // Passe automatiquement à la séquence suivante
                if (currentSequenceIndex + 1 < allSequences.Count)
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
}