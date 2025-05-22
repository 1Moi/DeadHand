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


        switch (keyName)
        {
            case "RedKey":
                redKey.SetActive(true);
                break;
            case "GreenKey":
                greenKey.SetActive(true);
                break;
            case "BlueKey":
                blueKey.SetActive(true);
                break;
            default:
                Debug.LogWarning("Nom de clef inconnu : " + keyName);
                yield break;
        }


        yield return new WaitForSeconds(autoCloseDelay);

        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        if (canvasRoot != null)
            canvasRoot.SetActive(false);
    }
}
