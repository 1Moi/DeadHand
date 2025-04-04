using UnityEngine;
using TMPro;
using System.Collections;

public class RewardAnimator : MonoBehaviour
{
    public GameObject canvasRoot; // Le Canvas qui contient l'animation
    public GameObject rewardPanel; // RewardAnimation root
    public GameObject redKey;
    public GameObject greenKey;
    public GameObject blueKey;
    public TextMeshProUGUI rewardText;
    public float autoCloseDelay = 2.5f;
    public float delayBeforeAppear = 0.2f; // Petit délai avant apparition

    public void ShowReward(string keyName)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(keyName));
    }

    IEnumerator ShowRoutine(string keyName)
    {
        yield return new WaitForSeconds(delayBeforeAppear);

        if (canvasRoot != null)
            canvasRoot.SetActive(true);

        rewardPanel.SetActive(true);

        // Désactiver toutes les clés
        redKey.SetActive(false);
        greenKey.SetActive(false);
        blueKey.SetActive(false);

        string displayText = "";

        switch (keyName)
        {
            case "RedKey":
                redKey.SetActive(true);
                displayText = "la clef rouge";
                break;
            case "GreenKey":
                greenKey.SetActive(true);
                displayText = "la clef verte";
                break;
            case "BlueKey":
                blueKey.SetActive(true);
                displayText = "la clef bleue";
                break;
            default:
                Debug.LogWarning("Nom de clef inconnu : " + keyName);
                yield break;
        }

        rewardText.text = $"Vous avez obtenu {displayText} !";

        yield return new WaitForSeconds(autoCloseDelay);

        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        if (canvasRoot != null)
            canvasRoot.SetActive(false);
    }
}
