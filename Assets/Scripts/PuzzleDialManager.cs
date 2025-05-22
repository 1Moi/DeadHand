using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PuzzleDialManager : MonoBehaviour
{
    [Header("Liste des dials dans l'ordre")]
    public List<PuzzleDial> dials;

    [Header("Combinaison correcte")]
    public List<int> correctCombination; // Exemple : [2,0,1,3]

    [Header("UI de fin")]
    public Image fadeImage; // Image noire avec alpha à modifier
    public TMP_Text dialogueText;
    public TMP_Text continueText;
    public GameObject canvasToActivate;
    public Camera mainCamera;

    [TextArea]
    public string message;
    public float typingSpeed = 0.05f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float fadeHoldDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip AudioClick;
    [SerializeField] private AudioClip AudioWin;
    [SerializeField] private AudioClip AudioAmbiance;
    [SerializeField] private AudioClip[] audioType;


    private bool combinationValidated = false;

    public void CheckCombination()
    {
        if (combinationValidated) return;

        if (dials.Count != correctCombination.Count)
        {
            Debug.LogWarning("Combinaison incorrectement configurée.");
            return;
        }

        for (int i = 0; i < dials.Count; i++)
        {
            if (dials[i].GetCurrentIndex() != correctCombination[i])
            {
                Debug.Log("Mauvaise combinaison.");
                return;
            }
        }

        Debug.Log("Bonne combinaison !");
        combinationValidated = true;
        if (AudioClick != null)
            GlobalSoundManager.PlaySFX(AudioWin);
        StartCoroutine(SequenceDeFin());
        if (AudioAmbiance != null)
            GlobalSoundManager.PlaySFX(AudioAmbiance);
        GlobalSoundManager.FadeOutMenuMusic(0.2f);
    }

    private IEnumerator SequenceDeFin()
    {
        yield return StartCoroutine(FadeIn());

        // Affichage lettre par lettre du texte
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
            dialogueText.color = Color.white;
            dialogueText.text = "";
            foreach (char c in message)
            {
                dialogueText.text += c;
                if (audioType != null && audioType.Length > 0)
                {
                    int indexAudio = Random.Range(0, audioType.Length);
                    GlobalSoundManager.PlaySFX(audioType[indexAudio]);
                }
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        if (continueText != null)
        {
            continueText.gameObject.SetActive(true);
            StartCoroutine(BlinkContinueText());
            continueText.color = Color.white;
            continueText.gameObject.SetActive(true);
        }

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        if (mainCamera != null)
        {
            Vector3 pos = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(0f, pos.y, pos.z);
        }

        if (canvasToActivate != null)
            canvasToActivate.SetActive(true);

        if (dialogueText != null)
        {
            yield return StartCoroutine(FadeOutText(dialogueText));
            dialogueText.gameObject.SetActive(false);
        }

        if (continueText != null)
            continueText.gameObject.SetActive(false);

        yield return StartCoroutine(FadeOut());
        GlobalSoundManager.PlaySFX(AudioClick);
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.enabled = true;

        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeInDuration);
            fadeImage.color = c;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;

        yield return new WaitForSeconds(fadeHoldDuration);

        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(t / fadeOutDuration);
            fadeImage.color = c;
            yield return null;
        }

        fadeImage.enabled = false;
    }

    private IEnumerator BlinkContinueText()
    {
        float blinkSpeed = 0.5f;
        Color originalColor = continueText.color;
        while (continueText.gameObject.activeSelf)
        {
            continueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            yield return new WaitForSeconds(blinkSpeed);
            continueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            yield return new WaitForSeconds(blinkSpeed);
        }
    }

    private IEnumerator FadeOutText(TMP_Text text)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color originalColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / duration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
            yield return null;
        }

        text.text = "";
        text.color = originalColor;
    }
}
